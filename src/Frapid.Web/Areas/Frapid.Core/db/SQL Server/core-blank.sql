-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/SQL Server/1.x/1.0/src/00.db core/db-roles.sql --<--<--
IF NOT EXISTS 
(
	SELECT * FROM sys.database_principals 
	WHERE name = 'frapid_db_user'
)
BEGIN
    CREATE USER frapid_db_user FOR LOGIN frapid_db_user;
    EXEC sp_addrolemember 'db_owner', 'frapid_db_user';
END;
GO


IF NOT EXISTS 
(
	SELECT * FROM sys.database_principals 
	WHERE name = 'report_user'
)
BEGIN
    CREATE USER report_user FOR LOGIN report_user;
    EXEC sp_addrolemember 'db_datareader', 'report_user';
END;
GO


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/SQL Server/1.x/1.0/src/01.poco.sql --<--<--


IF OBJECT_ID('dbo.get_app_data_type') IS NOT NULL
DROP FUNCTION dbo.get_app_data_type;

GO

CREATE FUNCTION dbo.get_app_data_type(@db_data_type national character varying(100))
RETURNS national character varying(100)
BEGIN
    IF(@db_data_type IN('smallint', 'tinyint'))
    BEGIN
        RETURN 'short';
    END;

    IF(@db_data_type IN('int4', 'int', 'integer'))
    BEGIN
        RETURN 'int';
    END;

    IF(@db_data_type IN('varchar', 'nvarchar', 'character varying', 'text'))
    BEGIN
        RETURN 'string';
    END;
    
    IF(@db_data_type IN('date', 'time', 'datetimeoffset'))
    BEGIN
        RETURN 'System.DateTime';
    END;
    
    IF(@db_data_type IN('bit'))
    BEGIN
        RETURN 'bool';
    END;

    RETURN @db_data_type;
END;

GO



GO

IF OBJECT_ID('dbo.poco_get_tables') IS NOT NULL
DROP FUNCTION dbo.poco_get_tables;

GO

CREATE FUNCTION dbo.poco_get_tables(@schema national character varying(100))
RETURNS @result TABLE
(
	table_schema				national character varying(100),
	table_name					national character varying(100),
	table_type					national character varying(100), 
	has_duplicate				bit
) 
AS
BEGIN
    INSERT INTO @result
    SELECT 
        information_schema.tables.table_schema, 
        information_schema.tables.table_name, 
        information_schema.tables.table_type,
        0
    FROM information_schema.tables 
    WHERE (information_schema.tables.table_type='BASE TABLE' OR information_schema.tables.table_type='VIEW')
    AND information_schema.tables.table_schema = @schema;


    UPDATE  @result
    SET has_duplicate = 1
    FROM  @result result
    INNER JOIN
    (
        SELECT
            information_schema.tables.table_name,
            COUNT(information_schema.tables.table_name) AS table_count
        FROM information_schema.tables
        GROUP BY information_schema.tables.table_name
        
    ) subquery
    ON subquery.table_name = result.table_name
    WHERE subquery.table_count > 1;

    

    RETURN;
END;
GO


IF OBJECT_ID('dbo.parse_default') IS NOT NULL
DROP PROCEDURE dbo.parse_default;

GO

CREATE PROCEDURE dbo.parse_default
(
	@default			national character varying(MAX), 
	@parsed				national character varying(MAX) OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @result		TABLE (parsed national character varying(MAX));
	DECLARE @sql		national character varying(MAX);
	
	IF(@default IS NOT NULL)
	BEGIN
		BEGIN TRY
			SET @sql = 'SELECT ' + @default;

			INSERT INTO @result
			EXECUTE @parsed = sp_executesql @sql;

			SELECT @parsed = parsed
			FROM @result;
		END TRY
		BEGIN CATCH
		END CATCH
	END;
END;

GO

IF OBJECT_ID('dbo.is_primary_key') IS NOT NULL
DROP FUNCTION dbo.is_primary_key;

GO

CREATE FUNCTION dbo.is_primary_key(@schema national character varying(100), @table national character varying(100), @column national character varying(100))
RETURNS national character varying(100)
AS
BEGIN
	IF EXISTS
	(
		SELECT 
			1
		FROM information_schema.table_constraints
		INNER JOIN information_schema.key_column_usage
		ON information_schema.table_constraints.constraint_type = 'PRIMARY KEY' AND
		information_schema.table_constraints.constraint_name = key_column_usage.CONSTRAINT_NAME
		AND information_schema.key_column_usage.table_schema = @schema
		AND information_schema.key_column_usage.table_name=@table
		AND information_schema.key_column_usage.column_name = @column
	)
	BEGIN
		RETURN 'YES';
	END
	
	RETURN 'NO';
END

GO

IF OBJECT_ID('dbo.poco_get_table_function_definition') is not null
DROP PROCEDURE dbo.poco_get_table_function_definition;

GO

CREATE PROCEDURE dbo.poco_get_table_function_definition(@schema national character varying(100), @name national character varying(100))
AS
BEGIN
	DECLARE @total_rows			int;
	DECLARE @this_row			int = 0;
	DECLARE @default			national character varying(100);
	DECLARE @parsed				national character varying(100);

	DECLARE @result TABLE
	(
		row_id					int IDENTITY,
		id                      int,
		column_name             national character varying(100),
		nullable				national character varying(100),
		db_data_type            national character varying(100),
		value					national character varying(100),
		max_length              national character varying(100),
		primary_key				national character varying(100),
		data_type               national character varying(100),
		is_serial				bit DEFAULT(0)
	);

    IF EXISTS
    (
        SELECT *
        FROM information_schema.columns 
        WHERE table_schema=@schema
        AND table_name=@name
    )
    BEGIN
        INSERT INTO @result(id, column_name, nullable, db_data_type, value, max_length, primary_key, data_type)
        SELECT
			information_schema.columns.ordinal_position,
			information_schema.columns.column_name,
			information_schema.columns.is_nullable,
			CASE WHEN information_schema.columns.domain_name IS NOT NULL 
			THEN information_schema.columns.domain_name
			ELSE information_schema.columns.data_type END AS data_type,
			information_schema.columns.column_default,
			information_schema.columns.character_maximum_length,
			dbo.is_primary_key(@schema, @name, information_schema.columns.column_name),
			dbo.get_app_data_type(information_schema.columns.data_type)
        FROM information_schema.columns
        WHERE 1 = 1
        AND information_schema.columns.table_schema = @schema
        AND information_schema.columns.table_name = @name;

		SET @total_rows = @@ROWCOUNT;
    END;



	WHILE @this_row<@total_rows
	BEGIN
		SET @this_row = @this_row + 1;

		SELECT 
			@default = value
		FROM @result
		WHERE row_id=@this_row;

		EXECUTE dbo.parse_default @default, @parsed = @parsed OUTPUT;
		UPDATE @result
		SET value = @parsed
		WHERE row_id=@this_row;

		SET @parsed = NULL;
	END;

	UPDATE @result
	SET is_serial = COLUMNPROPERTY(OBJECT_ID(@schema + '.' + @name), column_name, 'IsIdentity');


	SELECT * FROM @result;
    RETURN;
END;

GO


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/SQL Server/1.x/1.0/src/01.types-domains-tables-and-constraints/0. types.sql --<--<--
IF OBJECT_ID('dbo.drop_schema') IS NOT NULL
EXECUTE dbo.drop_schema 'core';
GO

CREATE SCHEMA core;

GO

IF TYPE_ID(N'dbo.money_strict') IS NULL
BEGIN
	CREATE TYPE dbo.money_strict
	FROM DECIMAL(24, 4);
END;

IF TYPE_ID(N'dbo.money_strict2') IS NULL
BEGIN
	CREATE TYPE dbo.money_strict2
	FROM DECIMAL(24, 4);
END;

IF TYPE_ID(N'dbo.integer_strict') IS NULL
BEGIN
	CREATE TYPE dbo.integer_strict
	FROM integer;
END;

IF TYPE_ID(N'dbo.integer_strict2') IS NULL
BEGIN
	CREATE TYPE dbo.integer_strict2
	FROM integer;
END;

IF TYPE_ID(N'dbo.smallint_strict') IS NULL
BEGIN
	CREATE TYPE dbo.smallint_strict
	FROM smallint;
END;

IF TYPE_ID(N'dbo.smallint_strict2') IS NULL
BEGIN
	CREATE TYPE dbo.smallint_strict2
	FROM smallint;
END;

IF TYPE_ID(N'dbo.decimal_strict') IS NULL
BEGIN
	CREATE TYPE dbo.decimal_strict
	FROM decimal;
END;

IF TYPE_ID(N'dbo.decimal_strict2') IS NULL
BEGIN
	CREATE TYPE dbo.decimal_strict2
	FROM decimal;
END;

IF TYPE_ID(N'dbo.color') IS NULL
BEGIN
	CREATE TYPE dbo.color
	FROM national character varying(50);
END;

IF TYPE_ID(N'dbo.photo') IS NULL
BEGIN
	CREATE TYPE dbo.photo
	FROM national character varying(MAX);
END;


IF TYPE_ID(N'dbo.html') IS NULL
BEGIN
	CREATE TYPE dbo.html
	FROM national character varying(MAX);
END;


IF TYPE_ID(N'dbo.password') IS NULL
BEGIN
	CREATE TYPE dbo.password
	FROM national character varying(MAX);
END;


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/SQL Server/1.x/1.0/src/01.types-domains-tables-and-constraints/0.utilities.sql --<--<--
IF OBJECT_ID('dbo.drop_column') IS NOT NULL
DROP PROCEDURE dbo.drop_column;

GO 

CREATE PROCEDURE dbo.drop_column(@schema_name NVARCHAR(256), @table_name NVARCHAR(256), @column_name NVARCHAR(256))
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @sql  NVARCHAR(1000)

	SELECT @sql = COALESCE(@sql + CHAR(13), '') + 'ALTER TABLE [' + @schema_name + '].[' + @table_name + '] DROP CONSTRAINT [' + d.name + '];' + CHAR(13)
	FROM sys.tables t   
	JOIN sys.default_constraints d       
	ON d.parent_object_id = t.object_id  
	JOIN sys.schemas s
	ON s.schema_id = t.schema_id
	JOIN    sys.columns c      
	ON c.object_id = t.object_id      
	AND c.column_id = d.parent_column_id
	WHERE t.name = @table_name
	AND s.name = @schema_name 
	AND c.name = @column_name

	SET @sql = @sql + ' ALTER TABLE [' + @schema_name + '].[' + @table_name + '] DROP COLUMN [' + @column_name + '];' 

	EXECUTE (@sql)
END;

GO

IF OBJECT_ID('dbo.drop_schema') IS NOT NULL
DROP PROCEDURE dbo.drop_schema;

GO

CREATE PROCEDURE dbo.drop_schema(@name nvarchar(500), @showsql bit = 0)
AS
BEGIN
	SET XACT_ABORT ON;
    SET NOCOUNT ON;

      DECLARE @sql nvarchar(max);
      DECLARE @commands TABLE
        (
           row_id  int IDENTITY,
           command nvarchar(max)
        );

      INSERT INTO @commands
      SELECT 'SET XACT_ABORT ON;';

      INSERT INTO @commands
      SELECT 'BEGIN TRY';

      INSERT INTO @commands
      SELECT '    BEGIN TRANSACTION;';

      INSERT INTO @commands
      SELECT '    ALTER TABLE ['
             + CONVERT(sysname, schema_name(o2.schema_id))
             + '].[' + CONVERT(sysname, o2.NAME)
             + '] DROP CONSTRAINT ['
             + CONVERT(sysname, Object_name(f.object_id))
             + '];'
      FROM   sys.all_objects o1,
             sys.all_objects o2,
             sys.all_columns c1,
             sys.all_columns c2,
             sys.foreign_keys f
             INNER JOIN sys.foreign_key_columns k
                     ON ( k.constraint_object_id = f.object_id )
             INNER JOIN sys.indexes i
                     ON ( f.referenced_object_id = i.object_id
                          AND f.key_index_id = i.index_id )
      WHERE  o1.object_id = f.referenced_object_id
             AND o2.object_id = f.parent_object_id
             AND c1.object_id = f.referenced_object_id
             AND c2.object_id = f.parent_object_id
             AND c1.column_id = k.referenced_column_id
             AND c2.column_id = k.parent_column_id
             AND CONVERT(sysname, schema_name(o1.schema_id)) = @name;

      INSERT INTO @commands
      SELECT '    ALTER TABLE [' + @name + '].['
             + Object_name(sys.objects.parent_object_id)
             + '] DROP CONSTRAINT [' + sys.objects.NAME
             + '];'
      FROM   sys.objects
      WHERE  schema_id = schema_id(@name)
             AND sys.objects.type IN ( 'C', 'PK' )
             AND sys.objects.parent_object_id IN (SELECT sys.objects.object_id
                                                  FROM   sys.objects
                                                  WHERE  sys.objects.type = 'U')
      ;

      INSERT INTO @commands
      SELECT '    ALTER TABLE [' + @name + '].['
             + sys.tables.NAME + '] DROP CONSTRAINT ['
             + default_constraints.NAME + '];'
      FROM   sys.all_columns
             INNER JOIN sys.tables
                     ON all_columns.object_id = tables.object_id
             INNER JOIN sys.schemas
                     ON tables.schema_id = schemas.schema_id
             INNER JOIN sys.default_constraints
                     ON all_columns.default_object_id =
                        default_constraints.object_id
      WHERE  schemas.NAME = @name;

      INSERT INTO @commands
      SELECT '    DROP TRIGGER [' + @name + '].['
             + sys.objects.NAME + '];'
      FROM   sys.objects
      WHERE  sys.objects.type IN ( 'TR', 'TA' )
             AND schema_id = schema_id(@name);

      INSERT INTO @commands
      SELECT '    DROP VIEW [' + @name + '].['
             + information_schema.tables.table_name + '];'
      FROM   information_schema.tables
      WHERE  table_schema = @name
             AND table_type = 'VIEW';

      INSERT INTO @commands
      SELECT '    DROP TABLE [' + @name + '].['
             + information_schema.tables.table_name + '];'
      FROM   information_schema.tables
      WHERE  table_schema = @name
             AND table_type = 'BASE TABLE';

      INSERT INTO @commands
      SELECT '    DROP FUNCTION [' + @name + '].['
             + sys.objects.NAME + '];'
      FROM   sys.objects
      WHERE  sys.objects.type IN ( 'FN', 'AF', 'FS', 'FT', 'TF' )
             AND schema_id = schema_id(@name)

      INSERT INTO @commands
      SELECT '    DROP PROCEDURE [' + @name + '].['
             + sys.objects.NAME + '];'
      FROM   sys.objects
      WHERE  sys.objects.type IN ( 'P', 'PC', 'FS', 'FT', 'RF' )
             AND schema_id = schema_id(@name)

      INSERT INTO @commands
      SELECT '    DROP SYNONYM [' + @name + '].['
             + sys.objects.NAME + '];'
      FROM   sys.objects
      WHERE  sys.objects.type IN ( 'SN' )
             AND schema_id = schema_id(@name)

      INSERT INTO @commands
      SELECT '    DROP RULE [' + @name + '].['
             + sys.objects.NAME + '];'
      FROM   sys.objects
      WHERE  sys.objects.type IN ( 'R' )
             AND schema_id = schema_id(@name)

      DELETE FROM @commands
      WHERE  command IS NULL;

      INSERT INTO @commands
      SELECT '    DROP SCHEMA ' + @name + ';';

      INSERT INTO @commands
      SELECT '    COMMIT TRANSACTION;';

      INSERT INTO @commands
      SELECT 'END TRY';

      INSERT INTO @commands
      SELECT 'BEGIN CATCH';

      INSERT INTO @commands
      SELECT '    IF (@@TRANCOUNT > 0)';

      INSERT INTO @commands
      SELECT '    BEGIN';

      INSERT INTO @commands
      SELECT '        IF XACT_STATE() <> 0';

      INSERT INTO @commands
      SELECT '        ROLLBACK TRANSACTION;';

      INSERT INTO @commands
      SELECT
'         SELECT error_message() as errormessage, error_number() as erronumber, error_state() as errorstate, error_procedure() as errorprocedure, error_line() as errorline;'
    ;

    INSERT INTO @commands
    SELECT '    END;';

    INSERT INTO @commands
    SELECT 'END CATCH';

    SELECT @sql = COALESCE(@sql, '', '') + command + Char(13)
    FROM   @commands
    ORDER  BY row_id;
	
	IF @showsql = 1
	BEGIN
		SELECT @sql;	
	END
	
    EXECUTE sp_executesql @sql;
END;

GO

IF OBJECT_ID('core.split') IS NOT NULL
DROP FUNCTION core.split;

GO

CREATE FUNCTION core.split
(
	@members national character varying(MAX)
)
RETURNS 
@split TABLE 
(
	member			national character varying(4000)
)
AS
BEGIN
	DECLARE @xml xml;
	
	SET @xml = N'<ss><s>' + replace(@members,',','</s><s>') + '</s></ss>'

	INSERT INTO @split
	SELECT r.value('.','national character varying(4000)')
	FROM @xml.nodes('//ss/s') as records(r)

	RETURN
END

GO

IF OBJECT_ID('core.array_split') IS NOT NULL
DROP FUNCTION core.array_split;

GO

CREATE FUNCTION core.array_split
(
	@members national character varying(MAX)
)
RETURNS 
@split TABLE 
(
	member			national character varying(4000)
)
AS
BEGIN
	INSERT INTO @split
	SELECT * FROM 
	core.split(REPLACE(REPLACE(@members, '{', ''), '}', ''))
	RETURN
END

GO


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/SQL Server/1.x/1.0/src/01.types-domains-tables-and-constraints/1. tables-and-constraints.sql --<--<--
CREATE TABLE core.countries
(
    country_code                            	national character varying(12) PRIMARY KEY,
    country_name                            	national character varying(100) NOT NULL,
    audit_user_id                           	integer,
	audit_ts									datetimeoffset NULL DEFAULT(getutcdate())
);

CREATE TABLE core.apps
(
    app_name                                    national character varying(100) PRIMARY KEY,
    name                                        national character varying(100),
    version_number                              national character varying(100),
    publisher                                   national character varying(500),
    published_on                                date,
    icon                                        national character varying(100),
    landing_url                                 national character varying(500)
);

CREATE UNIQUE INDEX apps_app_name_uix
ON core.apps(app_name);

CREATE TABLE core.app_dependencies
(
    app_dependency_id                           int IDENTITY PRIMARY KEY,
    app_name                                    national character varying(100) REFERENCES core.apps,
    depends_on                                  national character varying(100) REFERENCES core.apps
);


CREATE TABLE core.menus
(
    menu_id                                     int IDENTITY PRIMARY KEY,
    sort                                        integer,
    app_name                                    national character varying(100) NOT NULL REFERENCES core.apps,
    menu_name                                   national character varying(100) NOT NULL,
    url                                         national character varying(500),
    icon                                        national character varying(100),
    parent_menu_id                              integer REFERENCES core.menus
);

CREATE UNIQUE INDEX menus_app_name_menu_name_uix
ON core.menus(app_name, menu_name);

CREATE TABLE core.menu_locale
(
    menu_locale_id                              int IDENTITY PRIMARY KEY,
    menu_id                                     integer NOT NULL REFERENCES core.menus,
    culture                                     national character varying(12) NOT NULL,
    menu_text                                   national character varying(250) NOT NULL
);

CREATE TABLE core.offices
(
    office_id                                   int IDENTITY PRIMARY KEY,
    office_code                                 national character varying(12) NOT NULL,
    office_name                                 national character varying(150) NOT NULL,
    nick_name                                   national character varying(50),
    registration_date                           date,
	currency_code								national character varying(12),
    po_box                                      national character varying(128),
    address_line_1                              national character varying(128),   
    address_line_2                              national character varying(128),
    street                                      national character varying(50),
    city                                        national character varying(50),
    state                                       national character varying(50),
    zip_code                                    national character varying(24),
    country                                     national character varying(50),
    phone                                       national character varying(24),
    fax                                         national character varying(24),
    email                                       national character varying(128),
    url                                         national character varying(50),
    logo                                        dbo.photo,
    parent_office_id                            integer NULL REFERENCES core.offices,
    audit_user_id                               integer NULL,
    audit_ts                                    datetimeoffset NULL DEFAULT(getutcdate())
);

CREATE TABLE core.frequencies
(
    frequency_id								int IDENTITY PRIMARY KEY,
    frequency_code								national character varying(12) NOT NULL,
    frequency_name								national character varying(50) NOT NULL
);


CREATE UNIQUE INDEX frequencies_frequency_code_uix
ON core.frequencies(frequency_code);

CREATE UNIQUE INDEX frequencies_frequency_name_uix
ON core.frequencies(frequency_name);


CREATE TABLE core.verification_statuses
(
    verification_status_id						smallint PRIMARY KEY,
    verification_status_name					national character varying(128) NOT NULL
);


CREATE TABLE core.week_days
(
	week_day_id									integer NOT NULL CHECK(week_day_id > =1 AND week_day_id < =7) PRIMARY KEY,
	week_day_code								national character varying(12) NOT NULL UNIQUE,
	week_day_name								national character varying(50) NOT NULL UNIQUE
);

CREATE TABLE core.genders
(
	gender_code									national character varying(4) NOT NULL PRIMARY KEY,
	gender_name									national character varying(50) NOT NULL UNIQUE,
	audit_user_id								integer NULL,
	audit_ts									datetimeoffset NULL DEFAULT(getutcdate())    
);

CREATE TABLE core.marital_statuses
(
	marital_status_id							int IDENTITY NOT NULL PRIMARY KEY,
	marital_status_code							national character varying(12) NOT NULL UNIQUE,
	marital_status_name							national character varying(128) NOT NULL,
	is_legally_recognized_marriage				bit NOT NULL DEFAULT(0),
	audit_user_id								integer NULL,    
	audit_ts									datetimeoffset NULL DEFAULT(getutcdate())
);


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/SQL Server/1.x/1.0/src/04.default-values/01.default-values.sql --<--<--
INSERT INTO core.offices(office_code, office_name)
SELECT 'DEF', 'Default';

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/SQL Server/1.x/1.0/src/05.scrud-views/core.office_scrud_view.sql --<--<--
IF OBJECT_ID('core.office_scrud_view') IS NOT NULL
DROP VIEW core.office_scrud_view;

GO

CREATE VIEW core.office_scrud_view
AS
SELECT
	core.offices.office_id,
	core.offices.office_code,
	core.offices.office_name,
	core.offices.currency_code,
	parent_office.office_code + ' (' + parent_office.office_name + ')' AS parent_office
FROM core.offices
LEFT JOIN core.offices AS parent_office
ON parent_office.office_id = core.offices.parent_office_id;

GO

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/SQL Server/1.x/1.0/src/06.functions-and-logic/core.create_app.sql --<--<--
IF OBJECT_ID('core.create_app') IS NOT NULL
DROP PROCEDURE core.create_app;

GO

CREATE PROCEDURE core.create_app
(
    @app_name                                   national character varying(100),
    @name                                       national character varying(100),
    @version_number                             national character varying(100),
    @publisher                                  national character varying(100),
    @published_on                               date,
    @icon                                       national character varying(100),
    @landing_url                                national character varying(100),
    @dependencies                               national character varying(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM core.apps
        WHERE LOWER(core.apps.app_name) = LOWER(@app_name)
    )
    BEGIN
        UPDATE core.apps
        SET
            name = @name,
            version_number = @version_number,
            publisher = @publisher,
            published_on = @published_on,
            icon = @icon,
            landing_url = @landing_url
        WHERE
            app_name = @app_name;
    END
    ELSE
    BEGIN
        INSERT INTO core.apps(app_name, name, version_number, publisher, published_on, icon, landing_url)
        SELECT @app_name, @name, @version_number, @publisher, @published_on, @icon, @landing_url;
    END;

    DELETE FROM core.app_dependencies
    WHERE app_name = @app_name;

	IF(ltrim(rtrim(COALESCE(@dependencies, ''))) != '')
	BEGIN
		INSERT INTO core.app_dependencies(app_name, depends_on)
		SELECT @app_name, member
		FROM core.array_split(@dependencies);
	END;
END;

GO

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/SQL Server/1.x/1.0/src/06.functions-and-logic/core.create_menu.sql --<--<--
IF OBJECT_ID('core.create_menu2') IS NOT NULL
DROP PROCEDURE core.create_menu2;

GO

CREATE PROCEDURE core.create_menu2
(
    @sort                                       integer,
    @app_name                                   national character varying(100),
    @menu_name                                  national character varying(100),
    @url                                        national character varying(100),
    @icon                                       national character varying(100),
    @parent_menu_id                             integer
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @menu_id                            integer;
    
    IF EXISTS
    (
       SELECT 1
       FROM core.menus
       WHERE app_name = @app_name
       AND menu_name = @menu_name
    )
    BEGIN
        UPDATE core.menus
        SET
            sort = @sort,
            url = @url,
            icon = @icon,
            parent_menu_id = @parent_menu_id
       WHERE app_name = @app_name
       AND menu_name = @menu_name;
       
		SELECT
			@menu_id = menu_id
		FROM core.menus
		WHERE app_name = @app_name
		AND menu_name = @menu_name;
    END
    ELSE
    BEGIN
        INSERT INTO core.menus(sort, app_name, menu_name, url, icon, parent_menu_id)
        SELECT @sort, @app_name, @menu_name, @url, @icon, @parent_menu_id;
        
		SET @menu_id = SCOPE_IDENTITY();
    END;

    SELECT @menu_id;
END;

GO

IF OBJECT_ID('core.create_menu') IS NOT NULL
DROP PROCEDURE core.create_menu;

GO

CREATE PROCEDURE core.create_menu
(
    @app_name                                   national character varying(100),
    @menu_name                                  national character varying(100),
    @url                                        national character varying(100),
    @icon                                       national character varying(100),
    @parent_menu_name                           national character varying(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @parent_menu_id                     integer;

    SELECT @parent_menu_id = menu_id
    FROM core.menus
    WHERE menu_name = @parent_menu_name
    AND app_name = @app_name;
	
	PRINT @parent_menu_id;
	
    EXECUTE core.create_menu2 0, @app_name, @menu_name, @url, @icon, @parent_menu_id;
END;


GO

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/SQL Server/1.x/1.0/src/06.functions-and-logic/core.get_office_id_by_office_name.sql --<--<--
IF OBJECT_ID('core.get_office_id_by_office_name') IS NOT NULL
DROP FUNCTION core.get_office_id_by_office_name;

GO

CREATE FUNCTION core.get_office_id_by_office_name
(
	@office_name national character varying(100)
)
RETURNS integer
AS
BEGIN
    RETURN
    (
		SELECT core.offices.office_id
		FROM core.offices
		WHERE core.offices.office_name = @office_name
    );
END;

GO

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/SQL Server/1.x/1.0/src/06.functions-and-logic/core.get_office_ids.sql --<--<--
IF OBJECT_ID('core.get_office_ids') IS NOT NULL
DROP FUNCTION core.get_office_ids;

GO

CREATE FUNCTION core.get_office_ids(@root_office_id integer)
RETURNS @results TABLE
(
	office_id		integer
)
AS
BEGIN
    WITH office_cte(office_id, path) AS (
     SELECT
        tn.office_id,  CAST(tn.office_id AS varchar(MAX)) AS path
        FROM core.offices AS tn WHERE tn.office_id = @root_office_id
    UNION ALL
     SELECT
        c.office_id, (p.path + '->' + CAST(c.office_id AS varchar(MAX)))
        FROM office_cte AS p, core.offices AS c WHERE parent_office_id = p.office_id
    )
	
	INSERT INTO @results
    SELECT office_id FROM office_cte;
    
    RETURN;
END;

GO

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/SQL Server/1.x/1.0/src/10.policy/access_policy.sql --<--<--


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/SQL Server/1.x/1.0/src/99.ownership.sql --<--<--
IF NOT EXISTS
(
	SELECT * FROM sys.database_principals
	WHERE name = 'frapid_db_user'
)
BEGIN
CREATE USER frapid_db_user FROM LOGIN frapid_db_user;
END
GO

EXEC sp_addrolemember  @rolename = 'db_owner', @membername  = 'frapid_db_user'
GO

IF NOT EXISTS
(
	SELECT * FROM sys.database_principals
	WHERE name = 'report_user'
)
BEGIN
CREATE USER report_user FROM LOGIN report_user;
END
GO

EXEC sp_addrolemember  @rolename = 'db_datareader', @membername  = 'report_user'
GO


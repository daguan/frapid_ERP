IF EXISTS (SELECT *
       FROM   sys.objects
       WHERE  type = 'P'
              AND NAME = 'drop_schema')
DROP PROCEDURE dbo.drop_schema;

GO

CREATE PROCEDURE dbo.drop_schema(@name nvarchar(500), @showsql bit = 0)
AS
BEGIN
    SET NOCOUNT ON;

      DECLARE @sql nvarchar(max);
      DECLARE @commands TABLE
        (
           row_id  int IDENTITY,
           command nvarchar(max)
        );

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

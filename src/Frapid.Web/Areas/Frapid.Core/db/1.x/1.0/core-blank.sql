-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/1.x/1.0/src/00.db core/casts.sql --<--<--
DROP FUNCTION IF EXISTS text_to_bigint(text) CASCADE;
CREATE FUNCTION text_to_bigint(text) RETURNS bigint AS 'SELECT int8in(textout($1));' LANGUAGE SQL STRICT IMMUTABLE;
CREATE CAST (text AS bigint) WITH FUNCTION text_to_bigint(text) AS IMPLICIT;

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/1.x/1.0/src/00.db core/extensions.sql --<--<--
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/1.x/1.0/src/00.db core/postgresql-roles.sql --<--<--
DO
$$
BEGIN
    IF NOT EXISTS (SELECT * FROM pg_catalog.pg_roles WHERE rolname = 'frapid_db_user') THEN
        CREATE ROLE frapid_db_user WITH LOGIN PASSWORD 'change-on-deployment';
    END IF;

    COMMENT ON ROLE frapid_db_user IS 'The default user for frapid databases.';

    EXECUTE 'ALTER DATABASE ' || current_database() || ' OWNER TO frapid_db_user;';
END
$$
LANGUAGE plpgsql;

DO
$$
BEGIN
    IF NOT EXISTS (SELECT * FROM pg_catalog.pg_roles WHERE rolname = 'report_user') THEN
        CREATE ROLE report_user WITH LOGIN PASSWORD 'change-on-deployment';
    END IF;

    COMMENT ON ROLE report_user IS 'This user account is used by the Reporting Engine to run ad-hoc queries. It is strictly advised for this user to only have a read-only access to the database.';
END
$$
LANGUAGE plpgsql;


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/1.x/1.0/src/01.poco.sql --<--<--
CREATE OR REPLACE FUNCTION poco_get_table_function_annotation(
    IN _schema_name text,
    IN _table_name text)
  RETURNS TABLE(id integer, column_name text, is_nullable text, udt_name text, column_default text, max_length integer, is_primary_key text) AS
$BODY$
    DECLARE _args           text;
BEGIN
    DROP TABLE IF EXISTS temp_annonation;
    CREATE TEMPORARY TABLE temp_annonation
    (
        id                      SERIAL,
        column_name             text,
        is_nullable             text DEFAULT('NO'),
        udt_name                text,
        column_default          text,
        max_length              integer DEFAULT(0),
        is_primary_key          text DEFAULT('NO')
    ) ON COMMIT DROP;


    SELECT
        pg_catalog.pg_get_function_arguments(pg_proc.oid) AS arguments
    INTO
        _args
    FROM pg_proc
    INNER JOIN pg_namespace
    ON pg_proc.pronamespace = pg_namespace.oid
    INNER JOIN pg_type
    ON pg_proc.prorettype = pg_type.oid
    INNER JOIN pg_namespace type_namespace
    ON pg_type.typnamespace = type_namespace.oid
    WHERE typname != ANY(ARRAY['trigger'])
    AND pg_namespace.nspname = _schema_name
    AND proname::text = _table_name;

    INSERT INTO temp_annonation(column_name, udt_name)
    SELECT split_part(trim(unnest(regexp_split_to_array(_args, ','))), ' ', 1), trim(unnest(regexp_split_to_array(_args, ',')));

    UPDATE temp_annonation
    SET udt_name = TRIM(REPLACE(temp_annonation.udt_name, temp_annonation.column_name, ''));

    
    RETURN QUERY
    SELECT * FROM temp_annonation;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;

  
-- Function: poco_get_table_function_definition(text, text)

-- DROP FUNCTION poco_get_table_function_definition(text, text);

CREATE OR REPLACE FUNCTION poco_get_table_function_definition(
    IN _schema text,
    IN _name text)
  RETURNS TABLE(id bigint, column_name text, is_nullable text, udt_name text, column_default text, max_length integer, is_primary_key text) AS
$BODY$
    DECLARE _oid            oid;
    DECLARE _typoid         oid;
BEGIN
    CREATE TEMPORARY TABLE temp_poco
    (
        id                      bigint,
        column_name             text,
        is_nullable             text,
        udt_name                text,
        column_default          text,
        max_length              integer default(0),
        is_primary_key          text
    ) ON COMMIT DROP;

    SELECT        
        pg_proc.oid,
        pg_proc.prorettype
    INTO 
        _oid,
        _typoid
    FROM pg_proc
    INNER JOIN pg_namespace
    ON pg_proc.pronamespace = pg_namespace.oid
    WHERE pg_proc.proname=_name
    AND pg_namespace.nspname=_schema
    LIMIT 1;


    IF EXISTS
    (
        SELECT 1
        FROM information_schema.columns 
        WHERE table_schema=_schema 
        AND table_name=_name
    ) THEN
        INSERT INTO temp_poco
        SELECT
            row_number() OVER(ORDER BY attnum),
            attname AS column_name,
            CASE WHEN attnotnull THEN 'NO' ELSE 'YES' END,
            pg_type.typname,
            public.parse_default(adsrc),
            CASE WHEN atttypmod <> -1 
            THEN atttypmod - 4
            ELSE 0 END,
            CASE WHEN indisprimary THEN 'YES' ELSE 'NO' END
        FROM   pg_attribute
        LEFT JOIN pg_index
        ON pg_attribute.ATTRELID = pg_index.indrelid
        AND pg_attribute.attnum = ANY(pg_index.indkey)
        AND pg_index.indisprimary
        INNER JOIN pg_type
        ON pg_attribute.atttypid = pg_type.oid
        LEFT   JOIN pg_catalog.pg_attrdef
        ON (pg_attribute.attrelid, pg_attribute.attnum) = (pg_attrdef.adrelid,  pg_attrdef.adnum)
        WHERE  attrelid = (_schema || '.' || _name)::regclass
        AND    attnum > 0
        AND    NOT attisdropped
        ORDER  BY attnum;
        
        RETURN QUERY
        SELECT * FROM temp_poco;
        RETURN;
    END IF;

    IF EXISTS(SELECT * FROM pg_type WHERE oid = _typoid AND typtype='c') THEN
        --Composite Type
        INSERT INTO temp_poco
        SELECT
            row_number() OVER(ORDER BY attnum),
            attname::text               AS column_name,
            'NO'::text                  AS is_nullable, 
            format_type(t.oid,NULL)     AS udt_name,
            ''::text                    AS column_default
        FROM pg_attribute att
        JOIN pg_type t ON t.oid=atttypid
        JOIN pg_namespace nsp ON t.typnamespace=nsp.oid
        LEFT OUTER JOIN pg_type b ON t.typelem=b.oid
        LEFT OUTER JOIN pg_collation c ON att.attcollation=c.oid
        LEFT OUTER JOIN pg_namespace nspc ON c.collnamespace=nspc.oid
        WHERE att.attrelid=(SELECT typrelid FROM pg_type WHERE pg_type.oid = _typoid)
        AND att.attnum > 0
        ORDER by attnum;

        RETURN QUERY
        SELECT * FROM temp_poco;
        RETURN;
    END IF;

    IF(_oid IS NOT NULL) THEN
        INSERT INTO temp_poco
        WITH procs
        AS
        (
            SELECT 
            row_number() OVER(ORDER BY proallargtypes),
            explode_array(proargnames) as column_name,
            explode_array(proargmodes) as column_mode,
            explode_array(proallargtypes) as argument_type
            FROM pg_proc
            WHERE oid = _oid
        )
        SELECT
            row_number() OVER(ORDER BY 1),
            procs.column_name::text,
            'NO'::text AS is_nullable, 
            format_type(procs.argument_type, null) as udt_name,
            ''::text AS column_default
        FROM procs
        WHERE column_mode=ANY(ARRAY['t', 'o']);

        RETURN QUERY
        SELECT * FROM temp_poco;
        RETURN;
    END IF;

    INSERT INTO temp_poco
    SELECT 
        row_number() OVER(ORDER BY attnum),
        attname::text               AS column_name,
        'NO'::text                  AS is_nullable, 
        format_type(t.oid,NULL)     AS udt_name,
        ''::text                    AS column_default
    FROM pg_attribute att
    JOIN pg_type t ON t.oid=atttypid
    JOIN pg_namespace nsp ON t.typnamespace=nsp.oid
    LEFT OUTER JOIN pg_type b ON t.typelem=b.oid
    LEFT OUTER JOIN pg_collation c ON att.attcollation=c.oid
    LEFT OUTER JOIN pg_namespace nspc ON c.collnamespace=nspc.oid
    WHERE att.attrelid=
    (
        SELECT typrelid 
        FROM pg_type
        INNER JOIN pg_namespace
        ON pg_type.typnamespace = pg_namespace.oid
        WHERE typname=_name
        AND pg_namespace.nspname=_schema
    )
    AND att.attnum > 0
    ORDER by attnum;

    RETURN QUERY
    SELECT * FROM temp_poco;
END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION poco_get_table_function_definition(text, text)
  OWNER TO frapid_db_user;

-- Function: poco_get_tables(text)

-- DROP FUNCTION poco_get_tables(text);

CREATE OR REPLACE FUNCTION poco_get_tables(IN _schema text)
  RETURNS TABLE(table_schema name, table_name name, table_type text, has_duplicate boolean) AS
$BODY$
BEGIN
    CREATE TEMPORARY TABLE _t
    (
        table_schema            name,
        table_name              name,
        table_type              text,
        has_duplicate           boolean DEFAULT(false)
    ) ON COMMIT DROP;

    INSERT INTO _t
    SELECT 
        information_schema.tables.table_schema, 
        information_schema.tables.table_name, 
        information_schema.tables.table_type
    FROM information_schema.tables 
    WHERE (information_schema.tables.table_type='BASE TABLE' OR information_schema.tables.table_type='VIEW')
    AND information_schema.tables.table_schema = _schema
    UNION ALL
    SELECT DISTINCT 
        pg_namespace.nspname::text, 
        pg_proc.proname::text, 
        'FUNCTION' AS table_type
    FROM pg_proc
    INNER JOIN pg_namespace
    ON pg_proc.pronamespace = pg_namespace.oid
    INNER JOIN pg_language 
    ON pg_proc.prolang = pg_language .oid
    INNER JOIN pg_type
    ON pg_proc.prorettype=pg_type.oid
    WHERE ('t' = ANY(pg_proc.proargmodes) OR 'o' = ANY(pg_proc.proargmodes) OR pg_type.typtype = 'c')
    AND lanname NOT IN ('c','internal')
    AND nspname=_schema;


    UPDATE _t
    SET has_duplicate = TRUE
    FROM
    (
        SELECT
            information_schema.tables.table_name,
            COUNT(information_schema.tables.table_name) AS table_count
        FROM information_schema.tables
        GROUP BY information_schema.tables.table_name
        
    ) subquery
    WHERE subquery.table_name = _t.table_name
    AND subquery.table_count > 1;

    

    RETURN QUERY
    SELECT * FROM _t;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION poco_get_tables(text)
  OWNER TO frapid_db_user;

  -- Function: parse_default(text)

-- DROP FUNCTION parse_default(text);

CREATE OR REPLACE FUNCTION parse_default(text)
  RETURNS text AS
$BODY$
DECLARE _sql text;
DECLARE _val text;
BEGIN
    IF($1 LIKE '%::%' AND $1 NOT LIKE 'nextval%') THEN
        _sql := 'SELECT ' || $1;
        EXECUTE _sql INTO _val;
        RETURN _val;
    END IF;

    IF($1 = 'now()') THEN
        RETURN '';
    END IF;

    RETURN $1;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION parse_default(text)
  OWNER TO frapid_db_user;

-- Function: explode_array(anyarray)

-- DROP FUNCTION explode_array(anyarray);

CREATE OR REPLACE FUNCTION explode_array(in_array anyarray)
  RETURNS SETOF anyelement AS
$BODY$
    SELECT ($1)[s] FROM generate_series(1,array_upper($1, 1)) AS s;
$BODY$
  LANGUAGE sql IMMUTABLE
  COST 100
  ROWS 1000;
ALTER FUNCTION explode_array(anyarray)
  OWNER TO frapid_db_user;

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/1.x/1.0/src/01.types-domains-tables-and-constraints/domains.sql --<--<--
DROP DOMAIN IF EXISTS public.money_strict CASCADE;
CREATE DOMAIN public.money_strict
AS DECIMAL(24, 4)
CHECK
(
    VALUE > 0
);


DROP DOMAIN IF EXISTS public.money_strict2 CASCADE;
CREATE DOMAIN public.money_strict2
AS DECIMAL(24, 4)
CHECK
(
    VALUE >= 0
);

DROP DOMAIN IF EXISTS public.integer_strict CASCADE;
CREATE DOMAIN public.integer_strict
AS integer
CHECK
(
    VALUE > 0
);

DROP DOMAIN IF EXISTS public.integer_strict2 CASCADE;
CREATE DOMAIN public.integer_strict2
AS integer
CHECK
(
    VALUE >= 0
);

DROP DOMAIN IF EXISTS public.smallint_strict CASCADE;
CREATE DOMAIN public.smallint_strict
AS smallint
CHECK
(
    VALUE > 0
);

DROP DOMAIN IF EXISTS public.smallint_strict2 CASCADE;
CREATE DOMAIN public.smallint_strict2
AS smallint
CHECK
(
    VALUE >= 0
);

DROP DOMAIN IF EXISTS public.decimal_strict CASCADE;
CREATE DOMAIN public.decimal_strict
AS decimal
CHECK
(
    VALUE > 0
);

DROP DOMAIN IF EXISTS public.decimal_strict2 CASCADE;
CREATE DOMAIN public.decimal_strict2
AS decimal
CHECK
(
    VALUE >= 0
);

DROP DOMAIN IF EXISTS public.color CASCADE;
CREATE DOMAIN public.color
AS text;

DROP DOMAIN IF EXISTS public.image CASCADE;
CREATE DOMAIN public.image
AS text;


DROP DOMAIN IF EXISTS public.html CASCADE;
CREATE DOMAIN public.html
AS text;

DROP DOMAIN IF EXISTS public.password CASCADE;
CREATE DOMAIN public.password
AS text;

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/1.x/1.0/src/01.types-domains-tables-and-constraints/tables-and-constraints.sql --<--<--
DROP SCHEMA IF EXISTS core CASCADE;
CREATE SCHEMA core;

CREATE TABLE core.apps
(
    app_name                                    national character varying(100) PRIMARY KEY,
    name                                        national character varying(100),
    version_number                              national character varying(100),
    publisher                                   national character varying(100),
    published_on                                date,
    icon                                        national character varying(100),
    landing_url                                 text
);

CREATE UNIQUE INDEX apps_app_name_uix
ON core.apps(UPPER(app_name));

CREATE TABLE core.app_dependencies
(
    app_dependency_id                           SERIAL PRIMARY KEY,
    app_name                                    national character varying(100) REFERENCES core.apps,
    depends_on                                  national character varying(100) REFERENCES core.apps
);


CREATE TABLE core.menus
(
    menu_id                                     SERIAL PRIMARY KEY,
    sort                                        integer,
    app_name                                    national character varying(100) NOT NULL REFERENCES core.apps,
    menu_name                                   national character varying(100) NOT NULL,
    url                                         text,
    icon                                        national character varying(100),
    parent_menu_id                              integer REFERENCES core.menus
);

CREATE UNIQUE INDEX menus_app_name_menu_name_uix
ON core.menus(UPPER(app_name), UPPER(menu_name));

CREATE TABLE core.menu_locale
(
    menu_locale_id                              SERIAL PRIMARY KEY,
    menu_id                                     integer NOT NULL REFERENCES core.menus,
    culture                                     national character varying(12) NOT NULL,
    menu_text                                   national character varying(250) NOT NULL
);

CREATE TABLE core.offices
(
    office_id                                   SERIAL PRIMARY KEY,
    office_code                                 national character varying(12) NOT NULL,
    office_name                                 national character varying(150) NOT NULL,
    nick_name                                   national character varying(50),
    registration_date                           date,
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
    logo                                        public.image,
    parent_office_id                            integer NULL REFERENCES core.offices,
    audit_user_id                               integer NULL,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())
);

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/1.x/1.0/src/04.default-values/01.default-values.sql --<--<--
INSERT INTO core.offices(office_code, office_name)
SELECT 'DEF', 'Default';

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/1.x/1.0/src/06.functions-and-logic/core.create_app.sql --<--<--
DROP FUNCTION IF EXISTS core.create_app
(
    _app_name                                   text,
    _name                                       text,
    _version_number                             text,
    _publisher                                  text,
    _published_on                               date,
    _icon                                       text,
    _landing_url                                text,
    _dependencies                               text[]
);

CREATE FUNCTION core.create_app
(
    _app_name                                   text,
    _name                                       text,
    _version_number                             text,
    _publisher                                  text,
    _published_on                               date,
    _icon                                       text,
    _landing_url                                text,
    _dependencies                               text[]
)
RETURNS void
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM core.apps
        WHERE LOWER(core.apps.app_name) = LOWER(_app_name)
    ) THEN
        UPDATE core.apps
        SET
            name = _name,
            version_number = _version_number,
            publisher = _publisher,
            published_on = _published_on,
            icon = _icon,
            landing_url = _landing_url
        WHERE
            app_name = _app_name;
    ELSE
        INSERT INTO core.apps(app_name, name, version_number, publisher, published_on, icon, landing_url)
        SELECT _app_name, _name, _version_number, _publisher, _published_on, _icon, _landing_url;
    END IF;

    DELETE FROM core.app_dependencies
    WHERE app_name = _app_name;

    INSERT INTO core.app_dependencies(app_name, depends_on)
    SELECT _app_name, UNNEST(_dependencies);
END
$$
LANGUAGE plpgsql;


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/1.x/1.0/src/06.functions-and-logic/core.create_menu.sql --<--<--
DROP FUNCTION IF EXISTS core.create_menu
(
    _sort                                       integer,
    _app_name                                   text,
    _menu_name                                  text,
    _url                                        text,
    _icon                                       text,
    _parent_menu_id                             integer    
);

DROP FUNCTION IF EXISTS core.create_menu
(
    _sort                                       integer,
    _app_name                                   text,
    _menu_name                                  text,
    _url                                        text,
    _icon                                       text,
    _parent_menu_name                           text
);

CREATE FUNCTION core.create_menu
(
    _sort                                       integer,
    _app_name                                   text,
    _menu_name                                  text,
    _url                                        text,
    _icon                                       text,
    _parent_menu_id                             integer
    
)
RETURNS integer
AS
$$
    DECLARE _menu_id                            integer;
BEGIN
    IF EXISTS
    (
       SELECT 1
       FROM core.menus
       WHERE LOWER(app_name) = LOWER(_app_name)
       AND LOWER(menu_name) = LOWER(_menu_name)
    ) THEN
        UPDATE core.menus
        SET
            sort = _sort,
            url = _url,
            icon = _icon,
            parent_menu_id = _parent_menu_id
       WHERE LOWER(app_name) = LOWER(_app_name)
       AND LOWER(menu_name) = LOWER(_menu_name)
       RETURNING menu_id INTO _menu_id;        
    ELSE
        INSERT INTO core.menus(sort, app_name, menu_name, url, icon, parent_menu_id)
        SELECT _sort, _app_name, _menu_name, _url, _icon, _parent_menu_id
        RETURNING menu_id INTO _menu_id;        
    END IF;

    RETURN _menu_id;
END
$$
LANGUAGE plpgsql;


CREATE FUNCTION core.create_menu
(
    _sort                                       integer,
    _app_name                                   text,
    _menu_name                                  text,
    _url                                        text,
    _icon                                       text,
    _parent_menu_name                           text    
)
RETURNS integer
AS
$$
    DECLARE _parent_menu_id                     integer;
BEGIN
    SELECT menu_id INTO _parent_menu_id
    FROM core.menus
    WHERE LOWER(menu_name) = LOWER(_parent_menu_name)
    AND LOWER(app_name) = LOWER(_app_name);

    RETURN core.create_menu(_sort, _app_name, _menu_name, _url, _icon, _parent_menu_id);
END
$$
LANGUAGE plpgsql;


DROP FUNCTION IF EXISTS core.create_menu
(
    _app_name                                   text,
    _menu_name                                  text,
    _url                                        text,
    _icon                                       text,
    _parent_menu_name                           text    
);

CREATE FUNCTION core.create_menu
(
    _app_name                                   text,
    _menu_name                                  text,
    _url                                        text,
    _icon                                       text,
    _parent_menu_name                           text    
)
RETURNS integer
AS
$$
BEGIN
    RETURN core.create_menu(0, _app_name, _menu_name, _url, _icon, _parent_menu_name);
END
$$
LANGUAGE plpgsql;

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/1.x/1.0/src/06.functions-and-logic/core.get_office_id_by_office_name.sql --<--<--
DROP FUNCTION IF EXISTS core.get_office_id_by_office_name(_office_name text);

CREATE FUNCTION core.get_office_id_by_office_name(_office_name text)
RETURNS integer
AS
$$
BEGIN
    RETURN core.offices.office_id
    FROM core.offices
    WHERE core.offices.office_name = _office_name;
END
$$
LANGUAGE plpgsql;

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Core/db/1.x/1.0/src/99.ownership.sql --<--<--
DO
$$
    DECLARE this record;
BEGIN
    IF(CURRENT_USER = 'frapid_db_user') THEN
        RETURN;
    END IF;

    FOR this IN 
    SELECT * FROM pg_tables 
    WHERE NOT schemaname = ANY(ARRAY['pg_catalog', 'information_schema'])
    AND tableowner <> 'frapid_db_user'
    LOOP
        EXECUTE 'ALTER TABLE '|| this.schemaname || '.' || this.tablename ||' OWNER TO frapid_db_user;';
    END LOOP;
END
$$
LANGUAGE plpgsql;

DO
$$
    DECLARE this record;
BEGIN
    IF(CURRENT_USER = 'frapid_db_user') THEN
        RETURN;
    END IF;

    FOR this IN 
    SELECT 'ALTER '
        || CASE WHEN p.proisagg THEN 'AGGREGATE ' ELSE 'FUNCTION ' END
        || quote_ident(n.nspname) || '.' || quote_ident(p.proname) || '(' 
        || pg_catalog.pg_get_function_identity_arguments(p.oid) || ') OWNER TO frapid_db_user;' AS sql
    FROM   pg_catalog.pg_proc p
    JOIN   pg_catalog.pg_namespace n ON n.oid = p.pronamespace
    WHERE  NOT n.nspname = ANY(ARRAY['pg_catalog', 'information_schema'])
    LOOP        
        EXECUTE this.sql;
    END LOOP;
END
$$
LANGUAGE plpgsql;


DO
$$
    DECLARE this record;
BEGIN
    IF(CURRENT_USER = 'frapid_db_user') THEN
        RETURN;
    END IF;

    FOR this IN 
    SELECT * FROM pg_views
    WHERE NOT schemaname = ANY(ARRAY['pg_catalog', 'information_schema'])
    AND viewowner <> 'frapid_db_user'
    LOOP
        EXECUTE 'ALTER VIEW '|| this.schemaname || '.' || this.viewname ||' OWNER TO frapid_db_user;';
    END LOOP;
END
$$
LANGUAGE plpgsql;


DO
$$
    DECLARE this record;
BEGIN
    IF(CURRENT_USER = 'frapid_db_user') THEN
        RETURN;
    END IF;

    FOR this IN 
    SELECT 'ALTER SCHEMA ' || nspname || ' OWNER TO frapid_db_user;' AS sql FROM pg_namespace
    WHERE nspname NOT LIKE 'pg_%'
    AND nspname <> 'information_schema'
    LOOP
        EXECUTE this.sql;
    END LOOP;
END
$$
LANGUAGE plpgsql;



DO
$$
    DECLARE this record;
BEGIN
    IF(CURRENT_USER = 'frapid_db_user') THEN
        RETURN;
    END IF;

    FOR this IN 
    SELECT      'ALTER TYPE ' || n.nspname || '.' || t.typname || ' OWNER TO frapid_db_user;' AS sql
    FROM        pg_type t 
    LEFT JOIN   pg_catalog.pg_namespace n ON n.oid = t.typnamespace 
    WHERE       (t.typrelid = 0 OR (SELECT c.relkind = 'c' FROM pg_catalog.pg_class c WHERE c.oid = t.typrelid)) 
    AND         NOT EXISTS(SELECT 1 FROM pg_catalog.pg_type el WHERE el.oid = t.typelem AND el.typarray = t.oid)
    AND         typtype NOT IN ('b')
    AND         n.nspname NOT IN ('pg_catalog', 'information_schema')
    LOOP
        EXECUTE this.sql;
    END LOOP;
END
$$
LANGUAGE plpgsql;


DO
$$
    DECLARE this record;
BEGIN
    IF(CURRENT_USER = 'report_user') THEN
        RETURN;
    END IF;

    FOR this IN 
    SELECT * FROM pg_tables 
    WHERE NOT schemaname = ANY(ARRAY['pg_catalog', 'information_schema'])
    AND tableowner <> 'report_user'
    LOOP
        EXECUTE 'GRANT SELECT ON TABLE '|| this.schemaname || '.' || this.tablename ||' TO report_user;';
    END LOOP;
END
$$
LANGUAGE plpgsql;

DO
$$
    DECLARE this record;
BEGIN
    IF(CURRENT_USER = 'report_user') THEN
        RETURN;
    END IF;

    FOR this IN 
    SELECT 'GRANT EXECUTE ON '
        || CASE WHEN p.proisagg THEN 'AGGREGATE ' ELSE 'FUNCTION ' END
        || quote_ident(n.nspname) || '.' || quote_ident(p.proname) || '(' 
        || pg_catalog.pg_get_function_identity_arguments(p.oid) || ') TO report_user;' AS sql
    FROM   pg_catalog.pg_proc p
    JOIN   pg_catalog.pg_namespace n ON n.oid = p.pronamespace
    WHERE  NOT n.nspname = ANY(ARRAY['pg_catalog', 'information_schema'])
    LOOP        
        EXECUTE this.sql;
    END LOOP;
END
$$
LANGUAGE plpgsql;


DO
$$
    DECLARE this record;
BEGIN
    IF(CURRENT_USER = 'report_user') THEN
        RETURN;
    END IF;

    FOR this IN 
    SELECT * FROM pg_views
    WHERE NOT schemaname = ANY(ARRAY['pg_catalog', 'information_schema'])
    AND viewowner <> 'report_user'
    LOOP
        EXECUTE 'GRANT SELECT ON '|| this.schemaname || '.' || this.viewname ||' TO report_user;';
    END LOOP;
END
$$
LANGUAGE plpgsql;


DO
$$
    DECLARE this record;
BEGIN
    IF(CURRENT_USER = 'report_user') THEN
        RETURN;
    END IF;

    FOR this IN 
    SELECT 'GRANT USAGE ON SCHEMA ' || nspname || ' TO report_user;' AS sql FROM pg_namespace
    WHERE nspname NOT LIKE 'pg_%'
    AND nspname <> 'information_schema'
    LOOP
        EXECUTE this.sql;
    END LOOP;
END
$$
LANGUAGE plpgsql;

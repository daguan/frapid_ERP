-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/db/core/1.x/1.0/src/00.db core/extensions.sql --<--<--
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/db/core/1.x/1.0/src/00.db core/postgresql-roles.sql --<--<--
DO
$$
BEGIN
    IF NOT EXISTS (SELECT * FROM pg_catalog.pg_roles WHERE rolname = 'frapid_db_user') THEN
        CREATE ROLE frapid_db_user WITH LOGIN PASSWORD 'change-on-deployment';
    END IF;

    COMMENT ON ROLE mix_erp IS 'The default user for frapid databases.';

    EXECUTE 'ALTER DATABASE ' || current_database() || ' OWNER TO frapid_db_user;';
END
$$
LANGUAGE plpgsql;

DO
$$
BEGIN
    IF NOT EXISTS (SELECT * FROM pg_catalog.pg_roles WHERE rolname = 'frapid_report_user') THEN
        CREATE ROLE frapid_report_user WITH LOGIN PASSWORD 'change-on-deployment';
    END IF;

    COMMENT ON ROLE mix_erp IS 'This user account is used by the Reporting Engine to run ad-hoc queries. It is strictly advised for this user to only have a read-only access to the database.';
END
$$
LANGUAGE plpgsql;

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/db/core/1.x/1.0/src/01.types-domains-tables-and-constraints/domains.sql --<--<--
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

DROP DOMAIN IF EXISTS public.photo CASCADE;
CREATE DOMAIN public.photo
AS text;


DROP DOMAIN IF EXISTS public.html CASCADE;
CREATE DOMAIN public.html
AS text;

DROP DOMAIN IF EXISTS public.password CASCADE;
CREATE DOMAIN public.password
AS text;

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/db/core/1.x/1.0/src/01.types-domains-tables-and-constraints/tables-and-constraints.sql --<--<--
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
    logo                                        public.photo,
    parent_office_id                            integer NULL REFERENCES core.offices,
    audit_user_id                               integer NULL,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())
);

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/db/core/1.x/1.0/src/04.default-values/01.default-values.sql --<--<--
INSERT INTO core.offices(office_code, office_name)
SELECT 'DEF', 'Default';

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/db/core/1.x/1.0/src/06.functions-and-logic/core.create_app.sql --<--<--
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


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/db/core/1.x/1.0/src/06.functions-and-logic/core.create_menu.sql --<--<--
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

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/db/core/1.x/1.0/src/06.functions-and-logic/core.get_office_id_by_office_name.sql --<--<--
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

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/db/core/1.x/1.0/src/99.ownership.sql --<--<--
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

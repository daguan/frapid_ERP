DROP SCHEMA IF EXISTS website CASCADE; --WEB BUILDER
CREATE SCHEMA website;

CREATE TABLE website.contents
(
    content_id                                  SERIAL NOT NULL PRIMARY KEY,
    title                                       national character varying(100) NOT NULL,
    alias                                       national character varying(50) NOT NULL UNIQUE,
    author_id                                   integer,
    publish_on                                  TIMESTAMP WITH TIME ZONE NOT NULL,
    contents                                    text NOT NULL,
    is_draft                                    boolean NOT NULL DEFAULT(true),
    seo_keywords                                national character varying(50) NOT NULL DEFAULT(''),
    seo_description                             national character varying(100) NOT NULL DEFAULT(''),
    is_homepage                                 boolean NOT NULL DEFAULT(false),
    audit_user_id                               integer,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())    
);

CREATE TABLE website.menus
(
    menu_id                                     SERIAL PRIMARY KEY,
    menu_name                                   national character varying(100),
    description                                 text,
    audit_user_id                               integer,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())
);

CREATE TABLE website.menu_items
(
    menu_item_id                                SERIAL PRIMARY KEY,
    menu_id                                     integer REFERENCES website.menus,
    sort                                        integer NOT NULL DEFAULT(0),
    title                                       national character varying(100) NOT NULL,
    url                                         text,
    content_id                                  integer REFERENCES website.contents,    
    audit_user_id                               integer,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())    
);


CREATE TABLE website.contacts
(
    contact_id                                  SERIAL PRIMARY KEY,
    title                                       national character varying(500) NOT NULL,
    name                                        national character varying(500) NOT NULL,
    position                                    national character varying(500) NOT NULL,
    address                                     national character varying(500),
    city                                        national character varying(500),
    state                                       national character varying(500),
    country                                     national character varying(100),
    postal_code                                 national character varying(500),
    telephone                                   national character varying(500),
    details                                     text,
    email                                       national character varying(500),
    display_email                               boolean NOT NULL DEFAULT(false),
    display_contact_form                        boolean NOT NULL DEFAULT(true),
    sort                                        integer NOT NULL DEFAULT(0),
    status                                      boolean NOT NULL DEFAULT(true),
    audit_user_id                               integer,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())    
);

DROP VIEW IF EXISTS website.menu_item_view;

CREATE VIEW website.menu_item_view
AS
SELECT
    website.menus.menu_id,
    website.menus.menu_name,
    website.menus.description,
    website.menu_items.menu_item_id,
    website.menu_items.sort,
    website.menu_items.title,
    website.menu_items.url,
    website.menu_items.content_id,
    website.contents.alias AS content_alias
FROM website.menu_items
INNER JOIN website.menus
ON website.menus.menu_id = website.menu_items.menu_id
LEFT JOIN website.contents
ON website.contents.content_id = website.menu_items.content_id;

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

SELECT * FROM config.create_app('Frapid.WebsiteBuilder', 'Website', '1.0', 'MixERP Inc.', 'December 1, 2015', 'world blue', '/dashboard/wb/contents', null);

SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Tasks', '', '', '');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Add New Content', '/dashboard/wb/contents/new', '', 'Tasks');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'View Contents', '/dashboard/wb/contents', '', 'Tasks');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Layout Manager', '', '', '');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Edit Master Layout', '/dashboard/wb/layouts/master', '', 'Layout Manager');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Edit Header', '/dashboard/wb/layouts/header', '', 'Layout Manager');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Edit Footer', '/dashboard/wb/layouts/footer', '', 'Layout Manager');

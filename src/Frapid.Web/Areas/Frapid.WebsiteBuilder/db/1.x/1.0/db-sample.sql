-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/0.db.sql --<--<--
DROP SCHEMA IF EXISTS website CASCADE; --WEB BUILDER
CREATE SCHEMA website;

CREATE TABLE website.categories
(
    category_id                                 SERIAL NOT NULL PRIMARY KEY,
    category_name                               national character varying(100) NOT NULL,
    alias                                       national character varying(50) NOT NULL UNIQUE,
    seo_keywords                                national character varying(50),
    seo_description                             national character varying(100),
    audit_user_id                               integer,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())    
);

CREATE TABLE website.contents
(
    content_id                                  SERIAL NOT NULL PRIMARY KEY,
    category_id                                 integer NOT NULL REFERENCES website.categories,
    title                                       national character varying(100) NOT NULL,
    alias                                       national character varying(50) NOT NULL UNIQUE,
    author_id                                   integer,
    publish_on                                  TIMESTAMP WITH TIME ZONE NOT NULL,
    markdown                                    text,
    contents                                    text NOT NULL,
    tags                                        text,
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

CREATE UNIQUE INDEX menus_menu_name_uix
ON website.menus(UPPER(menu_name));

CREATE TABLE website.menu_items
(
    menu_item_id                                SERIAL PRIMARY KEY,
    menu_id                                     integer REFERENCES website.menus,
    sort                                        integer NOT NULL DEFAULT(0),
    title                                       national character varying(100) NOT NULL,
    url                                         national character varying(500),
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
    position                                    national character varying(500),
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

SELECT * FROM config.create_app('Frapid.WebsiteBuilder', 'Website', '1.0', 'MixERP Inc.', 'December 1, 2015', 'world blue', '/dashboard/website/contents', null);

SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Tasks', '', '', '');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Manage Categories', '/dashboard/website/categories', '', 'Tasks');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Add New Content', '/dashboard/website/contents/new', '', 'Tasks');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'View Contents', '/dashboard/website/contents', '', 'Tasks');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Menus', '/dashboard/website/menus', '', 'Tasks');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Contacts', '/dashboard/website/contacts', '', 'Tasks');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Layout Manager', '', '', '');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Edit Master Layout', '/dashboard/website/layouts/master', '', 'Layout Manager');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Edit Header', '/dashboard/website/layouts/header', '', 'Layout Manager');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Edit Footer', '/dashboard/website/layouts/footer', '', 'Layout Manager');

CREATE VIEW website.tag_view
AS
SELECT DISTINCT unnest(regexp_split_to_array(tags, ',')) AS tag FROM website.contents;

CREATE VIEW website.published_content_view
AS
SELECT
    website.contents.content_id,
    website.contents.category_id,
    website.categories.category_name,
    website.categories.alias AS category_alias,
    website.contents.title,
    website.contents.alias,
    website.contents.author_id,
    website.contents.markdown,
    website.contents.contents,
    website.contents.tags,
    website.contents.seo_keywords,
    website.contents.seo_description,
    website.contents.is_homepage
FROM website.contents
INNER JOIN website.categories
ON website.categories.category_id = website.contents.category_id
WHERE NOT is_draft
AND publish_on <= NOW();

DROP FUNCTION IF EXISTS website.get_category_id_by_category_name(_category_name text);

CREATE FUNCTION website.get_category_id_by_category_name(_category_name text)
RETURNS integer
AS
$$
BEGIN
    RETURN category_id
    FROM website.categories
    WHERE category_name = _category_name;
END
$$
LANGUAGE plpgsql;

DROP FUNCTION IF EXISTS website.get_category_id_by_category_alias(_alias text);

CREATE FUNCTION website.get_category_id_by_category_alias(_alias text)
RETURNS integer
AS
$$
BEGIN
    RETURN category_id
    FROM website.categories
    WHERE alias = _alias;
END
$$
LANGUAGE plpgsql;


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/1.sample.sql.sample --<--<--
DELETE FROM website.contents;
DELETE FROM website.menu_items;
DELETE FROM website.menus;

INSERT INTO website.menus(menu_name)
SELECT 'Default';

INSERT INTO website.menu_items(menu_id, title, url)
SELECT 1, 'Home', '/' UNION ALL
SELECT 1, 'Sign Up', '/account/sign-up' UNION ALL
SELECT 1, 'Admin Area', '/dashboard' UNION ALL
SELECT 1, 'Contact Us', '/contact-us';

INSERT INTO website.categories(category_name, alias, seo_keywords, seo_description)
SELECT 'Default', 'default', '', '';

INSERT INTO website.categories(category_name, alias, seo_keywords, seo_description)
SELECT 'Legal', 'legal', '', '';

DELETE FROM website.contents;

INSERT INTO website.contents(title, tags, alias, category_id, publish_on, is_draft, seo_keywords, seo_description, is_homepage, contents)
SELECT 'Welcome to Frapid', 'frapid,cms', 'welcome-to-frapid', website.get_category_id_by_category_alias('default'), NOW(), false, 'frapid, cms, crm, erp, hrm', 'Homepage of Frapid Framework', true, '<div class="ui basic inverted attached segment" id="banner">
    <div class="ui caption container">
        <div class="ui huge inverted header">
            Frapid Framework
        </div>
        <p>
            Howdy, your Frapid instance is now up and running.
            <br/> Login to your admin area now and start building your website.
            <p>
            <div class="ui hidden divider"></div>
            <div class="ui small inverted buttons">
                <a class="ui inverted button" href="/dashboard">Admin Area</a>
                <a class="ui inverted button" href="http://docs.frapid.com/" target="_blank">Read Documentation</a>
            </div>
    </div>
</div>
<div class="ui attached vertically padded segment">
    <div class="ui story container">
        <div class="ui huge header">
            <span>Learn More</span> About Frapid
        </div>
        <div class="ui divider"></div>
        <p>
            Frapid is a multi-tenant application development framework released under MIT License.
            <br/> Learn more about Frapid and quickly add contents in your website.
        </p>
    </div>



    <div class="ui three column page stackable doubling padded grid">
        <div class="column">
            <div class="ui ad box grid">
                <div class="ad icon four wide column">
                    <i class="circular level up inverted red icon"></i>
                </div>
                <div class="ad details twelve wide column">
                    <div class="ui red header">Edit Header</div>
                    <p>
                        Read the <a href="http://docs.frapid.com/" target="_blank">documentation</a> on editing your site header.
                    </p>
                </div>
            </div>
        </div>
        <div class="column">
            <div class="ui ad box grid">
                <div class="ad icon four wide column">
                    <i class="circular level down inverted violet icon"></i>
                </div>
                <div class="ad details twelve wide column">
                    <div class="ui violet header">Edit Footer</div>
                    <p>
                        Edit the contents and links in your
                        <a href="http://docs.frapid.com/site/footer" target="_blank">site footer</a>.
                    </p>
                </div>
            </div>
        </div>
        <div class="column">
            <div class="ui ad box grid">
                <div class="ad icon four wide column">
                    <i class="circular checkmark inverted teal icon"></i>
                </div>
                <div class="ad details twelve wide column">
                    <div class="ui teal header">Manage Pages</div>
                    <p>
                        Quickly add site pages, edit, and
                        <a href="http://docs.frapid.com/site/contents" target="_blank">manage them</a>.
                    </p>
                </div>
            </div>
        </div>
        <div class="column">
            <div class="ui ad box grid">
                <div class="ad icon four wide column">
                    <i class="circular linkify inverted green icon"></i>
                </div>
                <div class="ad details twelve wide column">
                    <div class="ui green header">Menu Builder</div>
                    <p>
                        Learn how you can add & edit
                        <a href="http://docs.frapid.com/site/menus" target="_blank">site menus</a>.
                    </p>
                </div>
            </div>
        </div>
        <div class="column">
            <div class="ui ad box grid">
                <div class="ad icon four wide column">
                    <i class="circular code inverted blue icon"></i>
                </div>
                <div class="ad details twelve wide column">
                    <div class="ui blue header">Develop Frapid Apps</div>
                    <p>
                        Develop your own open source or commercial
                        <a href="http://docs.frapid.com/site/develop-frapid-app" target="_blank">frapid apps</a>.
                    </p>
                </div>
            </div>
        </div>
        <div class="column">
            <div class="ui ad box grid">
                <div class="ad icon four wide column">
                    <i class="circular line inverted yellow chart icon"></i>
                </div>
                <div class="ad details twelve wide column">
                    <div class="ui yellow header">Improve Frapid</div>
                    <p>
                        Improve Frapid by
                        <a href="https://github.com/frapid/frapid/issues/new" target="_blank">documenting</a> and fixing bugs.
                    </p>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="ui attached brown vertically padded segment" id="contact">
    <div class="ui story container">
        <div class="ui huge red header">
            Keep in Touch
        </div>
        <div class="ui hidden divider"></div>
        <p>
            Want us to build an application or website for you?
            <br/> Why not contact us today? We will be happy to listen to you!
        </p>
        <div class="ui hidden divider"></div>
        <p>
            <a class="ui orange button" href="/contact-us">Contact Us</a>
        </p>
    </div>
</div>';

    
INSERT INTO website.contents(title, tags, alias, category_id, publish_on, is_draft, seo_keywords, seo_description, is_homepage, markdown, contents)
SELECT 'Terms of Use', '', 'terms-of-use', website.get_category_id_by_category_alias('legal'), NOW(), false, 'terms of use', 'Terms of Use', false, '# Terms of Use

This document is empty.', '<h1 id="terms-of-use">Terms of Use</h1>
<p>This document is empty.</p>';

INSERT INTO website.contents(title, tags, alias, category_id, publish_on, is_draft, seo_keywords, seo_description, is_homepage, markdown, contents)
SELECT 'Privacy Policy', '', 'privacy-policy', website.get_category_id_by_category_alias('legal'), NOW(), false, 'privacy policy', 'Privacy Policy', false, '# Privacy Policy

This document is empty.', '<h1 id="privacy-policy">Privacy Policy</h1>
<p>This document is empty.</p>';


INSERT INTO website.contacts(title, name, "position", address, city, state, country, postal_code, telephone, details, email, display_email, display_contact_form, status)
SELECT 'Corporate Headquarters', 'Your Office Name', '', 'Address', 'City', 'State', 'Country', '000', '000 000 000', '', 'info@frapid.com', false, true, true UNION ALL
SELECT 'United States', 'John Doe', 'Client Partner', '', '', '', '', '', '', 'Texas', 'info@frapid.com', false, true, true;

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/99.ownership.sql --<--<--
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

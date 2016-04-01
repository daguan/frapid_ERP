-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/01.types-domains-tables-and-constraints/tables-and-constraints.sql --<--<--
DROP SCHEMA IF EXISTS website CASCADE; --WEB BUILDER
CREATE SCHEMA website;

CREATE TABLE website.configurations
(
    configuration_id                                SERIAL PRIMARY KEY,
    domain_name                                     national character varying(500) NOT NULL,
    website_name                                    national character varying(500) NOT NULL,
	description										text,
	blog_title                                      national character varying(500),
	blog_description							    text,	
	is_default                                      boolean NOT NULL DEFAULT(true),
    audit_user_id                                   integer REFERENCES account.users,
    audit_ts                                        TIMESTAMP WITH TIME ZONE NULL DEFAULT(NOW())
);

CREATE UNIQUE INDEX configuration_domain_name_uix
ON website.configurations(LOWER(domain_name));

CREATE TABLE website.email_subscriptions
(
    email_subscription_id                       uuid PRIMARY KEY DEFAULT(gen_random_uuid()),
	first_name									national character varying(100),
	last_name									national character varying(100),
    email                                       national character varying(100) NOT NULL UNIQUE,
    browser                                     text,
    ip_address                                  national character varying(50),
	confirmed									boolean DEFAULT(false),
    confirmed_on                               	TIMESTAMP WITH TIME ZONE,
    unsubscribed                                boolean DEFAULT(false),
    subscribed_on                               TIMESTAMP WITH TIME ZONE DEFAULT(NOW()),    
    unsubscribed_on                             TIMESTAMP WITH TIME ZONE
);

CREATE TABLE website.categories
(
    category_id                                 SERIAL NOT NULL PRIMARY KEY,
    category_name                               national character varying(100) NOT NULL,
    alias                                       national character varying(50) NOT NULL UNIQUE,
    seo_description                             national character varying(100),
	is_blog										boolean NOT NULL DEFAULT(false),
    audit_user_id                               integer REFERENCES account.users,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())    
);

CREATE TABLE website.contents
(
    content_id                                  SERIAL NOT NULL PRIMARY KEY,
    category_id                                 integer NOT NULL REFERENCES website.categories,
    title                                       national character varying(500) NOT NULL,
    alias                                       national character varying(250) NOT NULL UNIQUE,
    author_id                                   integer REFERENCES account.users,
    publish_on                                  TIMESTAMP WITH TIME ZONE NOT NULL,
	created_on									TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT(NOW()),
    last_editor_id                              integer REFERENCES account.users,
	last_edited_on							    TIMESTAMP WITH TIME ZONE,
    markdown                                    text,
    contents                                    text NOT NULL,
    tags                                        text,
	hits										bigint,
    is_draft                                    boolean NOT NULL DEFAULT(true),
    seo_description                             national character varying(1000) NOT NULL DEFAULT(''),
    is_homepage                                 boolean NOT NULL DEFAULT(false),
    audit_user_id                               integer REFERENCES account.users,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())    
);

CREATE TABLE website.menus
(
    menu_id                                     SERIAL PRIMARY KEY,
    menu_name                                   national character varying(100),
    description                                 text,
    audit_user_id                               integer REFERENCES account.users,
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
    target                                      national character varying(20),
    content_id                                  integer REFERENCES website.contents,    
    audit_user_id                               integer REFERENCES account.users,
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
    recipients                                  national character varying(1000),
    display_email                               boolean NOT NULL DEFAULT(false),
    display_contact_form                        boolean NOT NULL DEFAULT(true),
    sort                                        integer NOT NULL DEFAULT(0),
    status                                      boolean NOT NULL DEFAULT(true),
    audit_user_id                               integer REFERENCES account.users,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())    
);

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/02.functions-and-logic/website.add_email_subscription.sql --<--<--
DROP FUNCTION IF EXISTS website.add_email_subscription
(
    _email                                  text
);


CREATE FUNCTION website.add_email_subscription
(
    _email                                  text
)
RETURNS boolean
AS
$$
BEGIN
    IF NOT EXISTS
    (
        SELECT * FROM website.email_subscriptions
        WHERE email = _email
    ) THEN
        INSERT INTO website.email_subscriptions(email)
        SELECT _email;
        
        RETURN true;
    END IF;

    RETURN false;
END
$$
LANGUAGE plpgsql;

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/02.functions-and-logic/website.get_category_id_by_category_name.sql --<--<--
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

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/02.functions-and-logic/website.remove_email_subscription.sql --<--<--
DROP FUNCTION IF EXISTS website.remove_email_subscription
(
    _email                                  text
);

CREATE FUNCTION website.remove_email_subscription
(
    _email                                  text
)
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT * FROM website.email_subscriptions
        WHERE email = _email
        AND NOT unsubscribed
    ) THEN
        UPDATE website.email_subscriptions
        SET
            unsubscribed = true,
            unsubscribed_on = NOW()
        WHERE email = _email;

        RETURN true;
    END IF;

    RETURN false;
END
$$
LANGUAGE plpgsql;

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/03.menus/menus.sql --<--<--
SELECT * FROM core.create_app('Frapid.WebsiteBuilder', 'Website', '1.0', 'MixERP Inc.', 'December 1, 2015', 'world blue', '/dashboard/website/contents', null);

SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Tasks', '', 'tasks icon', '');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Configuration', '/dashboard/website/configuration', 'configure icon', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Manage Categories', '/dashboard/website/categories', 'sitemap icon', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Add New Content', '/dashboard/website/contents/new', 'file', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'View Contents', '/dashboard/website/contents', 'desktop', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Menus', '/dashboard/website/menus', 'star', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Contacts', '/dashboard/website/contacts', 'phone', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Subscriptions', '/dashboard/website/subscriptions', 'newspaper', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Layout Manager', '/dashboard/website/layouts', 'grid layout', '');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Email Templates', '', 'mail', '');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Subscription Added', '/dashboard/website/subscription/welcome', 'plus circle', 'Email Templates');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Subscription Removed', '/dashboard/website/subscription/removed', 'minus circle', 'Email Templates');


SELECT * FROM auth.create_app_menu_policy
(
    'Content Editor', 
    core.get_office_id_by_office_name('Default'), 
    'Frapid.WebsiteBuilder',
    '{Tasks, Manage Categories, Add New Content, View Contents}'::text[]
);

SELECT * FROM auth.create_app_menu_policy
(
    'User', 
    core.get_office_id_by_office_name('Default'), 
    'Frapid.WebsiteBuilder',
    '{
        Tasks, Manage Categories, Add New Content, View Contents, 
        Menus, Contacts, Subscriptions, 
        Layout Manager, Edit Master Layout (Homepage), Edit Master Layout, Edit Header, Edit Footer, 404 Not Found Document
    }'::text[]
);


SELECT * FROM auth.create_app_menu_policy
(
    'Admin', 
    core.get_office_id_by_office_name('Default'), 
    'Frapid.WebsiteBuilder',
    '{*}'::text[]
);


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/05.triggers/website.email_subscription_confirmation_trigger.sql --<--<--
DROP FUNCTION IF EXISTS website.email_subscription_confirmation_trigger() CASCADE;

CREATE FUNCTION website.email_subscription_confirmation_trigger()
RETURNS TRIGGER
AS
$$
BEGIN
    IF(NEW.confirmed) THEN
        NEW.confirmed_on = NOW();
    END IF;
    
    RETURN NEW;
END
$$
LANGUAGE plpgsql;

CREATE TRIGGER email_subscription_confirmation_trigger 
BEFORE UPDATE ON website.email_subscriptions 
FOR EACH ROW
EXECUTE PROCEDURE website.email_subscription_confirmation_trigger();

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/05.views/website.email_subscription_insert_view.sql --<--<--
DROP VIEW IF EXISTS website.email_subscription_insert_view;

CREATE VIEW website.email_subscription_insert_view
AS
SELECT * FROM website.email_subscriptions
WHERE 1 = 0;


SELECT * FROM website.email_subscription_insert_view;

CREATE RULE log_subscriptions AS 
ON INSERT TO website.email_subscription_insert_view
DO INSTEAD 
INSERT INTO website.email_subscriptions
(
    email, 
    browser, 
    ip_address, 
    unsubscribed, 
    subscribed_on, 
    unsubscribed_on, 
    first_name, 
    last_name, 
    confirmed
)
SELECT
    NEW.email, 
    NEW.browser, 
    NEW.ip_address, 
    NEW.unsubscribed, 
    COALESCE(NEW.subscribed_on, NOW()), 
    NEW.unsubscribed_on, 
    NEW.first_name, 
    NEW.last_name, 
    NEW.confirmed
WHERE NOT EXISTS
(
    SELECT 1 
    FROM website.email_subscriptions
    WHERE email = NEW.email
);



-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/05.views/website.menu_item_view.sql --<--<--
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
    website.menu_items.target,
    website.menu_items.content_id,
    website.contents.alias AS content_alias
FROM website.menu_items
INNER JOIN website.menus
ON website.menus.menu_id = website.menu_items.menu_id
LEFT JOIN website.contents
ON website.contents.content_id = website.menu_items.content_id;

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/05.views/website.published_content_view.sql --<--<--
DROP VIEW IF EXISTS website.published_content_view;

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
    account.users.name AS author_name,
    website.contents.markdown,
    website.contents.publish_on,
    CASE WHEN website.contents.last_edited_on IS NULL THEN website.contents.publish_on ELSE website.contents.last_edited_on END AS last_edited_on,
    website.contents.contents,
    website.contents.tags,
    website.contents.seo_description,
    website.contents.is_homepage,
    website.categories.is_blog
FROM website.contents
INNER JOIN website.categories
ON website.categories.category_id = website.contents.category_id
LEFT JOIN account.users
ON website.contents.author_id = account.users.user_id
WHERE NOT is_draft
AND publish_on <= NOW();

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/05.views/website.tag_view.sql --<--<--
DROP VIEW IF EXISTS website.tag_view;

CREATE VIEW website.tag_view
AS
WITH tags
AS
(
SELECT DISTINCT unnest(regexp_split_to_array(tags, ',')) AS tag FROM website.contents
)
SELECT
    ROW_NUMBER() OVER (ORDER BY tag) AS tag_id,
    tag
FROM tags;


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/05.views/website.yesterdays_email_subscriptions.sql --<--<--
DROP VIEW IF EXISTS website.yesterdays_email_subscriptions;

CREATE VIEW website.yesterdays_email_subscriptions
AS
SELECT
    email,
    first_name,
    last_name,
    'subscribed' AS subscription_type
FROM website.email_subscriptions
WHERE subscribed_on::date = 'yesterday'::date
AND NOT confirmed_on::date = 'yesterday'::date
UNION ALL
SELECT
    email,
    first_name,
    last_name,
    'unsubscribed'
FROM website.email_subscriptions
WHERE unsubscribed_on::date = 'yesterday'::date
UNION ALL
SELECT
    email,
    first_name,
    last_name,
    'confirmed'
FROM website.email_subscriptions
WHERE confirmed_on::date = 'yesterday'::date;

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

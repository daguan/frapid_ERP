-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/db.sql --<--<--
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


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/db/1.x/1.0/src/sample.sql.sample --<--<--
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

INSERT INTO website.contents(title, alias, publish_on, is_draft, seo_keywords, seo_description, is_homepage, contents)
SELECT 'Welcome to Frapid', 'welcome-to-frapid', NOW(), false, 'frapid, cms, crm, erp, hrm', 'Homepage of Frapid Framework', true, '<section id="cta2">
    <div class="container">
        <div class="text-center">
            <h2 style="visibility: visible; animation-duration: 300ms; animation-delay: 0ms; animation-name: fadeInUp;" class="wow fadeInUp animated" data-wow-duration="300ms" data-wow-delay="0ms">
                <span>Frapid</span> Framework
            </h2>
            <p style="visibility: visible; animation-duration: 300ms; animation-delay: 100ms; animation-name: fadeInUp;" class="wow fadeInUp animated" data-wow-duration="300ms" data-wow-delay="100ms">
                Howdy, your Frapid instance is now up and running. 
                <br/>
                Login to your admin area now and start building your website.
                
            </p>
            <p style="visibility: visible; animation-duration: 300ms; animation-delay: 200ms; animation-name: fadeInUp;" class="wow fadeInUp animated" data-wow-duration="300ms" data-wow-delay="200ms">
                <a class="btn btn-primary btn-lg" href="/dashboard">ADMIN AREA</a>
                <a class="btn btn-default btn-lg" href="http://docs.frapid.com" target="_blank">READ THE DOCS</a>
            </p>
            <img style="visibility: visible; animation-duration: 300ms; animation-delay: 300ms; animation-name: fadeIn;" 
                class="img-responsive wow fadeIn animated" src="/my/template/images/cta2/cta2-img.png" 
                alt="" data-wow-duration="300ms" data-wow-delay="300ms">
            </div>
        </div>
    </section>
    <section id="services">
        <div class="container">
            <div class="section-header">
                <h2 style="visibility: visible; animation-name: fadeInDown;" class="section-title text-center wow fadeInDown animated">
                Learn More About Frapid
                </h2>
                <p style="visibility: visible; animation-name: fadeInDown;" class="text-center wow fadeInDown animated">
                    Frapid is a multi-tenant application development framework which comes with a very liberal 
                    
                    <a href="https://opensource.org/licenses/MIT" target="_blank">MIT License</a>.
                    <br/>
                    Learn more about Frapid and quickly add contents in your website.
                
                </p>
            </div>
            <div class="row">
                <div class="features">
                    <div style="visibility: visible; animation-duration: 300ms; animation-delay: 0ms; animation-name: fadeInUp;" class="col-md-4 col-sm-6 wow fadeInUp animated" data-wow-duration="300ms" data-wow-delay="0ms">
                        <div class="media service-box">
                            <div class="pull-left">
                                <i class="fa fa-line-chart"></i>
                            </div>
                            <div class="media-body">
                                <h4 class="media-heading">Edit Header</h4>
                                <p>
                                Read the 
                                    <a href="http://docs.frapid.com" target="_blank">documentation</a> on  your site header.
                                
                                </p>
                            </div>
                        </div>
                    </div>
                    <!--/.col-md-4-->
                    <div style="visibility: visible; animation-duration: 300ms; animation-delay: 100ms; animation-name: fadeInUp;" class="col-md-4 col-sm-6 wow fadeInUp animated" data-wow-duration="300ms" data-wow-delay="100ms">
                        <div class="media service-box">
                            <div class="pull-left">
                                <i class="fa fa-cubes"></i>
                            </div>
                            <div class="media-body">
                                <h4 class="media-heading">Edit Footer</h4>
                                <p>
                                    Edit the contents and links on your 
                                    <a href="http://docs.frapid.com/site/footer" target="_blank">website footer</a>.
                                
                                </p>
                            </div>
                        </div>
                    </div>
                    <!--/.col-md-4-->
                    <div style="visibility: visible; animation-duration: 300ms; animation-delay: 200ms; animation-name: fadeInUp;" class="col-md-4 col-sm-6 wow fadeInUp animated" data-wow-duration="300ms" data-wow-delay="200ms">
                        <div class="media service-box">
                            <div class="pull-left">
                                <i class="fa fa-pie-chart"></i>
                            </div>
                            <div class="media-body">
                                <h4 class="media-heading">Manage Pages</h4>
                                <p>
                                    Quick add website pages, edit them, and 
                                    <a href="http://docs.frapid.com/site/contents" target="_blank">manage contents</a>.
                                
                                </p>
                            </div>
                        </div>
                    </div>
                    <!--/.col-md-4-->
                    <div style="visibility: visible; animation-duration: 300ms; animation-delay: 300ms; animation-name: fadeInUp;" class="col-md-4 col-sm-6 wow fadeInUp animated" data-wow-duration="300ms" data-wow-delay="300ms">
                        <div class="media service-box">
                            <div class="pull-left">
                                <i class="fa fa-bar-chart"></i>
                            </div>
                            <div class="media-body">
                                <h4 class="media-heading">Menu Builder</h4>
                                <p>
                                    Learn how you can add and edit 
                                    <a href="http://docs.frapid.com/site/menus" target="_blank">website menus</a>.
                                
                                </p>
                            </div>
                        </div>
                    </div>
                    <!--/.col-md-4-->
                    <div style="visibility: visible; animation-duration: 300ms; animation-delay: 400ms; animation-name: fadeInUp;" class="col-md-4 col-sm-6 wow fadeInUp animated" data-wow-duration="300ms" data-wow-delay="400ms">
                        <div class="media service-box">
                            <div class="pull-left">
                                <i class="fa fa-language"></i>
                            </div>
                            <div class="media-body">
                                <h4 class="media-heading">Develop Frapid Apps</h4>
                                <p>
                                    Develop a brand new 
                                    <a href="http://docs.frapid.com/site/develop-frapid-app" target="_blank">Frapid application</a> 
                                    and 
                                    <a href="http://docs.frapid.com/site/application-license" target="_blank">quickly monetize</a> it.
                                
                                </p>
                            </div>
                        </div>
                    </div>
                    <!--/.col-md-4-->
                    <div style="visibility: visible; animation-duration: 300ms; animation-delay: 500ms; animation-name: fadeInUp;" class="col-md-4 col-sm-6 wow fadeInUp animated" data-wow-duration="300ms" data-wow-delay="500ms">
                        <div class="media service-box">
                            <div class="pull-left">
                                <i class="fa fa-bullseye"></i>
                            </div>
                            <div class="media-body">
                                <h4 class="media-heading">Improve Frapid</h4>
                                <p>
                                    Improve Frapid by 
                                    <a href="https://github.com/frapid/frapid/issues/new" target="_blank">documenting</a> and fixing bugs.
                                
                                </p>
                            </div>
                        </div>
                    </div>
                    <!--/.col-md-4-->
                </div>
            </div>
            <!--/.row-->
        </div>
        <!--/.container-->
    </section>
    <section id="get-in-touch">
        <div class="container">
            <div class="section-header">
                <h2 class="section-title text-center wow fadeInDown">Get in Touch</h2>
                <p class="text-center wow fadeInDown">
                    Want us to build an application or website for you? 
                    <br/>
                    Why not contact us today? We will be happy to assist you.
                
                </p>
                <p class="text-center">
                    <a class="btn btn-default btn-lg" href="/contact-us">Contact Us</a>
                </p>
            </div>
        </div>
    </section>
    <!--/#get-in-touch-->';

    
INSERT INTO website.contacts(title, name, "position", address, city, state, country, postal_code, telephone, details, email, display_email, display_contact_form, status)
SELECT 'Corporate Headquarters', 'Your Office Name', '', 'Address', 'City', 'State', 'Country', '000', '000 000 000', '', '', false, true, true UNION ALL
SELECT 'Americas', 'John Doe', 'Client Partner', '', '', '', '', '', '', 'Mexico', '', false, true, true;

    

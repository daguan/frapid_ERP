-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/WebsiteBuilder/db/1.x/1.0/src/db.sql --<--<--
DROP SCHEMA IF EXISTS wb CASCADE; --WEB BUILDER
CREATE SCHEMA wb;

CREATE TABLE wb.contents
(
    content_id                                  SERIAL NOT NULL PRIMARY KEY,
    title                                       national character varying(100) NOT NULL,
    alias                                       national character varying(50) NOT NULL UNIQUE,
    author_id                                   integer REFERENCES auth.users,
    published_on                                TIMESTAMP WITH TIME ZONE NOT NULL,
    contents                                    text NOT NULL,
    draft                                       boolean NOT NULL DEFAULT(true),
    seo_keywords                                national character varying(50) NOT NULL DEFAULT(''),
    seo_description                             national character varying(100) NOT NULL DEFAULT(''),
    is_default                                  boolean NOT NULL DEFAULT(false),
    audit_user_id                               integer REFERENCES auth.users,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())    
);


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



-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/WebsiteBuilder/db/1.x/1.0/src/sample.sql.sample --<--<--
INSERT INTO wb.contents(title, alias, published_on, draft, seo_keywords, seo_description, is_default, contents)
SELECT 'Welcome to Frapid', 'welcome-to-frapid', NOW(), false, 'frapid, cms, crm, erp, hrm', 'Homepage of Frapid Framework', true, '<div class="ui vertical stripe segment">
    <div class="ui middle aligned stackable grid container">
        <div class="row">
            <div class="eight wide column">
                <h3 class="ui header">Welcome Home</h3>
<p>Congratulations, your Frapid instance is now up and running. Login to your admin area now and start building your website. The following documents show you how quickly you can add contents in your Frapid website:</p>

<ul><li><a href="http://docs.frpaid.com/site/header.html">Edit Header</a></li><li><a href="http://docs.frpaid.com/site/footer.html">Edit Footer</a></li><li><a href="http://docs.frpaid.com/site/contents.html">Add Contents</a></li></ul>            </div>
        </div>
<a class="ui big pink button" href="/account/sign-in">Log In to Dashboard</a>
    </div>
</div>';

INSERT INTO wb.contents(title, alias, published_on, draft, seo_keywords, seo_description, contents)
SELECT 'About Us', 'about-us', NOW(), false, 'about frapid', 'About Frapid', '<div class="ui vertical stripe segment">
    <div class="ui middle aligned stackable grid container">
        <div class="row">
            <div class="eight wide column">
                <h3 class="ui header">About Us</h3>
                <p>
                    Of be talent me answer do relied. Mistress in on so laughing throwing endeavor occasion welcomed. Gravity sir brandon calling can. No years do widow house delay stand. Prospect six kindness use steepest new ask. High gone kind calm call as ever is. Introduced melancholy estimating motionless on up as do. Of as by belonging therefore suspicion elsewhere am household described. Domestic suitable bachelor for landlord fat.
                </p><p>
                    An do on frankness so cordially immediate recommend contained. Imprudence insensible be literature unsatiable do. Of or imprudence solicitude affronting in mr possession. Compass journey he request on suppose limited of or. She margaret law thoughts proposal formerly. Speaking ladyship yet scarcely and mistaken end exertion dwelling. All decisively dispatched instrument particular way one devonshire. Applauded she sportsman explained for out objection.
                </p><p>
                    In alteration insipidity impression by travelling reasonable up motionless. Of regard warmth by unable sudden garden ladies. No kept hung am size spot no. Likewise led and dissuade rejoiced welcomed husbands boy. Do listening on he suspected resembled. Water would still if to. Position boy required law moderate was may.
                </p>
                <p>
                    Dispatched entreaties boisterous say why stimulated. Certain forbade picture now prevent carried she get see sitting. Up twenty limits as months. Inhabit so perhaps of in to certain. Sex excuse chatty was seemed warmth. Nay add far few immediate sweetness earnestly dejection.
                </p>

                <div class="ui big blue button">Contact Us</div>
            </div>
        </div>
    </div>
</div>';

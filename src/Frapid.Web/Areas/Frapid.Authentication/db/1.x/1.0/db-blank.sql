-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Authentication/db/1.x/1.0/src/10.db.sql --<--<--
DROP SCHEMA IF EXISTS auth CASCADE;
CREATE SCHEMA auth;


CREATE TABLE auth.roles
(
    role_id                                 integer PRIMARY KEY,
    role_name                               national character varying(100) NOT NULL UNIQUE,
    is_administrator                        boolean NOT NULL DEFAULT(false),
    audit_user_id                           integer,
    audit_ts                                TIMESTAMP WITH TIME ZONE NULL 
                                            DEFAULT(NOW())    
);

INSERT INTO auth.roles
SELECT     1,   'Guest',    false UNION ALL
SELECT    10,   'Client',   false UNION ALL
SELECT   100,   'Partner',  false UNION ALL
SELECT  1000,   'User',     false UNION ALL
SELECT 10000,   'Admin',    true;

CREATE TABLE auth.configuration_profiles
(
    profile_id                              SERIAL PRIMARY KEY,
    profile_name                            national character varying(100) NOT NULL UNIQUE,
    is_active                               boolean NOT NULL DEFAULT(true),    
    allow_registration                      boolean NOT NULL DEFAULT(true),
    registration_role_id                    integer NOT NULL REFERENCES auth.roles,
    registration_office_id                  integer NOT NULL REFERENCES core.offices,
    allow_facebook_registration             boolean NOT NULL DEFAULT(true),
    allow_google_registration               boolean NOT NULL DEFAULT(true),
    google_signin_client_id                 text,
    google_signin_scope                     text,
    facebook_app_id                         text,
    facebook_scope                          text,
    audit_user_id                           integer,
    audit_ts                                TIMESTAMP WITH TIME ZONE NULL 
                                            DEFAULT(NOW())    
);


CREATE UNIQUE INDEX configuration_profile_uix
ON auth.configuration_profiles(is_active)
WHERE is_active;

INSERT INTO auth.configuration_profiles(profile_name, is_active, allow_registration, registration_role_id, registration_office_id)
SELECT 'Default', true, true, 1, 1;


CREATE TABLE auth.registrations
(
    registration_id                         uuid PRIMARY KEY DEFAULT(gen_random_uuid()),
    name                                    national character varying(100),
    email                                   national character varying(100) NOT NULL UNIQUE,
    phone                                   national character varying(100),
    password                                text,
    browser                                 text,
    ip_address                              national character varying(50) NOT NULL,
    registered_on                           TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT(NOW()),
    confirmed                               boolean NOT NULL,
    confirmed_on                            TIMESTAMP WITH TIME ZONE
);


CREATE TABLE auth.users
(
    user_id                                 SERIAL PRIMARY KEY,
    email                                   national character varying(100) NOT NULL UNIQUE,
    password                                text,
    office_id                               integer NOT NULL REFERENCES core.offices,
    role_id                                 integer NOT NULL REFERENCES auth.roles,
    name                                    national character varying(100),
    phone                                   national character varying(100),
    api_key                                 text UNIQUE,
    status                                  boolean DEFAULT(true),    
    audit_user_id                           integer REFERENCES auth.users,
    audit_ts                                TIMESTAMP WITH TIME ZONE NULL 
                                            DEFAULT(NOW())    
);

CREATE FUNCTION auth.email_exists(_email national character varying(100))
RETURNS bool
AS
$$
    DECLARE _count                          integer;
BEGIN
    SELECT count(*) INTO _count FROM auth.users WHERE lower(email) = LOWER(_email);

    IF(COALESCE(_count, 0) =0) THEN
        SELECT count(*) INTO _count FROM auth.registrations WHERE lower(email) = LOWER(_email);
    END IF;
    
    RETURN COALESCE(_count, 0) > 0;
END
$$
LANGUAGE plpgsql;




ALTER TABLE core.currencies
ADD CONSTRAINT currencies_users_fk
FOREIGN KEY(audit_user_id)
REFERENCES auth.users;

ALTER TABLE core.offices
ADD CONSTRAINT offices_users_fk
FOREIGN KEY(audit_user_id)
REFERENCES auth.users;

ALTER TABLE auth.configuration_profiles
ADD CONSTRAINT configuration_profiles_users_fk
FOREIGN KEY(audit_user_id)
REFERENCES auth.users;

ALTER TABLE auth.roles
ADD CONSTRAINT roles_users_fk
FOREIGN KEY(audit_user_id)
REFERENCES auth.users;


CREATE TABLE auth.fb_access_tokens
(
    user_id                         integer PRIMARY KEY REFERENCES auth.users,
    fb_user_id                      text,
    token                           text
);


CREATE TABLE auth.google_access_tokens
(
    user_id                         integer PRIMARY KEY REFERENCES auth.users,
    token                           text
);

CREATE TABLE auth.sign_ins
(
    sign_in_id                              BIGSERIAL PRIMARY KEY,
    user_id                                 integer REFERENCES auth.users,
    browser                                 text,
    ip_address                              national character varying(50) NOT NULL,
    login_timestamp                         TIMESTAMP WITH TIME ZONE NOT NULL 
                                            DEFAULT(NOW()),
    culture                                 national character varying(12) NOT NULL    
);

CREATE FUNCTION auth.get_registration_role_id()
RETURNS integer
AS
$$
BEGIN
    RETURN registration_role_id
    FROM auth.configuration_profiles
    WHERE is_active;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION auth.get_registration_office_id()
RETURNS integer
AS
$$
BEGIN
    RETURN registration_office_id
    FROM auth.configuration_profiles
    WHERE is_active;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION auth.can_register_with_facebook()
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT 1 FROM auth.configuration_profiles
        WHERE is_active
        AND allow_registration
        AND allow_facebook_registration
    ) THEN
        RETURN true;
    END IF;
    
    RETURN false;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION auth.can_register_with_google()
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT 1 FROM auth.configuration_profiles
        WHERE is_active
        AND allow_registration
        AND allow_google_registration
    ) THEN
        RETURN true;
    END IF;
    
    RETURN false;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION auth.is_restricted_user(_email national character varying(100))
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT *
        FROM auth.users
        WHERE auth.users.email = _email
        AND NOT auth.users.status
    ) THEN
        RETURN true;
    END IF;
    
    RETURN false;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION auth.user_exists(_email national character varying(100))
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT *
        FROM auth.users
        WHERE auth.users.email = _email
    ) THEN
        RETURN true;
    END IF;
    
    RETURN false;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION auth.fb_user_exists(_user_id integer)
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT *
        FROM auth.fb_access_tokens
        WHERE auth.fb_access_tokens.user_id = _user_id
    ) THEN
        RETURN true;
    END IF;
    
    RETURN false;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION auth.google_user_exists(_user_id integer)
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT *
        FROM auth.google_access_tokens
        WHERE auth.google_access_tokens.user_id = _user_id
    ) THEN
        RETURN true;
    END IF;
    
    RETURN false;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION auth.get_user_id_by_email(_email national character varying(100))
RETURNS integer
AS
$$
BEGIN
    RETURN user_id
    FROM auth.users
    WHERE auth.users.email = _email;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION auth.fb_sign_in
(
    _fb_user_id                             text,
    _email                                  text,
    _token                                  text,
    _browser                                text,
    _ip_address                             text,
    _culture                                text
)
RETURNS TABLE
(
    sign_in_id                              bigint,
    status                                  boolean,
    message                                 text
)
AS
$$
    DECLARE _user_id                        integer;
    DECLARE _sign_in_id                     bigint;
    DECLARE _auto_register                  boolean = false;
BEGIN
    IF auth.is_restricted_user(_email) THEN
        --LOGIN IS RESTRICTED TO THIS USER
        RETURN QUERY
        SELECT NULL::bigint, false, 'Access is denied'::text;

        RETURN;
    END IF;

    IF NOT auth.user_exists(_email) AND auth.can_register_with_facebook() THEN
        INSERT INTO auth.users(role_id, office_id, email)
        SELECT auth.get_registration_role_id(), auth.get_registration_office_id(), _email
        RETURNING user_id INTO _user_id;
    END IF;

    IF NOT auth.fb_user_exists(_user_id) THEN
        INSERT INTO auth.fb_access_tokens(user_id, fb_user_id, token)
        SELECT COALESCE(_user_id, auth.get_user_id_by_email(_email)), _fb_user_id, _token;
    ELSE
        UPDATE auth.fb_access_tokens
        SET token = _token
        WHERE user_id = _user_id;    
    END IF;

    IF(_user_id IS NULL) THEN
        SELECT user_id INTO _user_id
        FROM auth.users
        WHERE auth.users.email = _email;
    END IF;
    
    INSERT INTO auth.sign_ins(user_id, browser, ip_address, login_timestamp, culture)
    SELECT _user_id, _browser, _ip_address, NOW(), _culture
    RETURNING auth.sign_ins.sign_in_id INTO _sign_in_id;

    RETURN QUERY
    SELECT _sign_in_id, true, 'Welcome'::text;
    RETURN;    
END
$$
LANGUAGE plpgsql;



CREATE FUNCTION auth.google_sign_in
(
    _email                                  text,
    _token                                  text,
    _browser                                text,
    _ip_address                             text,
    _culture                                text
)
RETURNS TABLE
(
    sign_in_id                              bigint,
    status                                  boolean,
    message                                 text
)
AS
$$
    DECLARE _user_id                        integer;
    DECLARE _sign_in_id                     bigint;
BEGIN    
    IF auth.is_restricted_user(_email) THEN
        --LOGIN IS RESTRICTED TO THIS USER
        RETURN QUERY
        SELECT NULL::bigint, false, 'Access is denied';

        RETURN;
    END IF;

    IF NOT auth.user_exists(_email) AND auth.can_register_with_google() THEN
        INSERT INTO auth.users(role_id, email)
        SELECT auth.get_registration_role_id(), auth.get_registration_office_id(), _email
        RETURNING user_id INTO _user_id;
    END IF;

    SELECT user_id INTO _user_id
    FROM auth.users
    WHERE auth.users.email = _email;

    IF NOT auth.google_user_exists(_user_id) THEN
        INSERT INTO auth.google_access_tokens(user_id, token)
        SELECT COALESCE(_user_id, auth.get_user_id_by_email(_email)), _token;
    ELSE
        UPDATE auth.google_access_tokens
        SET token = _token
        WHERE user_id = _user_id;    
    END IF;

        
    INSERT INTO auth.sign_ins(user_id, browser, ip_address, login_timestamp, culture)
    SELECT _user_id, _browser, _ip_address, NOW(), _culture
    RETURNING auth.sign_ins.sign_in_id INTO _sign_in_id;

    RETURN QUERY
    SELECT _sign_in_id, true, 'Welcome'::text;
    RETURN;    
END
$$
LANGUAGE plpgsql;


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Authentication/db/1.x/1.0/src/99.ownership.sql --<--<--
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

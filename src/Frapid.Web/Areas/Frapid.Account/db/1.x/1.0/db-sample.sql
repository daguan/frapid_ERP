-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Account/db/1.x/1.0/src/10.db.sql --<--<--
DROP SCHEMA IF EXISTS account CASCADE;
CREATE SCHEMA account;

CREATE TABLE account.roles
(
    role_id                                 integer PRIMARY KEY,
    role_name                               national character varying(100) NOT NULL UNIQUE,
    is_administrator                        boolean NOT NULL DEFAULT(false),
    audit_user_id                           integer,
    audit_ts                                TIMESTAMP WITH TIME ZONE NULL 
                                            DEFAULT(NOW())    
);

ALTER TABLE config.default_entity_access
ADD FOREIGN KEY(role_id) REFERENCES account.roles;

INSERT INTO account.roles
SELECT     1,   'Guest',    false UNION ALL
SELECT    10,   'Client',   false UNION ALL
SELECT   100,   'Partner',  false UNION ALL
SELECT  1000,   'User',     false UNION ALL
SELECT 10000,   'Admin',    true;

CREATE TABLE account.configuration_profiles
(
    profile_id                              SERIAL PRIMARY KEY,
    profile_name                            national character varying(100) NOT NULL UNIQUE,
    is_active                               boolean NOT NULL DEFAULT(true),    
    allow_registration                      boolean NOT NULL DEFAULT(true),
    registration_office_id                  integer NOT NULL REFERENCES config.offices,
    registration_role_id                    integer NOT NULL REFERENCES account.roles,
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
ON account.configuration_profiles(is_active)
WHERE is_active;

INSERT INTO account.configuration_profiles(profile_name, is_active, allow_registration, registration_role_id, registration_office_id)
SELECT 'Default', true, true, 1, 1;


CREATE TABLE account.registrations
(
    registration_id                         uuid PRIMARY KEY DEFAULT(gen_random_uuid()),
    name                                    national character varying(100),
    email                                   national character varying(100) NOT NULL,
    phone                                   national character varying(100),
    password                                text,
    browser                                 text,
    ip_address                              national character varying(50),
    registered_on                           TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT(NOW()),
    confirmed                               boolean DEFAULT(false),
    confirmed_on                            TIMESTAMP WITH TIME ZONE
);

CREATE UNIQUE INDEX registrations_email_uix
ON account.registrations(LOWER(email));

CREATE TABLE account.users
(
    user_id                                 SERIAL PRIMARY KEY,
    email                                   national character varying(100) NOT NULL,
    password                                text,
    office_id                               integer NOT NULL REFERENCES config.offices,
    role_id                                 integer NOT NULL REFERENCES account.roles,
    name                                    national character varying(100),
    phone                                   national character varying(100),
    access_token                            text UNIQUE,
    status                                  boolean DEFAULT(true),    
    audit_user_id                           integer REFERENCES account.users,
    audit_ts                                TIMESTAMP WITH TIME ZONE NULL 
                                            DEFAULT(NOW())    
);


CREATE UNIQUE INDEX users_email_uix
ON account.users(LOWER(email));

ALTER TABLE config.entity_access
ADD FOREIGN KEY(user_id) REFERENCES account.users;

ALTER TABLE config.default_entity_access
ADD FOREIGN KEY(audit_user_id) REFERENCES account.users;

ALTER TABLE config.entity_access
ADD FOREIGN KEY(audit_user_id) REFERENCES account.users;

ALTER TABLE website.contents
ADD FOREIGN KEY(author_id) REFERENCES account.users;

ALTER TABLE website.contacts
ADD FOREIGN KEY(audit_user_id) REFERENCES account.users;

ALTER TABLE website.menus
ADD FOREIGN KEY(audit_user_id) REFERENCES account.users;

ALTER TABLE website.menu_items
ADD FOREIGN KEY(audit_user_id) REFERENCES account.users;

ALTER TABLE website.menu_items
ADD FOREIGN KEY(audit_user_id) REFERENCES account.users;

ALTER TABLE config.filters
ADD FOREIGN KEY(audit_user_id) REFERENCES account.users;

ALTER TABLE config.kanbans
ADD FOREIGN KEY(user_id) REFERENCES account.users;

ALTER TABLE config.kanbans
ADD FOREIGN KEY(audit_user_id) REFERENCES account.users;

ALTER TABLE config.kanban_details
ADD FOREIGN KEY(audit_user_id) REFERENCES account.users;

ALTER TABLE config.flag_types
ADD FOREIGN KEY(audit_user_id) REFERENCES account.users;

ALTER TABLE config.flags
ADD FOREIGN KEY(user_id) REFERENCES account.users;


CREATE TABLE account.reset_requests
(
    request_id                              uuid PRIMARY KEY DEFAULT(gen_random_uuid()),
    user_id                                 integer NOT NULL REFERENCES account.users,
    email                                   text,
    name                                    text,
    requested_on                            TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT(NOW()),
    expires_on                              TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT(NOW() + INTERVAL '24 hours'),
    browser                                 text,
    ip_address                              national character varying(50),
    confirmed                               boolean DEFAULT(false),
    confirmed_on                            TIMESTAMP WITH TIME ZONE
);


CREATE FUNCTION account.has_active_reset_request(_email text)
RETURNS boolean
AS
$$
    DECLARE _expires_on                     TIMESTAMP WITH TIME ZONE = NOW() + INTERVAL '24 Hours';
BEGIN
    IF EXISTS
    (
        SELECT * FROM account.reset_requests
        WHERE LOWER(email) = LOWER(_email)
        AND expires_on <= _expires_on
    ) THEN        
        RETURN true;
    END IF;

    RETURN false;
END
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION account.reset_account
(
    _email                                  text,
    _browser                                text,
    _ip_address                             text
)
RETURNS SETOF account.reset_requests
AS
$$
    DECLARE _request_id                     uuid;
    DECLARE _user_id                        integer;
    DECLARE _name                           text;
    DECLARE _expires_on                     TIMESTAMP WITH TIME ZONE = NOW() + INTERVAL '24 Hours';
BEGIN
    IF(NOT account.user_exists(_email) OR account.is_restricted_user(_email)) THEN
        RETURN;
    END IF;

    SELECT
        user_id,
        name
    INTO
        _user_id,
        _name
    FROM account.users
    WHERE LOWER(email) = LOWER(_email);

    IF account.has_active_reset_request(_email) THEN
        RETURN QUERY
        SELECT * FROM account.reset_requests
        WHERE LOWER(email) = LOWER(_email)
        AND expires_on <= _expires_on
        LIMIT 1;
        
        RETURN;
    END IF;

    INSERT INTO account.reset_requests(user_id, email, name, browser, ip_address, expires_on)
    SELECT _user_id, _email, _name, _browser, _ip_address, _expires_on
    RETURNING request_id INTO _request_id;

    RETURN QUERY
    SELECT *
    FROM account.reset_requests
    WHERE request_id = _request_id;

    RETURN;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.email_exists(_email national character varying(100))
RETURNS bool
AS
$$
    DECLARE _count                          integer;
BEGIN
    SELECT count(*) INTO _count FROM account.users WHERE lower(email) = LOWER(_email);

    IF(COALESCE(_count, 0) =0) THEN
        SELECT count(*) INTO _count FROM account.registrations WHERE lower(email) = LOWER(_email);
    END IF;
    
    RETURN COALESCE(_count, 0) > 0;
END
$$
LANGUAGE plpgsql;


ALTER TABLE config.currencies
ADD CONSTRAINT currencies_users_fk
FOREIGN KEY(audit_user_id)
REFERENCES account.users;

ALTER TABLE config.offices
ADD CONSTRAINT offices_users_fk
FOREIGN KEY(audit_user_id)
REFERENCES account.users;

ALTER TABLE account.configuration_profiles
ADD CONSTRAINT configuration_profiles_users_fk
FOREIGN KEY(audit_user_id)
REFERENCES account.users;

ALTER TABLE account.roles
ADD CONSTRAINT roles_users_fk
FOREIGN KEY(audit_user_id)
REFERENCES account.users;


CREATE TABLE account.fb_access_tokens
(
    user_id                                 integer PRIMARY KEY REFERENCES account.users,
    fb_user_id                              text,
    token                                   text
);


CREATE TABLE account.google_access_tokens
(
    user_id                                 integer PRIMARY KEY REFERENCES account.users,
    token                                   text
);

CREATE TABLE account.logins
(
    login_id                                BIGSERIAL PRIMARY KEY,
    user_id                                 integer REFERENCES account.users,
    office_id                               integer REFERENCES config.offices,
    browser                                 text,
    ip_address                              national character varying(50),
    login_timestamp                         TIMESTAMP WITH TIME ZONE NOT NULL 
                                            DEFAULT(NOW()),
    culture                                 national character varying(12) NOT NULL    
);

CREATE FUNCTION account.get_registration_role_id()
RETURNS integer
AS
$$
BEGIN
    RETURN registration_role_id
    FROM account.configuration_profiles
    WHERE is_active;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.get_registration_office_id()
RETURNS integer
AS
$$
BEGIN
    RETURN registration_office_id
    FROM account.configuration_profiles
    WHERE is_active;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.can_register_with_facebook()
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT 1 FROM account.configuration_profiles
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

CREATE FUNCTION account.can_register_with_google()
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT 1 FROM account.configuration_profiles
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

CREATE FUNCTION account.is_restricted_user(_email national character varying(100))
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT *
        FROM account.users
        WHERE LOWER(account.users.email) = LOWER(_email)
        AND NOT account.users.status
    ) THEN
        RETURN true;
    END IF;
    
    RETURN false;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.user_exists(_email national character varying(100))
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT *
        FROM account.users
        WHERE LOWER(account.users.email) = LOWER(_email)
    ) THEN
        RETURN true;
    END IF;
    
    RETURN false;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.fb_user_exists(_user_id integer)
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT *
        FROM account.fb_access_tokens
        WHERE account.fb_access_tokens.user_id = _user_id
    ) THEN
        RETURN true;
    END IF;
    
    RETURN false;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.google_user_exists(_user_id integer)
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT *
        FROM account.google_access_tokens
        WHERE account.google_access_tokens.user_id = _user_id
    ) THEN
        RETURN true;
    END IF;
    
    RETURN false;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.get_user_id_by_email(_email national character varying(100))
RETURNS integer
AS
$$
BEGIN
    RETURN user_id
    FROM account.users
    WHERE LOWER(account.users.email) = LOWER(_email);
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.fb_sign_in
(
    _fb_user_id                             text,
    _email                                  text,
    _office_id                              integer,
    _name                                   text,
    _token                                  text,
    _browser                                text,
    _ip_address                             text,
    _culture                                text
)
RETURNS TABLE
(
    login_id                                bigint,
    status                                  boolean,
    message                                 text
)
AS
$$
    DECLARE _user_id                        integer;
    DECLARE _login_id                       bigint;
    DECLARE _auto_register                  boolean = false;
BEGIN
    IF account.is_restricted_user(_email) THEN
        --LOGIN IS RESTRICTED TO THIS USER
        RETURN QUERY
        SELECT NULL::bigint, false, 'Access is denied'::text;

        RETURN;
    END IF;

    SELECT user_id INTO _user_id
    FROM account.users
    WHERE LOWER(account.users.email) = LOWER(_email);

    IF NOT account.user_exists(_email) AND account.can_register_with_facebook() THEN
        INSERT INTO account.users(role_id, office_id, email, name)
        SELECT account.get_registration_role_id(), account.get_registration_office_id(), _email, _name
        RETURNING user_id INTO _user_id;
    END IF;

    IF NOT account.fb_user_exists(_user_id) THEN
        INSERT INTO account.fb_access_tokens(user_id, fb_user_id, token)
        SELECT COALESCE(_user_id, account.get_user_id_by_email(_email)), _fb_user_id, _token;
    ELSE
        UPDATE account.fb_access_tokens
        SET token = _token
        WHERE user_id = _user_id;    
    END IF;

    IF(_user_id IS NULL) THEN
        SELECT user_id INTO _user_id
        FROM account.users
        WHERE LOWER(account.users.email) = LOWER(_email);
    END IF;
    
    INSERT INTO account.logins(user_id, office_id, browser, ip_address, login_timestamp, culture)
    SELECT _user_id, _office_id, _browser, _ip_address, NOW(), _culture
    RETURNING account.logins.login_id INTO _login_id;

    RETURN QUERY
    SELECT _login_id, true, 'Welcome'::text;
    RETURN;    
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.sign_in
(
    _email                                  text,
    _challenge                              text,
    _password                               text
)
RETURNS TABLE
(
    login_id                                 bigint,
    status                                  boolean,
    message                                 text
)
AS
$$
    DECLARE 
BEGIN
    
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.google_sign_in
(
    _email                                  text,
    _office_id                              integer,
    _name                                   text,
    _token                                  text,
    _browser                                text,
    _ip_address                             text,
    _culture                                text
)
RETURNS TABLE
(
    login_id                                bigint,
    status                                  boolean,
    message                                 text
)
AS
$$
    DECLARE _user_id                        integer;
    DECLARE _login_id                       bigint;
BEGIN    
    IF account.is_restricted_user(_email) THEN
        --LOGIN IS RESTRICTED TO THIS USER
        RETURN QUERY
        SELECT NULL::bigint, false, 'Access is denied';

        RETURN;
    END IF;

    IF NOT account.user_exists(_email) AND account.can_register_with_google() THEN
        INSERT INTO account.users(role_id, office_id, email, name)
        SELECT account.get_registration_role_id(), account.get_registration_office_id(), _email, _name
        RETURNING user_id INTO _user_id;
    END IF;

    SELECT user_id INTO _user_id
    FROM account.users
    WHERE LOWER(account.users.email) = LOWER(_email);

    IF NOT account.google_user_exists(_user_id) THEN
        INSERT INTO account.google_access_tokens(user_id, token)
        SELECT COALESCE(_user_id, account.get_user_id_by_email(_email)), _token;
    ELSE
        UPDATE account.google_access_tokens
        SET token = _token
        WHERE user_id = _user_id;    
    END IF;

        
    INSERT INTO account.logins(user_id, office_id, browser, ip_address, login_timestamp, culture)
    SELECT _user_id, _office_id, _browser, _ip_address, NOW(), _culture
    RETURNING account.logins.login_id INTO _login_id;

    RETURN QUERY
    SELECT _login_id, true, 'Welcome'::text;
    RETURN;    
END
$$
LANGUAGE plpgsql;


CREATE FUNCTION account.can_confirm_registration(_token uuid)
RETURNS boolean
STABLE
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT *
        FROM account.registrations
        WHERE registration_id = _token
        AND NOT confirmed
    ) THEN
        RETURN true;
    END IF;

    RETURN false;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.confirm_registration(_token uuid)
RETURNS boolean
AS
$$
    DECLARE _can_confirm        boolean;
    DECLARE _office_id          integer;
    DECLARE _role_id            integer;
BEGIN
    _can_confirm := account.can_confirm_registration(_token);

    IF(NOT _can_confirm) THEN
        RETURN false;
    END IF;

    SELECT
        registration_office_id,
        registration_role_id
    INTO
        _office_id,
        _role_id
    FROM account.configuration_profiles
    WHERE is_active
    LIMIT 1;

    INSERT INTO account.users(email, password, office_id, role_id, name, phone)
    SELECT email, password, _office_id, _role_id, name, phone
    FROM account.registrations
    WHERE registration_id = _token;

    UPDATE account.registrations
    SET 
        confirmed = true, 
        confirmed_on = NOW()
    WHERE registration_id = _token;
    
    RETURN true;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.has_account(_email national character varying(100))
RETURNS bool
AS
$$
    DECLARE _count                          integer;
BEGIN
    SELECT count(*) INTO _count FROM account.users WHERE lower(email) = LOWER(_email);
    RETURN COALESCE(_count, 0) = 1;
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.complete_reset
(
    _request_id                     uuid,
    _password                       text
)
RETURNS void
AS
$$
    DECLARE _user_id                integer;
    DECLARE _email                  text;
BEGIN
    SELECT
        account.users.user_id,
        account.users.email
    INTO
        _user_id,
        _email
    FROM account.reset_requests
    INNER JOIN account.users
    ON account.users.user_id = account.reset_requests.user_id
    WHERE account.reset_requests.request_id = _request_id
    AND expires_on >= NOW();

    _password = encode(digest(_email || _password, 'sha512'), 'hex');
    
    UPDATE account.users
    SET
        password = _password
    WHERE user_id = _user_id;

    UPDATE account.reset_requests
    SET confirmed = true, confirmed_on = NOW();
END
$$
LANGUAGE plpgsql;

CREATE FUNCTION account.sign_in
(
    _email                                  text,
    _office_id                              integer,
    _challenge                              text,
    _password                               text,
    _browser                                text,
    _ip_address                             text,
    _culture                                text
)
RETURNS TABLE
(
    login_id                                bigint,
    status                                  boolean,
    message                                 text
)
AS
$$
    DECLARE _login_id                       bigint;
    DECLARE _user_id                        integer;
BEGIN
    IF account.is_restricted_user(_email) OR COALESCE(_challenge, '') = '' THEN
        RETURN QUERY
        SELECT NULL::bigint, false, 'Access is denied'::text;

        RETURN;
    END IF;

    IF NOT EXISTS
    (
        SELECT 1
        FROM account.users
        WHERE encode(digest(_challenge || password, 'sha512'), 'hex') = _password
    ) THEN
        RETURN QUERY
        SELECT NULL::bigint, false, 'Access is denied'::text;

        RETURN;
    END IF;

    SELECT user_id INTO _user_id
    FROM account.users
    WHERE email = _email;

    INSERT INTO account.logins(user_id, office_id, browser, ip_address, login_timestamp, culture)
    SELECT _user_id, _office_id, _browser, _ip_address, NOW(), _culture
    RETURNING account.logins.login_id INTO _login_id;
    
    RETURN QUERY
    SELECT _login_id, true, 'Welcome'::text;
    RETURN;    
END
$$
LANGUAGE plpgsql;

CREATE VIEW account.sign_in_view
AS
SELECT
    account.logins.login_id,
    account.users.email,
    account.logins.user_id,
    account.roles.role_id,
    account.roles.role_name,
    account.roles.is_administrator,
    account.logins.browser,
    account.logins.ip_address,
    account.logins.login_timestamp,
    account.logins.culture,
    account.logins.office_id,
    config.offices.office_name,
    config.offices.office_code || ' (' || config.offices.office_name || ')' AS office
FROM account.logins
INNER JOIN account.users
ON account.users.user_id = account.logins.user_id
INNER JOIN account.roles
ON account.roles.role_id = account.users.role_id
INNER JOIN config.offices
ON config.offices.office_id = account.logins.office_id;

SELECT * FROM config.create_app('Frapid.Account', 'Account', '1.0', 'MixERP Inc.', 'December 1, 2015', 'grey lock', '/dashboard/account/configuration-profile', '{Frapid.WebsiteBuilder}'::text[]);

SELECT * FROM config.create_menu('Frapid.Account', 'Tasks', '', '', '');
SELECT * FROM config.create_menu('Frapid.Account', 'Roles', '/dashboard/account/roles', '', 'Tasks');
SELECT * FROM config.create_menu('Frapid.Account', 'Configuration Profile', '/dashboard/account/configuration-profile', '', 'Tasks');
SELECT * FROM config.create_menu('Frapid.Account', 'User Management', '/dashboard/account/user-management', '', 'Tasks');

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Account/db/1.x/1.0/src/99.ownership.sql --<--<--
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

DROP SCHEMA IF EXISTS config CASCADE;
CREATE SCHEMA config;

CREATE TABLE config.currencies
(
    currency_code                           national character varying(12) PRIMARY KEY,
    currency_symbol                         national character varying(12) NOT NULL,
    currency_name                           national character varying(48) NOT NULL UNIQUE,
    hundredth_name                          national character varying(48) NOT NULL,
    audit_user_id                           integer,
    audit_ts                                TIMESTAMP WITH TIME ZONE NULL 
                                            DEFAULT(NOW())
);

CREATE TABLE config.offices
(
    office_id                               SERIAL PRIMARY KEY,
    office_code                             national character varying(12) NOT NULL,
    office_name                             national character varying(150) NOT NULL,
    nick_name                               national character varying(50),
    registration_date                       date,
    currency_code                           national character varying(12) REFERENCES config.currencies,
    po_box                                  national character varying(128),
    address_line_1                          national character varying(128),   
    address_line_2                          national character varying(128),
    street                                  national character varying(50),
    city                                    national character varying(50),
    state                                   national character varying(50),
    zip_code                                national character varying(24),
    country                                 national character varying(50),
    phone                                   national character varying(24),
    fax                                     national character varying(24),
    email                                   national character varying(128),
    url                                     national character varying(50),
    registration_number                     national character varying(24),
    pan_number                              national character varying(24),
    allow_transaction_posting               boolean not null DEFAULT(true),
    parent_office_id                        integer NULL REFERENCES config.offices,
    audit_user_id                           integer NULL,
    audit_ts                                TIMESTAMP WITH TIME ZONE NULL 
                                            DEFAULT(NOW())
);

INSERT INTO config.offices(office_code, office_name)
SELECT 'DEF', 'Default';

CREATE TABLE config.smtp_configs
(
    smtp_id                                     SERIAL PRIMARY KEY,
    configuration_name                          national character varying(256) NOT NULL UNIQUE,
    enabled                                     boolean NOT NULL DEFAULT false,
    is_default                                  boolean NOT NULL DEFAULT false,
    from_display_name                           national character varying(256) NOT NULL,
    from_email_address                          national character varying(256) NOT NULL,
    smtp_host                                   national character varying(256) NOT NULL,
    smtp_enable_ssl                             boolean NOT NULL DEFAULT true,
    smtp_username                               national character varying(256) NOT NULL,
    smtp_password                               national character varying(256) NOT NULL,
    smtp_port                                   integer NOT NULL DEFAULT(587),
    audit_user_id                               integer,
    audit_ts                                    timestamp with time zone DEFAULT now()
);


CREATE TABLE config.email_queue
(
    queue_id                                    BIGSERIAL NOT NULL PRIMARY KEY,
    subject                                     national character varying(256) NOT NULL,
    send_to                                     national character varying(256) NOT NULL,
    attachments                                 text,
    message                                     text NOT NULL,
    added_on                                    TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT(NOW()),
    delivered                                   boolean NOT NULL DEFAULT(false),
    delivered_on                                TIMESTAMP WITH TIME ZONE,
    canceled                                    boolean NOT NULL DEFAULT(false),
    canceled_on                                 TIMESTAMP WITH TIME ZONE
);

CREATE TABLE config.apps
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
ON config.apps(UPPER(app_name));

CREATE TABLE config.app_dependencies
(
    app_name                                    national character varying(100) REFERENCES config.apps,
    depends_on                                  national character varying(100) REFERENCES config.apps
);


CREATE TABLE config.menus
(
    menu_id                                     SERIAL PRIMARY KEY,
    sort                                        integer,
    app_name                                    national character varying(100) NOT NULL REFERENCES config.apps,
    menu_name                                   national character varying(100) NOT NULL,
    url                                         text,
    icon                                        national character varying(100),
    parent_menu_id                              integer REFERENCES config.menus
);

CREATE UNIQUE INDEX menus_app_name_menu_name_uix
ON config.menus(UPPER(app_name), UPPER(menu_name));


CREATE TABLE config.access_types
(
    access_type_id                  integer NOT NULL PRIMARY KEY,
    access_type_name                national character varying(48) NOT NULL UNIQUE
);


CREATE TABLE config.default_entity_access
(
    default_entity_access_id        SERIAL NOT NULL PRIMARY KEY,
    entity_name                     national character varying(128) NULL,
    role_id                         integer NOT NULL REFERENCES account.roles,
    access_type_id                  integer NULL REFERENCES config.access_types,
    allow_access                    boolean NOT NULL,
    audit_user_id                   integer NULL REFERENCES account.users,
    audit_ts                        TIMESTAMP WITH TIME ZONE NULL 
                                    DEFAULT(NOW())
);

CREATE TABLE config.entity_access
(
    entity_access_id                SERIAL NOT NULL PRIMARY KEY,
    entity_name                     national character varying(128) NULL,
    user_id                         integer NOT NULL REFERENCES account.users,
    access_type_id                  integer NULL REFERENCES config.access_types,
    allow_access                    boolean NOT NULL,
    audit_user_id                   integer NULL REFERENCES account.users,
    audit_ts                        TIMESTAMP WITH TIME ZONE NULL 
                                    DEFAULT(NOW())
);

DROP FUNCTION IF EXISTS config.has_access(_user_id integer, _entity text, _access_type_id integer);

CREATE FUNCTION config.has_access(_user_id integer, _entity text, _access_type_id integer)
RETURNS boolean
AS
$$
    DECLARE _role_id                    integer;
    DECLARE _group_config               boolean = NULL;
    DECLARE _user_config                boolean = NULL;
    DECLARE _config                     boolean = true;
BEGIN
    SELECT role_id INTO _role_id FROM account.users WHERE user_id = _user_id;

    --GROUP config BASED ON ALL ENTITIES AND ALL ACCESS TYPES
    IF EXISTS
    (
        SELECT * FROM config.default_entity_access
        WHERE role_id = _role_id
        AND NOT allow_access
        AND access_type_id IS NULL
        AND COALESCE(entity_name, '') = ''
    ) THEN
        _group_config = false;
    END IF;

    --GROUP config BASED ON ALL ENTITIES AND SPECIFIED ACCESS TYPE
    IF EXISTS
    (
        SELECT * FROM config.default_entity_access
        WHERE role_id = _role_id
        AND NOT allow_access
        AND access_type_id = _access_type_id
        AND COALESCE(entity_name, '') = ''
    ) THEN
        _group_config = false;
    END IF;
 

    --GROUP config BASED ON SPECIFIED ENTITY AND ALL ACCESS TYPES
    IF EXISTS
    (
        SELECT * FROM config.default_entity_access
        WHERE role_id = _role_id
        AND NOT allow_access
        AND access_type_id IS NULL
        AND entity_name = _entity
    ) THEN
        _group_config = false;
    END IF;

    --GROUP config BASED ON SPECIFIED ENTITY AND SPECIFIED ACCESS TYPE
    IF EXISTS
    (
        SELECT * FROM config.default_entity_access
        WHERE role_id = _role_id
        AND NOT allow_access
        AND access_type_id = _access_type_id
        AND entity_name = _entity
    ) THEN
        _group_config = false;
    END IF;


    --USER config BASED ON ALL ENTITIES AND ALL ACCESS TYPES
    SELECT allow_access INTO _user_config FROM config.entity_access
    WHERE user_id = _user_id
    AND access_type_id IS NULL
    AND COALESCE(entity_name, '') = '';

    --USER config BASED ON SPECIFIED ENTITY AND ALL ACCESS TYPES
    IF(_user_config IS NULL) THEN
        SELECT allow_access INTO _user_config 
        FROM config.entity_access
        WHERE user_id = _user_id
        AND access_type_id IS NULL
        AND entity_name = _entity;
    END IF;
 
    --USER config BASED ON ALL ENTITIES AND SPECIFIED ACCESS TYPE
    IF(_user_config IS NULL) THEN
        SELECT allow_access INTO _user_config FROM config.entity_access
        WHERE user_id = _user_id
        AND access_type_id = _access_type_id
        AND COALESCE(entity_name, '') = '';
    END IF;
 

    --USER config BASED ON SPECIFIED ENTITY AND SPECIFIED ACCESS TYPE
    IF(_user_config IS NULL) THEN
        SELECT allow_access INTO _user_config FROM config.entity_access
        WHERE user_id = _user_id
        AND access_type_id = _access_type_id
        AND entity_name = _entity;
    END IF;

    IF(_group_config IS NOT NULL) THEN
        _config := _group_config;
    END IF;

    IF(_user_config IS NOT NULL) THEN
        _config := _user_config;
    END IF;

    RETURN _config;
END
$$
LANGUAGE plpgsql;


CREATE FUNCTION config.get_user_id_by_login_id(_login_id bigint)
RETURNS integer
AS
$$
BEGIN
    RETURN 
    user_id
    FROM account.logins
    WHERE login_id = _login_id;
END
$$
LANGUAGE plpgsql;
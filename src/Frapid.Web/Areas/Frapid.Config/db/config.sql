-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/1.x/1.0/src/00.db core/extensions.sql --<--<--
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/1.x/1.0/src/00.db core/postgresql-roles.sql --<--<--
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



-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/1.x/1.0/src/01.types-domains-tables-and-constraints/tables-and-constraints.sql --<--<--
DROP SCHEMA IF EXISTS config CASCADE;
CREATE SCHEMA config;

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




CREATE TABLE config.kanbans
(
    kanban_id                               BIGSERIAL NOT NULL PRIMARY KEY,
    object_name                             national character varying(128) NOT NULL,
    user_id                                 integer,
    kanban_name                             national character varying(128) NOT NULL,
    description                             text,
    audit_user_id                           integer,
    audit_ts                                TIMESTAMP WITH TIME ZONE NULL 
                                            DEFAULT(NOW())    
);
CREATE TABLE config.kanban_details
(
    kanban_detail_id                        BIGSERIAL NOT NULL PRIMARY KEY,
    kanban_id                               bigint NOT NULL REFERENCES config.kanbans(kanban_id),
    rating                                  smallint CHECK(rating>=0 AND rating<=5),
    resource_id                             national character varying(128) NOT NULL,
    audit_user_id                           integer NULL,
    audit_ts                                TIMESTAMP WITH TIME ZONE NULL 
                                            DEFAULT(NOW())    
);

CREATE UNIQUE INDEX kanban_details_kanban_id_resource_id_uix
ON config.kanban_details(kanban_id, resource_id);

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
    from_name                                   national character varying(256) NOT NULL,
    reply_to                                    national character varying(256) NOT NULL,
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
    app_dependency_id                           SERIAL PRIMARY KEY,
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
    role_id                         integer NOT NULL,
    access_type_id                  integer NULL REFERENCES config.access_types,
    allow_access                    boolean NOT NULL,
    audit_user_id                   integer NULL,
    audit_ts                        TIMESTAMP WITH TIME ZONE NULL 
                                    DEFAULT(NOW())
);

CREATE TABLE config.entity_access
(
    entity_access_id                SERIAL NOT NULL PRIMARY KEY,
    entity_name                     national character varying(128) NULL,
    user_id                         integer NOT NULL,
    access_type_id                  integer NULL REFERENCES config.access_types,
    allow_access                    boolean NOT NULL,
    audit_user_id                   integer NULL,
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

CREATE TABLE config.filters
(
    filter_id                       BIGSERIAL NOT NULL PRIMARY KEY,
    object_name                     text NOT NULL,
    filter_name                     text NOT NULL,
    is_default                      boolean NOT NULL DEFAULT(false),
    is_default_admin                boolean NOT NULL DEFAULT(false),
    filter_statement                national character varying(12) NOT NULL DEFAULT('WHERE'),
    column_name                     text NOT NULL,
    filter_condition                integer NOT NULL,
    filter_value                    text,
    filter_and_value                text,
    audit_user_id                   integer,
    audit_ts                        TIMESTAMP WITH TIME ZONE NULL 
                                    DEFAULT(NOW())
);

CREATE INDEX filters_object_name_inx
ON config.filters(object_name);



CREATE TABLE config.custom_field_data_types
(
    data_type                       national character varying(50) NOT NULL PRIMARY KEY,
    is_number                       boolean DEFAULT(false),
    is_date                         boolean DEFAULT(false),
    is_boolean                      boolean DEFAULT(false),
    is_long_text                    boolean DEFAULT(false)
);

CREATE TABLE config.custom_field_forms
(
    form_name                       national character varying(100) NOT NULL PRIMARY KEY,
    table_name                      national character varying(100) NOT NULL UNIQUE,
    key_name                        national character varying(100) NOT NULL        
);


CREATE TABLE config.custom_field_setup
(
    custom_field_setup_id           SERIAL NOT NULL PRIMARY KEY,
    form_name                       national character varying(100) NOT NULL
                                    REFERENCES config.custom_field_forms,
    field_order                     integer NOT NULL DEFAULT(0),
    field_name                      national character varying(100) NOT NULL,
    field_label                     national character varying(100) NOT NULL,                   
    data_type                       national character varying(50)
                                    REFERENCES config.custom_field_data_types,
    description                     text NOT NULL
);


CREATE TABLE config.custom_fields
(
    custom_field_id             BIGSERIAL NOT NULL PRIMARY KEY,
    custom_field_setup_id       integer NOT NULL REFERENCES config.custom_field_setup,
    resource_id                 text NOT NULL,
    value                       text
);


CREATE TABLE config.flag_types
(
    flag_type_id                            SERIAL PRIMARY KEY,
    flag_type_name                          national character varying(24) NOT NULL,
    background_color                        color NOT NULL,
    foreground_color                        color NOT NULL,
    audit_user_id                           integer NULL,
    audit_ts                                TIMESTAMP WITH TIME ZONE NULL
                                            DEFAULT(NOW())
);

COMMENT ON TABLE config.flag_types IS 'Flags are used by users to mark transactions. The flags created by a user is not visible to others.';

CREATE TABLE config.flags
(
    flag_id                                 BIGSERIAL PRIMARY KEY,
    user_id                                 integer NOT NULL,
    flag_type_id                            integer NOT NULL REFERENCES config.flag_types(flag_type_id),
    resource                                text, --Fully qualified resource name. Example: transactions.non_gl_stock_master.
    resource_key                            text, --The unique identifier for lookup. Example: non_gl_stock_master_id,
    resource_id                             text, --The value of the unique identifier to lookup for,
    flagged_on                              TIMESTAMP WITH TIME ZONE NULL 
                                            DEFAULT(NOW())
);

CREATE UNIQUE INDEX flags_user_id_resource_resource_id_uix
ON config.flags(user_id, UPPER(resource), UPPER(resource_key), UPPER(resource_id));





DROP VIEW IF EXISTS config.custom_field_view;

CREATE VIEW config.custom_field_view
AS
SELECT
    config.custom_field_forms.table_name,
    config.custom_field_forms.key_name,
    config.custom_field_setup.custom_field_setup_id,
    config.custom_field_setup.form_name,
    config.custom_field_setup.field_order,
    config.custom_field_setup.field_name,
    config.custom_field_setup.field_label,
    config.custom_field_setup.description,
    config.custom_field_data_types.data_type,
    config.custom_field_data_types.is_number,
    config.custom_field_data_types.is_date,
    config.custom_field_data_types.is_boolean,
    config.custom_field_data_types.is_long_text,
    config.custom_fields.resource_id,
    config.custom_fields.value
FROM config.custom_field_setup
INNER JOIN config.custom_field_data_types
ON config.custom_field_data_types.data_type = config.custom_field_setup.data_type
INNER JOIN config.custom_field_forms
ON config.custom_field_forms.form_name = config.custom_field_setup.form_name
INNER JOIN config.custom_fields
ON config.custom_fields.custom_field_setup_id = config.custom_field_setup.custom_field_setup_id
ORDER BY field_order;


















CREATE FUNCTION config.get_flag_type_id
(
    user_id_        integer,
    resource_       text,
    resource_key_   text,
    resource_id_    text
)
RETURNS integer
STABLE
AS
$$
BEGIN
    RETURN flag_type_id
    FROM config.flags
    WHERE user_id=$1
    AND resource=$2
    AND resource_key=$3
    AND resource_id=$4;
END
$$
LANGUAGE plpgsql;










CREATE FUNCTION config.create_flag
(
    user_id_            integer,
    flag_type_id_       integer,
    resource_           text,
    resource_key_       text,
    resource_id_        text
)
RETURNS void
VOLATILE
AS
$$
BEGIN
    IF NOT EXISTS(SELECT * FROM config.flags WHERE user_id=user_id_ AND resource=resource_ AND resource_key=resource_key_ AND resource_id=resource_id_) THEN
        INSERT INTO config.flags(user_id, flag_type_id, resource, resource_key, resource_id)
        SELECT user_id_, flag_type_id_, resource_, resource_key_, resource_id_;
    ELSE
        UPDATE config.flags
        SET
            flag_type_id=flag_type_id_
        WHERE 
            user_id=user_id_ 
        AND 
            resource=resource_ 
        AND 
            resource_key=resource_key_ 
        AND 
            resource_id=resource_id_;
    END IF;
END
$$
LANGUAGE plpgsql;


DROP VIEW IF EXISTS config.flag_view;

CREATE VIEW config.flag_view
AS
SELECT
    config.flags.flag_id,
    config.flags.user_id,
    config.flags.flag_type_id,
    config.flags.resource_id,
    config.flags.resource,
    config.flags.resource_key,
    config.flags.flagged_on,
    config.flag_types.flag_type_name,
    config.flag_types.background_color,
    config.flag_types.foreground_color
FROM config.flags
INNER JOIN config.flag_types
ON config.flags.flag_type_id = config.flag_types.flag_type_id;


DROP VIEW IF EXISTS config.filter_name_view;

CREATE VIEW config.filter_name_view
AS
SELECT
    DISTINCT
    object_name,
    filter_name,
    is_default
FROM config.filters;

CREATE VIEW config.custom_field_definition_view
AS
SELECT
    config.custom_field_forms.table_name,
    config.custom_field_forms.key_name,
    config.custom_field_setup.custom_field_setup_id,
    config.custom_field_setup.form_name,
    config.custom_field_setup.field_order,
    config.custom_field_setup.field_name,
    config.custom_field_setup.field_label,
    config.custom_field_setup.description,
    config.custom_field_data_types.data_type,
    config.custom_field_data_types.is_number,
    config.custom_field_data_types.is_date,
    config.custom_field_data_types.is_boolean,
    config.custom_field_data_types.is_long_text,
    ''::text AS resource_id,
    ''::text AS value
FROM config.custom_field_setup
INNER JOIN config.custom_field_data_types
ON config.custom_field_data_types.data_type = config.custom_field_setup.data_type
INNER JOIN config.custom_field_forms
ON config.custom_field_forms.form_name = config.custom_field_setup.form_name
ORDER BY field_order;



DROP FUNCTION IF EXISTS config.get_custom_field_setup_id_by_table_name
(
    _schema_name national character varying(100), 
    _table_name national character varying(100),
    _field_name national character varying(100)
);

CREATE FUNCTION config.get_custom_field_setup_id_by_table_name
(
    _schema_name national character varying(100), 
    _table_name national character varying(100),
    _field_name national character varying(100)
)
RETURNS integer
AS
$$
BEGIN
    RETURN custom_field_setup_id
    FROM config.custom_field_setup
    WHERE form_name = config.get_custom_field_form_name(_schema_name, _table_name)
    AND field_name = _field_name;
END
$$
LANGUAGE plpgsql;



DROP FUNCTION IF EXISTS config.get_custom_field_definition
(
    _table_name             text,
    _resource_id            text
);

CREATE FUNCTION config.get_custom_field_definition
(
    _table_name             text,
    _resource_id            text
)
RETURNS TABLE
(
    table_name              national character varying(100),
    key_name                national character varying(100),
    custom_field_setup_id   integer,
    form_name               national character varying(100),
    field_order             integer,
    field_name              national character varying(100),
    field_label             national character varying(100),
    description             text,
    data_type               national character varying(50),
    is_number               boolean,
    is_date                 boolean,
    is_boolean              boolean,
    is_long_text            boolean,
    resource_id             text,
    value                   text
)
AS
$$
BEGIN
    DROP TABLE IF EXISTS definition_temp;
    CREATE TEMPORARY TABLE definition_temp
    (
        table_name              national character varying(100),
        key_name                national character varying(100),
        custom_field_setup_id   integer,
        form_name               national character varying(100),
        field_order             integer,
        field_name              national character varying(100),
        field_label             national character varying(100),
        description             text,
        data_type               national character varying(50),
        is_number               boolean,
        is_date                 boolean,
        is_boolean              boolean,
        is_long_text            boolean,
        resource_id             text,
        value                   text
    ) ON COMMIT DROP;
    
    INSERT INTO definition_temp
    SELECT * FROM config.custom_field_definition_view
    WHERE config.custom_field_definition_view.table_name = _table_name
    ORDER BY field_order;

    UPDATE definition_temp
    SET resource_id = _resource_id;

    UPDATE definition_temp
    SET value = config.custom_fields.value
    FROM config.custom_fields
    WHERE definition_temp.custom_field_setup_id = config.custom_fields.custom_field_setup_id
    AND config.custom_fields.resource_id = _resource_id;
    
    RETURN QUERY
    SELECT * FROM definition_temp;
END
$$
LANGUAGE plpgsql;




DO
$$
BEGIN
    IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='Text') THEN
        INSERT INTO config.custom_field_data_types(data_type, is_number)
        SELECT 'Text', true;
    END IF;

    IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='Number') THEN
        INSERT INTO config.custom_field_data_types(data_type, is_number)
        SELECT 'Number', true;
    END IF;

    IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='Date') THEN
        INSERT INTO config.custom_field_data_types(data_type, is_date)
        SELECT 'Date', true;
    END IF;

    IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='True/False') THEN
        INSERT INTO config.custom_field_data_types(data_type, is_boolean)
        SELECT 'True/False', true;
    END IF;

    IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='Long Text') THEN
        INSERT INTO config.custom_field_data_types(data_type, is_long_text)
        SELECT 'Long Text', true;
    END IF;
END
$$
LANGUAGE plpgsql;

DROP FUNCTION IF EXISTS config.get_custom_field_form_name
(
    _table_name character varying
);

CREATE FUNCTION config.get_custom_field_form_name
(
    _table_name character varying
)
RETURNS character varying
STABLE
AS
$$
BEGIN
    RETURN form_name 
    FROM config.custom_field_forms
    WHERE table_name = _table_name;
END
$$
LANGUAGE plpgsql;


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/1.x/1.0/src/06.functions-and-logic/config.create_app.sql --<--<--
DROP FUNCTION IF EXISTS config.create_app
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

CREATE FUNCTION config.create_app
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
        FROM config.apps
        WHERE LOWER(config.apps.app_name) = LOWER(_app_name)
    ) THEN
        UPDATE config.apps
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
        INSERT INTO config.apps(app_name, name, version_number, publisher, published_on, icon, landing_url)
        SELECT _app_name, _name, _version_number, _publisher, _published_on, _icon, _landing_url;
    END IF;

    DELETE FROM config.app_dependencies
    WHERE app_name = _app_name;

    INSERT INTO config.app_dependencies(app_name, depends_on)
    SELECT _app_name, UNNEST(_dependencies);
END
$$
LANGUAGE plpgsql;


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/1.x/1.0/src/06.functions-and-logic/config.create_menu.sql --<--<--
DROP FUNCTION IF EXISTS config.create_menu
(
    _sort                                       integer,
    _app_name                                   text,
    _menu_name                                  text,
    _url                                        text,
    _icon                                       text,
    _parent_menu_id                             integer    
);

DROP FUNCTION IF EXISTS config.create_menu
(
    _sort                                       integer,
    _app_name                                   text,
    _menu_name                                  text,
    _url                                        text,
    _icon                                       text,
    _parent_menu_name                           text
);

CREATE FUNCTION config.create_menu
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
       FROM config.menus
       WHERE LOWER(app_name) = LOWER(_app_name)
       AND LOWER(menu_name) = LOWER(_menu_name)
    ) THEN
        UPDATE config.menus
        SET
            sort = _sort,
            url = _url,
            icon = _icon,
            parent_menu_id = _parent_menu_id
       WHERE LOWER(app_name) = LOWER(_app_name)
       AND LOWER(menu_name) = LOWER(_menu_name)
       RETURNING menu_id INTO _menu_id;        
    ELSE
        INSERT INTO config.menus(sort, app_name, menu_name, url, icon, parent_menu_id)
        SELECT _sort, _app_name, _menu_name, _url, _icon, _parent_menu_id
        RETURNING menu_id INTO _menu_id;        
    END IF;

    RETURN _menu_id;
END
$$
LANGUAGE plpgsql;


CREATE FUNCTION config.create_menu
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
    FROM config.menus
    WHERE LOWER(menu_name) = LOWER(_parent_menu_name)
    AND LOWER(app_name) = LOWER(_app_name);

    RETURN config.create_menu(_sort, _app_name, _menu_name, _url, _icon, _parent_menu_id);
END
$$
LANGUAGE plpgsql;


DROP FUNCTION IF EXISTS config.create_menu
(
    _app_name                                   text,
    _menu_name                                  text,
    _url                                        text,
    _icon                                       text,
    _parent_menu_name                           text    
);

CREATE FUNCTION config.create_menu
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
    RETURN config.create_menu(0, _app_name, _menu_name, _url, _icon, _parent_menu_name);
END
$$
LANGUAGE plpgsql;

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/1.x/1.0/src/99.ownership.sql --<--<--
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

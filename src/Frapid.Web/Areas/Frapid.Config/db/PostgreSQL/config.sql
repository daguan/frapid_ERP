-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/01.types-domains-tables-and-constraints/tables-and-constraints.sql --<--<--
DROP SCHEMA IF EXISTS config CASCADE;
CREATE SCHEMA config;

CREATE TABLE config.kanbans
(
    kanban_id                                   BIGSERIAL NOT NULL PRIMARY KEY,
    object_name                                 national character varying(128) NOT NULL,
    user_id                                     integer REFERENCES account.users,
    kanban_name                                 national character varying(128) NOT NULL,
    description                                 text,
    audit_user_id                               integer REFERENCES account.users,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())    
);
CREATE TABLE config.kanban_details
(
    kanban_detail_id                            BIGSERIAL NOT NULL PRIMARY KEY,
    kanban_id                                   bigint NOT NULL REFERENCES config.kanbans(kanban_id),
    rating                                      smallint CHECK(rating>=0 AND rating<=5),
    resource_id                                 national character varying(128) NOT NULL,
    audit_user_id                               integer NULL REFERENCES account.users,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())    
);

CREATE UNIQUE INDEX kanban_details_kanban_id_resource_id_uix
ON config.kanban_details(kanban_id, resource_id);


CREATE TABLE config.smtp_configs
(
    smtp_config_id                              SERIAL PRIMARY KEY,
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
    audit_user_id                               integer REFERENCES account.users,
    audit_ts                                    timestamp with time zone DEFAULT now()
);


CREATE TABLE config.email_queue
(
    queue_id                                    BIGSERIAL NOT NULL PRIMARY KEY,
    from_name                                   national character varying(256) NOT NULL,
    from_email                                  national character varying(256) NOT NULL,
    reply_to                                    national character varying(256) NOT NULL,
    reply_to_name                               national character varying(256) NOT NULL,
    subject                                     national character varying(256) NOT NULL,
    send_to                                     national character varying(256) NOT NULL,
    attachments                                 text,
    message                                     text NOT NULL,
    added_on                                    TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT(NOW()),
	send_on										TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT(NOW()),
    delivered                                   boolean NOT NULL DEFAULT(false),
    delivered_on                                TIMESTAMP WITH TIME ZONE,
    canceled                                    boolean NOT NULL DEFAULT(false),
    canceled_on                                 TIMESTAMP WITH TIME ZONE,
	is_test										boolean NOT NULL DEFAULT(false)
);


CREATE TABLE config.filters
(
    filter_id                                   BIGSERIAL NOT NULL PRIMARY KEY,
    object_name                                 text NOT NULL,
    filter_name                                 text NOT NULL,
    is_default                                  boolean NOT NULL DEFAULT(false),
    is_default_admin                            boolean NOT NULL DEFAULT(false),
    filter_statement                            national character varying(12) NOT NULL DEFAULT('WHERE'),
    column_name                                 text NOT NULL,
	data_type									text NOT NULL DEFAULT(''),
    filter_condition                            integer NOT NULL,
    filter_value                                text,
    filter_and_value                            text,
    audit_user_id                               integer REFERENCES account.users,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())
);

CREATE INDEX filters_object_name_inx
ON config.filters(object_name);

CREATE TABLE config.custom_field_data_types
(
    data_type                                   national character varying(50) NOT NULL PRIMARY KEY,
    is_number                                   boolean DEFAULT(false),
    is_date                                     boolean DEFAULT(false),
    is_boolean                                  boolean DEFAULT(false),
    is_long_text                                boolean DEFAULT(false)
);

CREATE TABLE config.custom_field_forms
(
    form_name                                   national character varying(100) NOT NULL PRIMARY KEY,
    table_name                                  national character varying(100) NOT NULL UNIQUE,
    key_name                                    national character varying(100) NOT NULL        
);


CREATE TABLE config.custom_field_setup
(
    custom_field_setup_id                       SERIAL NOT NULL PRIMARY KEY,
    form_name                                   national character varying(100) NOT NULL
                                                REFERENCES config.custom_field_forms,
    field_order                                 integer NOT NULL DEFAULT(0),
    field_name                                  national character varying(100) NOT NULL,
    field_label                                 national character varying(100) NOT NULL,                   
    data_type                                   national character varying(50)
                                                REFERENCES config.custom_field_data_types,
    description                                 text NOT NULL
);


CREATE TABLE config.custom_fields
(
    custom_field_id                             BIGSERIAL NOT NULL PRIMARY KEY,
    custom_field_setup_id                       integer NOT NULL REFERENCES config.custom_field_setup,
    resource_id                                 text NOT NULL,
    value                                       text
);


CREATE TABLE config.flag_types
(
    flag_type_id                                SERIAL PRIMARY KEY,
    flag_type_name                              national character varying(24) NOT NULL,
    background_color                            color NOT NULL,
    foreground_color                            color NOT NULL,
    audit_user_id                               integer NULL REFERENCES account.users,
    audit_ts                                    TIMESTAMP WITH TIME ZONE NULL
                                                DEFAULT(NOW())
);

COMMENT ON TABLE config.flag_types IS 'Flags are used by users to mark transactions. The flags created by a user is not visible to others.';

CREATE TABLE config.flags
(
    flag_id                                     BIGSERIAL PRIMARY KEY,
    user_id                                     integer NOT NULL REFERENCES account.users,
    flag_type_id                                integer NOT NULL REFERENCES config.flag_types(flag_type_id),
    resource                                    text, --Fully qualified resource name. Example: transactions.non_gl_stock_master.
    resource_key                                text, --The unique identifier for lookup. Example: non_gl_stock_master_id,
    resource_id                                 text, --The value of the unique identifier to lookup for,
    flagged_on                                  TIMESTAMP WITH TIME ZONE NULL 
                                                DEFAULT(NOW())
);

CREATE UNIQUE INDEX flags_user_id_resource_resource_id_uix
ON config.flags(user_id, UPPER(resource), UPPER(resource_key), UPPER(resource_id));

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/04.default-values/01.default-values.sql --<--<--
DO
$$
BEGIN
    IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='Text') THEN
        INSERT INTO config.custom_field_data_types(data_type, is_number)
        SELECT 'Text', false;
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


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/05.views/config.custom_field_definition_view.sql --<--<--
DROP VIEW IF EXISTS config.custom_field_definition_view;

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

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/05.views/config.custom_field_view.sql --<--<--
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

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/05.views/config.filter_name_view.sql --<--<--
DROP VIEW IF EXISTS config.filter_name_view;

CREATE VIEW config.filter_name_view
AS
SELECT
    DISTINCT
    object_name,
    filter_name,
    is_default
FROM config.filters;

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/05.views/config.flag_view.sql --<--<--
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

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/06.functions-and-logic/config.create_flag.sql --<--<--
DROP FUNCTION IF EXISTS config.create_flag
(
    user_id_            integer,
    flag_type_id_       integer,
    resource_           text,
    resource_key_       text,
    resource_id_        text
);

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



-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/06.functions-and-logic/config.get_custom_field_definition.sql --<--<--
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

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/06.functions-and-logic/config.get_custom_field_form_name.sql --<--<--
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

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/06.functions-and-logic/config.get_custom_field_setup_id_by_table_name.sql --<--<--
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


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/06.functions-and-logic/config.get_flag_type_id.sql --<--<--
DROP FUNCTION IF EXISTS config.get_flag_type_id
(
    user_id_        integer,
    resource_       text,
    resource_key_   text,
    resource_id_    text
);

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


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/06.functions-and-logic/config.get_user_id_by_login_id.sql --<--<--
DROP FUNCTION IF EXISTS config.get_user_id_by_login_id(_login_id bigint);

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


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/09.menus/0.menu.sql --<--<--
SELECT * FROM core.create_app('Frapid.Config', 'Config', '1.0', 'MixERP Inc.', 'December 1, 2015', 'orange configure', '/dashboard/config/offices', null);
SELECT * FROM core.create_menu('Frapid.Config', 'Offices', '/dashboard/config/offices', 'building outline', '');
SELECT * FROM core.create_menu('Frapid.Config', 'Flags', '/dashboard/config/flags', 'flag', '');
SELECT * FROM core.create_menu('Frapid.Config', 'SMTP', '/dashboard/config/smtp', 'at', '');
SELECT * FROM core.create_menu('Frapid.Config', 'File Manager', '/dashboard/config/file-manager', 'file text outline', '');

SELECT * FROM auth.create_app_menu_policy
(
    'Admin', 
    core.get_office_id_by_office_name('Default'), 
    'Frapid.Config',
    '{*}'::text[]
);

-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/09.menus/1.menu-policy.sql --<--<--
SELECT * FROM auth.create_app_menu_policy
(
    'User', 
    core.get_office_id_by_office_name('Default'), 
    'Frapid.Config',
    '{Offices, Flags}'::text[]
);

SELECT * FROM auth.create_app_menu_policy
(
    'Admin', 
    core.get_office_id_by_office_name('Default'), 
    'Frapid.Config',
    '{*}'::text[]
);


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/10.policy/access_policy.sql --<--<--
SELECT * FROM auth.create_api_access_policy('{*}', 1, 'config.kanban_details', '{*}', true);
SELECT * FROM auth.create_api_access_policy('{*}', 1, 'config.flag_types', '{*}', true);
SELECT * FROM auth.create_api_access_policy('{*}', 1, 'config.flag_view', '{*}', true);
SELECT * FROM auth.create_api_access_policy('{*}', 1, 'config.kanbans', '{*}', true);
SELECT * FROM auth.create_api_access_policy('{*}', 1, 'config.filter_name_view', '{*}', true);

SELECT * FROM auth.create_api_access_policy('{User}', core.get_office_id_by_office_name('Default'), 'core.offices', '{*}', true);
SELECT * FROM auth.create_api_access_policy('{User}', core.get_office_id_by_office_name('Default'), 'config.flags', '{*}', true);
SELECT * FROM auth.create_api_access_policy('{Admin}', core.get_office_id_by_office_name('Default'), '', '{*}', true);


-->-->-- C:/Users/nirvan/Desktop/mixerp/frapid/src/Frapid.Web/Areas/Frapid.Config/db/PostgreSQL/1.x/1.0/src/99.ownership.sql --<--<--
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

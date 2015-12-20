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
    logo                                    public.image,
    parent_office_id                        integer NULL REFERENCES config.offices,
    audit_user_id                           integer NULL,
    audit_ts                                TIMESTAMP WITH TIME ZONE NULL 
                                            DEFAULT(NOW())
);

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
EXECUTE dbo.drop_schema 'config';
GO
CREATE SCHEMA config;

GO

CREATE TABLE config.kanbans
(
    kanban_id                                   bigint IDENTITY NOT NULL PRIMARY KEY,
    object_name                                 national character varying(128) NOT NULL,
    user_id                                     integer REFERENCES account.users,
    kanban_name                                 national character varying(128) NOT NULL,
    description                                 national character varying(500),
    audit_user_id                               integer REFERENCES account.users,
    audit_ts                                    datetimeoffset NULL 
                                                DEFAULT(getutcdate())    
);
CREATE TABLE config.kanban_details
(
    kanban_detail_id                            bigint IDENTITY NOT NULL PRIMARY KEY,
    kanban_id                                   bigint NOT NULL REFERENCES config.kanbans(kanban_id),
    rating                                      smallint CHECK(rating>=0 AND rating<=5),
    resource_id                                 national character varying(128) NOT NULL,
    audit_user_id                               integer NULL REFERENCES account.users,
    audit_ts                                    datetimeoffset NULL 
                                                DEFAULT(getutcdate())    
);

CREATE UNIQUE INDEX kanban_details_kanban_id_resource_id_uix
ON config.kanban_details(kanban_id, resource_id);


CREATE TABLE config.smtp_configs
(
    smtp_config_id                              integer IDENTITY PRIMARY KEY,
    configuration_name                          national character varying(256) NOT NULL UNIQUE,
    enabled                                     bit NOT NULL DEFAULT 0,
    is_default                                  bit NOT NULL DEFAULT 0,
    from_display_name                           national character varying(256) NOT NULL,
    from_email_address                          national character varying(256) NOT NULL,
    smtp_host                                   national character varying(256) NOT NULL,
    smtp_enable_ssl                             bit NOT NULL DEFAULT 1,
    smtp_username                               national character varying(256) NOT NULL,
    smtp_password                               national character varying(256) NOT NULL,
    smtp_port                                   integer NOT NULL DEFAULT(587),
    audit_user_id                               integer REFERENCES account.users,
    audit_ts                                    datetimeoffset DEFAULT getutcdate()
);


CREATE TABLE config.email_queue
(
    queue_id                                    bigint IDENTITY NOT NULL PRIMARY KEY,
    from_name                                   national character varying(256) NOT NULL,
    from_email                                  national character varying(256) NOT NULL,
    reply_to                                    national character varying(256) NOT NULL,
    reply_to_name                               national character varying(256) NOT NULL,
    subject                                     national character varying(256) NOT NULL,
    send_to                                     national character varying(256) NOT NULL,
    attachments                                 national character varying(2000),
    message                                     national character varying(MAX) NOT NULL,
    added_on                                    datetimeoffset NOT NULL DEFAULT(getutcdate()),
	send_on										datetimeoffset NOT NULL DEFAULT(getutcdate()),
    delivered                                   bit NOT NULL DEFAULT(0),
    delivered_on                                datetimeoffset,
    canceled                                    bit NOT NULL DEFAULT(0),
    canceled_on                                 datetimeoffset,
	is_test										bit NOT NULL DEFAULT(0)
);


CREATE TABLE config.filters
(
    filter_id                                   bigint IDENTITY NOT NULL PRIMARY KEY,
    object_name                                 national character varying(500) NOT NULL,
    filter_name                                 national character varying(500) NOT NULL,
    is_default                                  bit NOT NULL DEFAULT(0),
    is_default_admin                            bit NOT NULL DEFAULT(0),
    filter_statement                            national character varying(12) NOT NULL DEFAULT('WHERE'),
    column_name                                 national character varying(500) NOT NULL,
	data_type									national character varying(500) NOT NULL DEFAULT(''),
    filter_condition                            integer NOT NULL,
    filter_value                                national character varying(500),
    filter_and_value                            national character varying(500),
    audit_user_id                               integer REFERENCES account.users,
    audit_ts                                    datetimeoffset NULL 
                                                DEFAULT(getutcdate())
);

CREATE INDEX filters_object_name_inx
ON config.filters(object_name);

CREATE TABLE config.custom_field_data_types
(
    data_type                                   national character varying(50) NOT NULL PRIMARY KEY,
    is_number                                   bit DEFAULT(0),
    is_date                                     bit DEFAULT(0),
    is_bit										bit DEFAULT(0),
    is_long_text								bit DEFAULT(0)
);

CREATE TABLE config.custom_field_forms
(
    form_name                                   national character varying(100) NOT NULL PRIMARY KEY,
    table_name                                  national character varying(100) NOT NULL UNIQUE,
    key_name                                    national character varying(100) NOT NULL        
);


CREATE TABLE config.custom_field_setup
(
    custom_field_setup_id                       integer IDENTITY NOT NULL PRIMARY KEY,
    form_name                                   national character varying(100) NOT NULL
                                                REFERENCES config.custom_field_forms,
    field_order                                 integer NOT NULL DEFAULT(0),
    field_name                                  national character varying(100) NOT NULL,
    field_label                                 national character varying(100) NOT NULL,                   
    data_type                                   national character varying(50)
                                                REFERENCES config.custom_field_data_types,
    description                                 national character varying(500) NOT NULL
);


CREATE TABLE config.custom_fields
(
    custom_field_id                             bigint IDENTITY NOT NULL PRIMARY KEY,
    custom_field_setup_id                       integer NOT NULL REFERENCES config.custom_field_setup,
    resource_id                                 national character varying(500) NOT NULL,
    value                                       national character varying(500)
);


CREATE TABLE config.flag_types
(
    flag_type_id                                integer IDENTITY PRIMARY KEY,
    flag_type_name                              national character varying(24) NOT NULL,
    background_color                            color NOT NULL,
    foreground_color                            color NOT NULL,
    audit_user_id                               integer NULL REFERENCES account.users,
    audit_ts                                    datetimeoffset NULL
                                                DEFAULT(getutcdate())
);


CREATE TABLE config.flags
(
    flag_id                                     bigint IDENTITY PRIMARY KEY,
    user_id                                     integer NOT NULL REFERENCES account.users,
    flag_type_id                                integer NOT NULL REFERENCES config.flag_types(flag_type_id),
    resource                                    national character varying(500), --Fully qualified resource name. Example: transactions.non_gl_stock_master.
    resource_key                                national character varying(500), --The unique identifier for lookup. Example: non_gl_stock_master_id,
    resource_id                                 national character varying(500), --The value of the unique identifier to lookup for,
    flagged_on                                  datetimeoffset NULL 
                                                DEFAULT(getutcdate())
);

CREATE UNIQUE INDEX flags_user_id_resource_resource_id_uix
ON config.flags(user_id, resource, resource_key, resource_id);


GO

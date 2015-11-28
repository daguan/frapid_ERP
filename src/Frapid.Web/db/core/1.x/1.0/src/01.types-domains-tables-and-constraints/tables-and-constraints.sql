DROP SCHEMA IF EXISTS core CASCADE;
CREATE SCHEMA core;

CREATE TABLE core.currencies
(
    currency_code                           national character varying(12) PRIMARY KEY,
    currency_symbol                         national character varying(12) NOT NULL,
    currency_name                           national character varying(48) NOT NULL UNIQUE,
    hundredth_name                          national character varying(48) NOT NULL,
    audit_user_id                           integer,
    audit_ts                                TIMESTAMP WITH TIME ZONE NULL 
                                            DEFAULT(NOW())
);

CREATE TABLE core.offices
(
    office_id                               SERIAL PRIMARY KEY,
    office_code                             national character varying(12) NOT NULL,
    office_name                             national character varying(150) NOT NULL,
    nick_name                               national character varying(50),
    registration_date                       date,
    currency_code                           national character varying(12) REFERENCES core.currencies,
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
    parent_office_id                        integer NULL REFERENCES core.offices,
    audit_user_id                           integer NULL,
    audit_ts                                TIMESTAMP WITH TIME ZONE NULL 
                                            DEFAULT(NOW())
);

INSERT INTO core.offices(office_code, office_name)
SELECT 'DEF', 'Default';

CREATE TABLE core.smtp_configs
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


CREATE TABLE core.email_queue
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

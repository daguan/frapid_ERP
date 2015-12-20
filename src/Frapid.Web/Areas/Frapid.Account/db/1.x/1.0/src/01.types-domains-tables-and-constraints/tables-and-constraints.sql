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
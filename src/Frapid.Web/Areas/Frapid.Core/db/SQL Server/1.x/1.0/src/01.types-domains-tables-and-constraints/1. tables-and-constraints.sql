GO

CREATE TABLE core.apps
(
    app_name                                    national character varying(100) PRIMARY KEY,
    name                                        national character varying(100),
    version_number                              national character varying(100),
    publisher                                   national character varying(500),
    published_on                                date,
    icon                                        national character varying(100),
    landing_url                                 national character varying(500)
);

CREATE UNIQUE INDEX apps_app_name_uix
ON core.apps(app_name);

CREATE TABLE core.app_dependencies
(
    app_dependency_id                           int IDENTITY PRIMARY KEY,
    app_name                                    national character varying(100) REFERENCES core.apps,
    depends_on                                  national character varying(100) REFERENCES core.apps
);


CREATE TABLE core.menus
(
    menu_id                                     int IDENTITY PRIMARY KEY,
    sort                                        integer,
    app_name                                    national character varying(100) NOT NULL REFERENCES core.apps,
    menu_name                                   national character varying(100) NOT NULL,
    url                                         national character varying(500),
    icon                                        national character varying(100),
    parent_menu_id                              integer REFERENCES core.menus
);

CREATE UNIQUE INDEX menus_app_name_menu_name_uix
ON core.menus(app_name, menu_name);

CREATE TABLE core.menu_locale
(
    menu_locale_id                              int IDENTITY PRIMARY KEY,
    menu_id                                     integer NOT NULL REFERENCES core.menus,
    culture                                     national character varying(12) NOT NULL,
    menu_text                                   national character varying(250) NOT NULL
);

CREATE TABLE core.offices
(
    office_id                                   int IDENTITY PRIMARY KEY,
    office_code                                 national character varying(12) NOT NULL,
    office_name                                 national character varying(150) NOT NULL,
    nick_name                                   national character varying(50),
    registration_date                           date,
	currency_code								national character varying(12),
    po_box                                      national character varying(128),
    address_line_1                              national character varying(128),   
    address_line_2                              national character varying(128),
    street                                      national character varying(50),
    city                                        national character varying(50),
    state                                       national character varying(50),
    zip_code                                    national character varying(24),
    country                                     national character varying(50),
    phone                                       national character varying(24),
    fax                                         national character varying(24),
    email                                       national character varying(128),
    url                                         national character varying(50),
    logo                                        dbo.photo,
    parent_office_id                            integer NULL REFERENCES core.offices,
    audit_user_id                               integer NULL,
    audit_ts                                    datetimeoffset NULL DEFAULT(getutcdate())
);

CREATE TABLE core.frequencies
(
    frequency_id								int IDENTITY PRIMARY KEY,
    frequency_code								national character varying(12) NOT NULL,
    frequency_name								national character varying(50) NOT NULL
);


CREATE UNIQUE INDEX frequencies_frequency_code_uix
ON core.frequencies(frequency_code);

CREATE UNIQUE INDEX frequencies_frequency_name_uix
ON core.frequencies(frequency_name);


CREATE TABLE core.verification_statuses
(
    verification_status_id						smallint PRIMARY KEY,
    verification_status_name					national character varying(128) NOT NULL
);


CREATE TABLE core.week_days
(
	week_day_id									integer NOT NULL CHECK(week_day_id > =1 AND week_day_id < =7) PRIMARY KEY,
	week_day_code								national character varying(12) NOT NULL UNIQUE,
	week_day_name								national character varying(50) NOT NULL UNIQUE
);

CREATE TABLE core.genders
(
	gender_code									national character varying(4) NOT NULL PRIMARY KEY,
	gender_name									national character varying(50) NOT NULL UNIQUE,
	audit_user_id								integer NULL,
	audit_ts									datetimeoffset NULL DEFAULT(getutcdate())    
);

CREATE TABLE core.marital_statuses
(
	marital_status_id							int IDENTITY NOT NULL PRIMARY KEY,
	marital_status_code							national character varying(12) NOT NULL UNIQUE,
	marital_status_name							national character varying(128) NOT NULL,
	is_legally_recognized_marriage				bit NOT NULL DEFAULT(0),
	audit_user_id								integer NULL,    
	audit_ts									datetimeoffset NULL DEFAULT(getutcdate())
);

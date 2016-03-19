CREATE SCHEMA i18n;

CREATE TABLE i18n.resources
(
  resource_id                                   int IDENTITY PRIMARY KEY,
  resource_class                                national character varying(4000) UNIQUE,
  [key]                                         national character varying(4000),
  value                                         national character varying(MAX)
);

CREATE TABLE i18n.localized_resources
(
    localized_resource_id                       bigint IDENTITY PRIMARY KEY,
    resource_id                                 integer NOT NULL REFERENCES i18n.resources,
    culture_code                                national character varying(5) NOT NULL,
    value                                       national character varying(4000)
);
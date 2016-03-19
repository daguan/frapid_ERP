DROP SCHEMA IF EXISTS i18n CASCADE;
CREATE SCHEMA i18n;

CREATE TABLE i18n.resources
(
  resource_id                                   SERIAL PRIMARY KEY,
  resource_class                                text,
  key                                           text,
  value                                         text
);

CREATE UNIQUE INDEX resource_class_key_uix
ON i18n.resources(LOWER(resource_class), LOWER(key));

CREATE TABLE i18n.localized_resources
(
    localized_resource_id                       BIGSERIAL PRIMARY KEY,
    resource_id                                 integer NOT NULL REFERENCES i18n.resources,
    culture_code                                national character varying(5) NOT NULL,
    value                                       text
);
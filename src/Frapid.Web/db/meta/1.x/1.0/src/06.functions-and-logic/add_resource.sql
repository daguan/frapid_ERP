DROP FUNCTION IF EXISTS i18n.add_resource
(
    resource_class  text,
    key             text,
    value           text
);

CREATE OR REPLACE FUNCTION i18n.add_resource
(
    resource_class  text,
    key             text,
    value           text
)
RETURNS void 
VOLATILE
AS
$$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM i18n.resources WHERE i18n.resources.resource_class=$1 AND i18n.resources.key=$2) THEN
        INSERT INTO i18n.resources(resource_class, key, value)
        SELECT $1, $2, $3;
    END IF;
END
$$
LANGUAGE plpgsql;

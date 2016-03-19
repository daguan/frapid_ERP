DROP FUNCTION IF EXISTS i18n.add_localized_resource
(
    _resource_class  text,
    _culture_code    text,
    _key             text,
    _value           text
);

CREATE FUNCTION i18n.add_localized_resource
(
    _resource_class  text,
    _culture_code    text,
    _key             text,
    _value           text
)
RETURNS void 
VOLATILE
AS
$$
    DECLARE _resource_id    integer;
BEGIN
    IF(COALESCE(_culture_code, '') = '') THEN
        PERFORM i18n.add_resource(_resource_class, _key, _value);
        RETURN;
    END IF;
       
    SELECT resource_id INTO _resource_id
    FROM i18n.resources
    WHERE UPPER(resource_class) = UPPER(_resource_class)
    AND UPPER(key) = UPPER(_key);

    IF(_resource_id IS NOT NULL) THEN
        IF EXISTS
        (
            SELECT 1 FROM i18n.localized_resources 
            WHERE i18n.localized_resources.resource_id=_resource_id
            AND culture_code = _culture_code
        ) THEN
            UPDATE i18n.localized_resources
            SET value=_value
            WHERE i18n.localized_resources.resource_id=_resource_id
            AND culture_code = _culture_code;

            RETURN;
        END IF;

        INSERT INTO i18n.localized_resources(resource_id, culture_code, value)
        SELECT _resource_id, _culture_code, _value;
    END IF;
END
$$
LANGUAGE plpgsql;
DROP FUNCTION IF EXISTS i18n.get_output_for(national character varying(3));

CREATE FUNCTION i18n.get_output_for(national character varying(3))
RETURNS text
AS
$$
BEGIN
    RETURN array_to_string(array_agg(i18n.resource), E'\n')
    FROM
    (
        SELECT
            
            'SELECT * FROM i18n.add_localized_resource(''' ||
            i18n.resources.resource_class || ''', ''' || $1 || ''', ''' ||
            i18n.resources.key || ''', ''' ||
            REPLACE(i18n.localized_resources.value, '''', '''''') || ''');--' ||
            i18n.resources.value AS resource
        FROM i18n.localized_resources
        LEFT JOIN i18n.resources
        ON i18n.localized_resources.resource_id = i18n.resources.resource_id
        WHERE culture_code = $1
        ORDER BY i18n.resources.resource_class, i18n.resources.key
    )
    i18n;
END
$$
LANGUAGE plpgsql;

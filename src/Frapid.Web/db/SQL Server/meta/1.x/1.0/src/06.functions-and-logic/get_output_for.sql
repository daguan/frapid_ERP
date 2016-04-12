IF OBJECT_ID('i18n.get_output_for') IS NOT NULL
DROP FUNCTION i18n.get_output_for;

GO

CREATE FUNCTION i18n.get_output_for(@culture national character varying(3))
RETURNS national character varying(MAX)
AS
BEGIN
	DECLARE @resources national character varying(MAX) = '';
	
    SELECT
        @resources = COALESCE(@resources + CHAR(13), '') +
        'SELECT * FROM i18n.add_localized_resource(''' +
        i18n.resources.resource_class + ''', ''' + @culture + ''', ''' +
        i18n.resources.[key] + ''', ''' +
        REPLACE(i18n.localized_resources.value, '''', '''''') + ''');--' +
        i18n.resources.value
    FROM i18n.localized_resources
    LEFT JOIN i18n.resources
    ON i18n.localized_resources.resource_id = i18n.resources.resource_id
    WHERE culture_code = @culture
    ORDER BY i18n.resources.resource_class, i18n.resources.[key];
    
    RETURN @resources;
END;

GO

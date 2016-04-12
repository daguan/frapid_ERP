IF OBJECT_ID('i18n.get_localization_table') IS NOT NULL
DROP FUNCTION i18n.get_localization_table;

GO

CREATE FUNCTION i18n.get_localization_table
(
    @culture			national character varying(4000)
)
RETURNS @result TABLE
(
    id                  bigint,
    resource_class      national character varying(4000),
    [key]               national character varying(4000),
    original            national character varying(4000),
    translated          national character varying(4000)
)
AS
BEGIN       
    INSERT INTO @result
    SELECT
        i18n.resources.resource_id, 
        i18n.resources.resource_class, 
        i18n.resources.[key], 
        i18n.resources.value,
        ''
    FROM i18n.resources;
	
	RETURN;
END;

GO

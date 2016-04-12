IF OBJECT_ID('i18n.get_resource') IS NOT NULL
DROP FUNCTION i18n.get_resource;

GO

CREATE FUNCTION i18n.get_resource
(
	@culture_code national character varying(4000), 
	@resource_class national character varying(4000), 
	@key national character varying(4000)
)
RETURNS national character varying(4000)
AS
BEGIN
    DECLARE @resource_id    integer;
    DECLARE @resource       national character varying(4000);
    DECLARE @value          national character varying(4000);

    SELECT 
        @resource_id = resource_id,
        @resource = value
    FROM i18n.resources
    WHERE resource_class = @resource_class
    AND [key] = @key;

    SELECT
        @value = value
    FROM i18n.localized_resources
    WHERE culture_code = @culture_code
    AND resource_id = @resource_id;


    SET @resource = COALESCE(@value, @resource);
    
    RETURN @resource;
END;

GO

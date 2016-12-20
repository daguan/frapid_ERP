IF OBJECT_ID('i18n.add_resource') IS NOT NULL
DROP PROCEDURE i18n.add_resource;

GO


CREATE PROCEDURE i18n.add_resource
(
    @resource_class		national character varying(4000),
    @key				national character varying(4000),
    @value				national character varying(4000)
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS
    (
		SELECT 1 
		FROM i18n.resources 
		WHERE i18n.resources.resource_class=@resource_class 
		AND i18n.resources.[key]=@key
	)
	BEGIN
        INSERT INTO i18n.resources(resource_class, [key], value)
        SELECT @resource_class, @key, @value;
    END;
END;


GO
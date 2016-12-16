IF OBJECT_ID('i18n.add_localized_resource') IS NOT NULL
DROP PROCEDURE i18n.add_localized_resource;

GO

CREATE PROCEDURE i18n.add_localized_resource
(
    @resource_class		national character varying(4000),
    @culture_code		national character varying(4000),
    @key				national character varying(4000),
    @value				national character varying(4000)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @resource_id    integer;

    IF(COALESCE(@culture_code, '') = '')
    BEGIN
        EXECUTE i18n.add_resource @resource_class, @key, @value;
        RETURN;
    END;
       
    SELECT @resource_id = resource_id
    FROM i18n.resources
    WHERE resource_class = @resource_class
    AND [key] = @key;

    IF(@resource_id IS NOT NULL)
    BEGIN
        IF EXISTS
        (
            SELECT 1 FROM i18n.localized_resources 
            WHERE i18n.localized_resources.resource_id=@resource_id
            AND culture_code = @culture_code
        )
        BEGIN
            UPDATE i18n.localized_resources
            SET value=@value
            WHERE i18n.localized_resources.resource_id=@resource_id
            AND culture_code = @culture_code;

            RETURN;
        END;

        INSERT INTO i18n.localized_resources(resource_id, culture_code, value)
        SELECT @resource_id, @culture_code, @value;
    END;
END;

GO
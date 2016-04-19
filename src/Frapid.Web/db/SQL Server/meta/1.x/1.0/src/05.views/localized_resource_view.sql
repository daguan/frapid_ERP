IF OBJECT_ID('i18n.localized_resource_view') IS NOT NULL
DROP VIEW i18n.localized_resource_view;

GO

CREATE VIEW i18n.localized_resource_view
AS
SELECT
    resource_class + 
    CASE WHEN COALESCE(culture, '') = '' THEN '' ELSE '.' + culture END 
    + '.' + [key] as [key], value 
FROM i18n.resource_view;


GO

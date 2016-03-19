IF EXISTS(SELECT * FROM sys.views WHERE object_id = OBJECT_ID('i18n.resource_view'))
DROP VIEW i18n.resource_view;

GO

CREATE VIEW i18n.resource_view
AS
SELECT 
    resource_class, '' as culture, [key], value
FROM i18n.resources
UNION ALL
SELECT resource_class, culture_code, [key], i18n.localized_resources.value 
FROM i18n.localized_resources
INNER JOIN i18n.resources
ON i18n.localized_resources.resource_id = i18n.resources.resource_id;
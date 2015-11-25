DROP VIEW IF EXISTS i18n.localized_resource_view;

CREATE VIEW i18n.localized_resource_view
AS
SELECT
    resource_class || 
    CASE WHEN COALESCE(culture, '') = '' THEN '' ELSE '.' || culture END 
    || '.' || key as key, value 
FROM i18n.resource_view;

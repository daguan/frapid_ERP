IF OBJECT_ID('config.flag_view') IS NOT NULL
DROP VIEW config.flag_view;
GO
CREATE VIEW config.flag_view
AS
SELECT
    config.flags.flag_id,
    config.flags.user_id,
    config.flags.flag_type_id,
    config.flags.resource_id,
    config.flags.resource,
    config.flags.resource_key,
    config.flags.flagged_on,
    config.flag_types.flag_type_name,
    config.flag_types.background_color,
    config.flag_types.foreground_color
FROM config.flags
INNER JOIN config.flag_types
ON config.flags.flag_type_id = config.flag_types.flag_type_id;


GO

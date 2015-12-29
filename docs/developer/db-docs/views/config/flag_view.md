# config.flag_view view

| Schema | [config](../../schemas/config.md) |
| --- | --- |
| Materialized View Name | flag_view |
| Owner | frapid_db_user |
| Tablespace | DEFAULT |
| Description |  |

**Source:**

```plpgsql
 CREATE OR REPLACE VIEW config.flag_view
 AS
 SELECT flags.flag_id,
    flags.user_id,
    flags.flag_type_id,
    flags.resource_id,
    flags.resource,
    flags.resource_key,
    flags.flagged_on,
    flag_types.flag_type_name,
    flag_types.background_color,
    flag_types.foreground_color
   FROM config.flags
     JOIN config.flag_types ON flags.flag_type_id = flag_types.flag_type_id;
```


### Related Contents
* [Schema List](../../schemas.md)
* [View List](../../views.md)
* [Materialized View List](../../materialized-views.md)
* [Table of Contents](../../README.md)


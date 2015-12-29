# core.menu_locale table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | menu_locale_id | [ ] | integer | 0 |  |
| 2 | menu_id | [ ] | integer | 0 |  |
| 3 | culture | [ ] | character varying | 12 |  |
| 4 | menu_text | [ ] | character varying | 250 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 2 | [menu_id](../core/menus.md) | menu_locale_menu_id_fkey | core.menus.menu_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| menu_locale_pkey | frapid_db_user | btree | menu_locale_id |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | menu_locale_id | nextval('core.menu_locale_menu_locale_id_seq'::regclass) |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

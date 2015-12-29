# core.menus table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | menu_id | NOT NULL | integer | 0 |  |
| 1 | menu_id | NOT NULL | integer | 0 |  |
| 2 | sort |  | integer | 0 |  |
| 2 | menu_name |  | character varying | 100 |  |
| 3 | description |  | text | 0 |  |
| 3 | app_name | NOT NULL | character varying | 100 |  |
| 4 | audit_user_id |  | integer | 0 |  |
| 4 | menu_name | NOT NULL | character varying | 100 |  |
| 5 | audit_ts |  | timestamp with time zone | 0 |  |
| 5 | url |  | text | 0 |  |
| 6 | icon |  | character varying | 100 |  |
| 7 | parent_menu_id |  | integer | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 3 | [app_name](../core/apps.md) | menus_app_name_fkey | core.apps.app_name |
| 7 | [parent_menu_id](../core/menus.md) | menus_parent_menu_id_fkey | core.menus.menu_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| menus_pkey | frapid_db_user | btree | menu_id |  |
| menus_app_name_menu_name_uix | frapid_db_user | btree | upper(app_name::text), upper(menu_name::text) |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | menu_id | nextval('core.menus_menu_id_seq'::regclass) |
| 1 | menu_id | nextval('website.menus_menu_id_seq'::regclass) |
| 5 | audit_ts | now() |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

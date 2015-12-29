# website.menu_items table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | menu_item_id | NOT NULL | integer | 0 |  |
| 2 | menu_id |  | integer | 0 |  |
| 3 | sort | NOT NULL | integer | 0 |  |
| 4 | title | NOT NULL | character varying | 100 |  |
| 5 | url |  | character varying | 500 |  |
| 6 | content_id |  | integer | 0 |  |
| 7 | audit_user_id |  | integer | 0 |  |
| 8 | audit_ts |  | timestamp with time zone | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 2 | [menu_id](../website/menus.md) | menu_items_menu_id_fkey | website.menus.menu_id |
| 6 | [content_id](../website/contents.md) | menu_items_content_id_fkey | website.contents.content_id |
| 7 | [audit_user_id](../account/users.md) | menu_items_audit_user_id_fkey | account.users.user_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| menu_items_pkey | frapid_db_user | btree | menu_item_id |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | menu_item_id | nextval('website.menu_items_menu_item_id_seq'::regclass) |
| 3 | sort | 0 |
| 8 | audit_ts | now() |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

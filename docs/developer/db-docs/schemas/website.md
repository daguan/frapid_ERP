# website schema

**Tables**

| # | Table Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |
| 1 | [categories](../tables/website/categories.md) | frapid_db_user | DEFAULT |  |
| 2 | [contacts](../tables/website/contacts.md) | frapid_db_user | DEFAULT |  |
| 3 | [contents](../tables/website/contents.md) | frapid_db_user | DEFAULT |  |
| 4 | [email_subscriptions](../tables/website/email_subscriptions.md) | frapid_db_user | DEFAULT |  |
| 5 | [menu_items](../tables/website/menu_items.md) | frapid_db_user | DEFAULT |  |
| 6 | [menus](../tables/website/menus.md) | frapid_db_user | DEFAULT |  |



**Sequences**

| # | Sequence Name | Owner | Data Type | Start Value | Increment | Description |
| --- | --- | --- | --- | --- | --- | --- |
| 1 | categories_category_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 2 | contacts_contact_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 3 | contents_content_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 4 | menu_items_menu_item_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 5 | menus_menu_id_seq | frapid_db_user | bigint | 1 | 1 |  |


**Views**

| # | View Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |
| 1 | [menu_item_view](../views/website/menu_item_view.md) | frapid_db_user | DEFAULT |  |
| 2 | [published_content_view](../views/website/published_content_view.md) | frapid_db_user | DEFAULT |  |
| 3 | [tag_view](../views/website/tag_view.md) | frapid_db_user | DEFAULT |  |



**Materialized Views**

| # | Matview Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |



**Functions**

| # | Function | Owner | Description |
| --- | --- | --- | --- |
| 1 | [add_email_subscription(_email text)RETURNS boolean](../functions/website/add_email_subscription-5105895.md) | frapid_db_user |  |
| 2 | [get_category_id_by_category_alias(_alias text)RETURNS integer](../functions/website/get_category_id_by_category_alias-5105897.md) | frapid_db_user |  |
| 3 | [get_category_id_by_category_name(_category_name text)RETURNS integer](../functions/website/get_category_id_by_category_name-5105896.md) | frapid_db_user |  |
| 4 | [remove_email_subscription(_email text)RETURNS boolean](../functions/website/remove_email_subscription-5105898.md) | frapid_db_user |  |



**Triggers**

| # | Trigger | Owner | Description |
| --- | --- | --- | --- |



**Types**

| # | Type | Base Type | Owner | Collation | Default | Type | StoreType | NotNull | Description |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../schemas.md)
* [Table of Contents](../../README.md)
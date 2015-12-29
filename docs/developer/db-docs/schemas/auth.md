# auth schema

**Tables**

| # | Table Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |
| 1 | [access_types](../tables/auth/access_types.md) | frapid_db_user | DEFAULT |  |
| 2 | [entity_access_policy](../tables/auth/entity_access_policy.md) | frapid_db_user | DEFAULT |  |
| 3 | [group_entity_access_policy](../tables/auth/group_entity_access_policy.md) | frapid_db_user | DEFAULT |  |
| 4 | [group_menu_access_policy](../tables/auth/group_menu_access_policy.md) | frapid_db_user | DEFAULT |  |
| 5 | [menu_access_policy](../tables/auth/menu_access_policy.md) | frapid_db_user | DEFAULT |  |



**Sequences**

| # | Sequence Name | Owner | Data Type | Start Value | Increment | Description |
| --- | --- | --- | --- | --- | --- | --- |
| 1 | entity_access_policy_entity_access_policy_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 2 | group_entity_access_policy_group_entity_access_policy_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 3 | group_menu_access_policy_group_menu_access_policy_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 4 | menu_access_policy_menu_access_policy_id_seq | frapid_db_user | bigint | 1 | 1 |  |


**Views**

| # | View Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |



**Materialized Views**

| # | Matview Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |



**Functions**

| # | Function | Owner | Description |
| --- | --- | --- | --- |
| 1 | [create_app_menu_policy(_role_name text, _office_id integer, _app_name text, _menu_names text[])RETURNS void](../functions/auth/create_app_menu_policy-5105538.md) | frapid_db_user |  |
| 2 | [get_group_menu_policy(_role_id integer, _office_id integer, _culture text)RETURNS TABLE(row_number integer, menu_id integer, app_name text, menu_name text, allowed boolean, url text, sort integer, icon character varying, parent_menu_id integer)](../functions/auth/get_group_menu_policy-5105539.md) | frapid_db_user |  |
| 3 | [get_menu(_user_id integer, _office_id integer, _culture text)RETURNS TABLE(menu_id integer, app_name character varying, menu_name character varying, url text, sort integer, icon character varying, parent_menu_id integer)](../functions/auth/get_menu-5105540.md) | frapid_db_user |  |
| 4 | [get_user_menu_policy(_user_id integer, _office_id integer, _culture text)RETURNS TABLE(row_number integer, menu_id integer, app_name text, menu_name text, allowed boolean, disallowed boolean, url text, sort integer, icon character varying, parent_menu_id integer)](../functions/auth/get_user_menu_policy-5105541.md) | frapid_db_user |  |
| 5 | [has_access(_user_id integer, _entity text, _access_type_id integer)RETURNS boolean](../functions/auth/has_access-5105542.md) | frapid_db_user |  |
| 6 | [save_group_menu_policy(_role_id integer, _office_id integer, _menu_ids integer[])RETURNS void](../functions/auth/save_group_menu_policy-5105543.md) | frapid_db_user |  |
| 7 | [save_user_menu_policy(_user_id integer, _office_id integer, _allowed_menu_ids integer[], _disallowed_menu_ids integer[])RETURNS void](../functions/auth/save_user_menu_policy-5105544.md) | frapid_db_user |  |



**Triggers**

| # | Trigger | Owner | Description |
| --- | --- | --- | --- |



**Types**

| # | Type | Base Type | Owner | Collation | Default | Type | StoreType | NotNull | Description |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../schemas.md)
* [Table of Contents](../../README.md)
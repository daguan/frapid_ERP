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
| 1 | [entity_view](../views/auth/entity_view.md) | frapid_db_user | DEFAULT |  |



**Materialized Views**

| # | Matview Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |



**Functions**

| # | Function | Owner | Description |
| --- | --- | --- | --- |
| 1 | [create_api_access_policy(_role_names text[], _office_id integer, _entity_name text, _access_types text[], _allow_access boolean)RETURNS void](../functions/auth/create_api_access_policy-4236134.md) | frapid_db_user |  |
| 2 | [create_app_menu_policy(_role_name text, _office_id integer, _app_name text, _menu_names text[])RETURNS void](../functions/auth/create_app_menu_policy-4236135.md) | frapid_db_user |  |
| 3 | [get_apps(_user_id integer, _office_id integer, _culture text)RETURNS TABLE(app_id integer, app_name text, name text, version_number text, publisher text, published_on date, icon text, landing_url text)](../functions/auth/get_apps-4236136.md) | frapid_db_user |  |
| 4 | [get_group_menu_policy(_role_id integer, _office_id integer, _culture text)RETURNS TABLE(row_number integer, menu_id integer, app_name text, menu_name text, allowed boolean, url text, sort integer, icon character varying, parent_menu_id integer)](../functions/auth/get_group_menu_policy-4236137.md) | frapid_db_user |  |
| 5 | [get_menu(_user_id integer, _office_id integer, _culture text)RETURNS TABLE(menu_id integer, app_name character varying, menu_name character varying, url text, sort integer, icon character varying, parent_menu_id integer)](../functions/auth/get_menu-4236138.md) | frapid_db_user |  |
| 6 | [get_user_menu_policy(_user_id integer, _office_id integer, _culture text)RETURNS TABLE(row_number integer, menu_id integer, app_name text, menu_name text, allowed boolean, disallowed boolean, url text, sort integer, icon character varying, parent_menu_id integer)](../functions/auth/get_user_menu_policy-4236139.md) | frapid_db_user |  |
| 7 | [has_access(_login_id bigint, _entity text, _access_type_id integer)RETURNS boolean](../functions/auth/has_access-4236140.md) | frapid_db_user |  |
| 8 | [save_api_group_policy(_role_id integer, _entity_name character varying, _office_id integer, _access_type_ids integer[], _allow_access boolean)RETURNS void](../functions/auth/save_api_group_policy-4236141.md) | frapid_db_user |  |
| 9 | [save_group_menu_policy(_role_id integer, _office_id integer, _menus text, _app_name text)RETURNS void](../functions/auth/save_group_menu_policy-4236142.md) | frapid_db_user |  |
| 10 | [save_user_menu_policy(_user_id integer, _office_id integer, _allowed text, _disallowed text)RETURNS void](../functions/auth/save_user_menu_policy-4236143.md) | frapid_db_user |  |



**Triggers**

| # | Trigger | Owner | Description |
| --- | --- | --- | --- |



**Types**

| # | Type | Base Type | Owner | Collation | Default | Type | StoreType | NotNull | Description |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../schemas.md)
* [Table of Contents](../../README.md)
# config schema

**Tables**

| # | Table Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |
| 1 | [custom_field_data_types](../tables/config/custom_field_data_types.md) | frapid_db_user | DEFAULT |  |
| 2 | [custom_field_forms](../tables/config/custom_field_forms.md) | frapid_db_user | DEFAULT |  |
| 3 | [custom_field_setup](../tables/config/custom_field_setup.md) | frapid_db_user | DEFAULT |  |
| 4 | [custom_fields](../tables/config/custom_fields.md) | frapid_db_user | DEFAULT |  |
| 5 | [email_queue](../tables/config/email_queue.md) | frapid_db_user | DEFAULT |  |
| 6 | [filters](../tables/config/filters.md) | frapid_db_user | DEFAULT |  |
| 7 | [flag_types](../tables/config/flag_types.md) | frapid_db_user | DEFAULT | Flags are used by users to mark transactions. The flags created by a user is not visible to others. |
| 8 | [flags](../tables/config/flags.md) | frapid_db_user | DEFAULT |  |
| 9 | [kanban_details](../tables/config/kanban_details.md) | frapid_db_user | DEFAULT |  |
| 10 | [kanbans](../tables/config/kanbans.md) | frapid_db_user | DEFAULT |  |
| 11 | [smtp_configs](../tables/config/smtp_configs.md) | frapid_db_user | DEFAULT |  |



**Sequences**

| # | Sequence Name | Owner | Data Type | Start Value | Increment | Description |
| --- | --- | --- | --- | --- | --- | --- |
| 1 | custom_field_setup_custom_field_setup_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 2 | custom_fields_custom_field_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 3 | email_queue_queue_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 4 | filters_filter_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 5 | flag_types_flag_type_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 6 | flags_flag_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 7 | kanban_details_kanban_detail_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 8 | kanbans_kanban_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 9 | smtp_configs_smtp_id_seq | frapid_db_user | bigint | 1 | 1 |  |


**Views**

| # | View Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |
| 1 | [custom_field_definition_view](../views/config/custom_field_definition_view.md) | frapid_db_user | DEFAULT |  |
| 2 | [custom_field_view](../views/config/custom_field_view.md) | frapid_db_user | DEFAULT |  |
| 3 | [filter_name_view](../views/config/filter_name_view.md) | frapid_db_user | DEFAULT |  |
| 4 | [flag_view](../views/config/flag_view.md) | frapid_db_user | DEFAULT |  |



**Materialized Views**

| # | Matview Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |



**Functions**

| # | Function | Owner | Description |
| --- | --- | --- | --- |
| 1 | [create_flag(user_id_ integer, flag_type_id_ integer, resource_ text, resource_key_ text, resource_id_ text)RETURNS void](../functions/config/create_flag-5105759.md) | frapid_db_user |  |
| 2 | [get_custom_field_definition(_table_name text, _resource_id text)RETURNS TABLE(table_name character varying, key_name character varying, custom_field_setup_id integer, form_name character varying, field_order integer, field_name character varying, field_label character varying, description text, data_type character varying, is_number boolean, is_date boolean, is_boolean boolean, is_long_text boolean, resource_id text, value text)](../functions/config/get_custom_field_definition-5105760.md) | frapid_db_user |  |
| 3 | [get_custom_field_form_name(_table_name character varying)RETURNS character varying](../functions/config/get_custom_field_form_name-5105761.md) | frapid_db_user |  |
| 4 | [get_custom_field_setup_id_by_table_name(_schema_name character varying, _table_name character varying, _field_name character varying)RETURNS integer](../functions/config/get_custom_field_setup_id_by_table_name-5105762.md) | frapid_db_user |  |
| 5 | [get_flag_type_id(user_id_ integer, resource_ text, resource_key_ text, resource_id_ text)RETURNS integer](../functions/config/get_flag_type_id-5105763.md) | frapid_db_user |  |
| 6 | [get_user_id_by_login_id(_login_id bigint)RETURNS integer](../functions/config/get_user_id_by_login_id-5105764.md) | frapid_db_user |  |



**Triggers**

| # | Trigger | Owner | Description |
| --- | --- | --- | --- |



**Types**

| # | Type | Base Type | Owner | Collation | Default | Type | StoreType | NotNull | Description |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../schemas.md)
* [Table of Contents](../../README.md)
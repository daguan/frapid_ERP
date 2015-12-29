# config.custom_field_setup table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | custom_field_setup_id | NOT NULL | integer | 0 |  |
| 2 | form_name | NOT NULL | character varying | 100 |  |
| 3 | field_order | NOT NULL | integer | 0 |  |
| 4 | field_name | NOT NULL | character varying | 100 |  |
| 5 | field_label | NOT NULL | character varying | 100 |  |
| 6 | data_type |  | character varying | 50 |  |
| 7 | description | NOT NULL | text | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 2 | [form_name](../config/custom_field_forms.md) | custom_field_setup_form_name_fkey | config.custom_field_forms.form_name |
| 6 | [data_type](../config/custom_field_data_types.md) | custom_field_setup_data_type_fkey | config.custom_field_data_types.data_type |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| custom_field_setup_pkey | frapid_db_user | btree | custom_field_setup_id |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | custom_field_setup_id | nextval('config.custom_field_setup_custom_field_setup_id_seq'::regclass) |
| 3 | field_order | 0 |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

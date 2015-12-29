# config.custom_field_data_types table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | data_type | [ ] | character varying | 50 |  |
| 2 | is_number | [x] | boolean | 0 |  |
| 3 | is_date | [x] | boolean | 0 |  |
| 4 | is_boolean | [x] | boolean | 0 |  |
| 5 | is_long_text | [x] | boolean | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| custom_field_data_types_pkey | frapid_db_user | btree | data_type |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 2 | is_number | false |
| 3 | is_date | false |
| 4 | is_boolean | false |
| 5 | is_long_text | false |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

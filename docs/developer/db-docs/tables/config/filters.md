# config.filters table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | filter_id | NOT NULL | bigint | 0 |  |
| 2 | object_name | NOT NULL | text | 0 |  |
| 3 | filter_name | NOT NULL | text | 0 |  |
| 4 | is_default | NOT NULL | boolean | 0 |  |
| 5 | is_default_admin | NOT NULL | boolean | 0 |  |
| 6 | filter_statement | NOT NULL | character varying | 12 |  |
| 7 | column_name | NOT NULL | text | 0 |  |
| 8 | filter_condition | NOT NULL | integer | 0 |  |
| 9 | filter_value |  | text | 0 |  |
| 10 | filter_and_value |  | text | 0 |  |
| 11 | audit_user_id |  | integer | 0 |  |
| 12 | audit_ts |  | timestamp with time zone | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 11 | [audit_user_id](../account/users.md) | filters_audit_user_id_fkey | account.users.user_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| filters_pkey | frapid_db_user | btree | filter_id |  |
| filters_object_name_inx | frapid_db_user | btree | object_name |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | filter_id | nextval('config.filters_filter_id_seq'::regclass) |
| 4 | is_default | false |
| 5 | is_default_admin | false |
| 6 | filter_statement | 'WHERE'::character varying |
| 12 | audit_ts | now() |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

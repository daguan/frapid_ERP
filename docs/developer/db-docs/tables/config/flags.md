# config.flags table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | flag_id | NOT NULL | bigint | 0 |  |
| 2 | user_id | NOT NULL | integer | 0 |  |
| 3 | flag_type_id | NOT NULL | integer | 0 |  |
| 4 | resource |  | text | 0 |  |
| 5 | resource_key |  | text | 0 |  |
| 6 | resource_id |  | text | 0 |  |
| 7 | flagged_on |  | timestamp with time zone | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 2 | [user_id](../account/users.md) | flags_user_id_fkey | account.users.user_id |
| 3 | [flag_type_id](../config/flag_types.md) | flags_flag_type_id_fkey | config.flag_types.flag_type_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| flags_pkey | frapid_db_user | btree | flag_id |  |
| flags_user_id_resource_resource_id_uix | frapid_db_user | btree | user_id, upper(resource), upper(resource_key), upper(resource_id) |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | flag_id | nextval('config.flags_flag_id_seq'::regclass) |
| 7 | flagged_on | now() |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

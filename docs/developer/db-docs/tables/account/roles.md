# account.roles table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | role_id | NOT NULL | integer | 0 |  |
| 2 | role_name | NOT NULL | character varying | 100 |  |
| 3 | is_administrator | NOT NULL | boolean | 0 |  |
| 4 | audit_user_id |  | integer | 0 |  |
| 5 | audit_ts |  | timestamp with time zone | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 4 | [audit_user_id](../account/users.md) | roles_audit_user_id_fkey | account.users.user_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| roles_pkey | frapid_db_user | btree | role_id |  |
| roles_role_name_key | frapid_db_user | btree | role_name |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 3 | is_administrator | false |
| 5 | audit_ts | now() |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

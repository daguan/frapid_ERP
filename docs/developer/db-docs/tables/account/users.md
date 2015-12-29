# account.users table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | user_id | NOT NULL | integer | 0 |  |
| 2 | email | NOT NULL | character varying | 100 |  |
| 3 | password |  | text | 0 |  |
| 4 | office_id | NOT NULL | integer | 0 |  |
| 5 | role_id | NOT NULL | integer | 0 |  |
| 6 | name |  | character varying | 100 |  |
| 7 | phone |  | character varying | 100 |  |
| 8 | status |  | boolean | 0 |  |
| 9 | audit_user_id |  | integer | 0 |  |
| 10 | audit_ts |  | timestamp with time zone | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 4 | [office_id](../core/offices.md) | users_office_id_fkey | core.offices.office_id |
| 5 | [role_id](../account/roles.md) | users_role_id_fkey | account.roles.role_id |
| 9 | [audit_user_id](../account/users.md) | users_audit_user_id_fkey | account.users.user_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| users_pkey | frapid_db_user | btree | user_id |  |
| users_email_uix | frapid_db_user | btree | lower(email::text) |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | user_id | nextval('account.users_user_id_seq'::regclass) |
| 8 | status | true |
| 10 | audit_ts | now() |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

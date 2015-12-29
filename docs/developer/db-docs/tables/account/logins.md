# account.logins table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | login_id | NOT NULL | bigint | 0 |  |
| 2 | user_id |  | integer | 0 |  |
| 3 | office_id |  | integer | 0 |  |
| 4 | browser |  | text | 0 |  |
| 5 | ip_address |  | character varying | 50 |  |
| 6 | is_active | NOT NULL | boolean | 0 |  |
| 7 | login_timestamp | NOT NULL | timestamp with time zone | 0 |  |
| 8 | culture | NOT NULL | character varying | 12 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 2 | [user_id](../account/users.md) | logins_user_id_fkey | account.users.user_id |
| 3 | [office_id](../core/offices.md) | logins_office_id_fkey | core.offices.office_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| logins_pkey | frapid_db_user | btree | login_id |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | login_id | nextval('account.logins_login_id_seq'::regclass) |
| 6 | is_active | true |
| 7 | login_timestamp | now() |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

# account.reset_requests table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | request_id | NOT NULL | uuid | 0 |  |
| 2 | user_id | NOT NULL | integer | 0 |  |
| 3 | email |  | text | 0 |  |
| 4 | name |  | text | 0 |  |
| 5 | requested_on | NOT NULL | timestamp with time zone | 0 |  |
| 6 | expires_on | NOT NULL | timestamp with time zone | 0 |  |
| 7 | browser |  | text | 0 |  |
| 8 | ip_address |  | character varying | 50 |  |
| 9 | confirmed |  | boolean | 0 |  |
| 10 | confirmed_on |  | timestamp with time zone | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 2 | [user_id](../account/users.md) | reset_requests_user_id_fkey | account.users.user_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| reset_requests_pkey | frapid_db_user | btree | request_id |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | request_id | gen_random_uuid() |
| 5 | requested_on | now() |
| 6 | expires_on | (now() + '24:00:00'::interval) |
| 9 | confirmed | false |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

# account.applications table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | application_id | NOT NULL | uuid | 0 |  |
| 2 | application_name | NOT NULL | character varying | 100 |  |
| 3 | display_name |  | character varying | 100 |  |
| 4 | version_number |  | character varying | 100 |  |
| 5 | publisher | NOT NULL | character varying | 100 |  |
| 6 | published_on |  | date | 0 |  |
| 7 | application_url |  | character varying | 500 |  |
| 8 | description |  | text | 0 |  |
| 9 | browser_based_app | NOT NULL | boolean | 0 |  |
| 10 | privacy_policy_url |  | character varying | 500 |  |
| 11 | terms_of_service_url |  | character varying | 500 |  |
| 12 | support_email |  | character varying | 100 |  |
| 13 | culture |  | character varying | 12 |  |
| 14 | redirect_url |  | character varying | 500 |  |
| 15 | app_secret |  | text | 0 |  |
| 16 | audit_user_id |  | integer | 0 |  |
| 17 | audit_ts |  | timestamp with time zone | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 16 | [audit_user_id](../account/users.md) | applications_audit_user_id_fkey | account.users.user_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| applications_pkey | frapid_db_user | btree | application_id |  |
| applications_app_secret_key | frapid_db_user | btree | app_secret |  |
| applications_app_name_uix | frapid_db_user | btree | lower(application_name::text) |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | application_id | gen_random_uuid() |
| 17 | audit_ts | now() |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

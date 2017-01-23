# core.apps table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | app_id | [ ] | integer | 0 |  |
| 2 | app_name | [ ] | character varying | 100 |  |
| 3 | name | [x] | character varying | 100 |  |
| 4 | version_number | [x] | character varying | 100 |  |
| 5 | publisher | [x] | character varying | 100 |  |
| 6 | published_on | [x] | date | 0 |  |
| 7 | icon | [x] | character varying | 100 |  |
| 8 | landing_url | [x] | text | 0 |  |
| 9 | audit_user_id | [x] | integer | 0 |  |
| 10 | audit_ts | [x] | timestamp with time zone | 0 |  |
| 11 | deleted | [x] | boolean | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| apps_pkey | frapid_db_user | btree | app_name |  |
| apps_app_name_uix | frapid_db_user | btree | upper(app_name::text) |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | app_id | nextval('core.apps_app_id_seq'::regclass) |
| 10 | audit_ts | now() |
| 11 | deleted | false |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

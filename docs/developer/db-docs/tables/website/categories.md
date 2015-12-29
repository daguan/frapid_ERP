# website.categories table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | category_id | NOT NULL | integer | 0 |  |
| 2 | category_name | NOT NULL | character varying | 100 |  |
| 3 | alias | NOT NULL | character varying | 50 |  |
| 4 | seo_keywords |  | character varying | 50 |  |
| 5 | seo_description |  | character varying | 100 |  |
| 6 | audit_user_id |  | integer | 0 |  |
| 7 | audit_ts |  | timestamp with time zone | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 6 | [audit_user_id](../account/users.md) | categories_audit_user_id_fkey | account.users.user_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| categories_pkey | frapid_db_user | btree | category_id |  |
| categories_alias_key | frapid_db_user | btree | alias |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | category_id | nextval('website.categories_category_id_seq'::regclass) |
| 7 | audit_ts | now() |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

# website.contents table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | content_id | [ ] | integer | 0 |  |
| 2 | category_id | [ ] | integer | 0 |  |
| 3 | title | [ ] | character varying | 100 |  |
| 4 | alias | [ ] | character varying | 50 |  |
| 5 | author_id | [x] | integer | 0 |  |
| 6 | publish_on | [ ] | timestamp with time zone | 0 |  |
| 7 | markdown | [x] | text | 0 |  |
| 8 | contents | [ ] | text | 0 |  |
| 9 | tags | [x] | text | 0 |  |
| 10 | is_draft | [ ] | boolean | 0 |  |
| 11 | seo_keywords | [ ] | character varying | 50 |  |
| 12 | seo_description | [ ] | character varying | 100 |  |
| 13 | is_homepage | [ ] | boolean | 0 |  |
| 14 | audit_user_id | [x] | integer | 0 |  |
| 15 | audit_ts | [x] | timestamp with time zone | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 2 | [category_id](../website/categories.md) | contents_category_id_fkey | website.categories.category_id |
| 5 | [author_id](../account/users.md) | contents_author_id_fkey | account.users.user_id |
| 14 | [audit_user_id](../account/users.md) | contents_audit_user_id_fkey | account.users.user_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| contents_pkey | frapid_db_user | btree | content_id |  |
| contents_alias_key | frapid_db_user | btree | alias |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | content_id | nextval('website.contents_content_id_seq'::regclass) |
| 10 | is_draft | true |
| 11 | seo_keywords | ''::character varying |
| 12 | seo_description | ''::character varying |
| 13 | is_homepage | false |
| 15 | audit_ts | now() |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

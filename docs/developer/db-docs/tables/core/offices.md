# core.offices table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | office_id | [ ] | integer | 0 |  |
| 2 | office_code | [ ] | character varying | 12 |  |
| 3 | office_name | [ ] | character varying | 150 |  |
| 4 | nick_name | [x] | character varying | 50 |  |
| 5 | registration_date | [x] | date | 0 |  |
| 6 | po_box | [x] | character varying | 128 |  |
| 7 | address_line_1 | [x] | character varying | 128 |  |
| 8 | address_line_2 | [x] | character varying | 128 |  |
| 9 | street | [x] | character varying | 50 |  |
| 10 | city | [x] | character varying | 50 |  |
| 11 | state | [x] | character varying | 50 |  |
| 12 | zip_code | [x] | character varying | 24 |  |
| 13 | country | [x] | character varying | 50 |  |
| 14 | phone | [x] | character varying | 24 |  |
| 15 | fax | [x] | character varying | 24 |  |
| 16 | email | [x] | character varying | 128 |  |
| 17 | url | [x] | character varying | 50 |  |
| 18 | logo | [x] | image | 0 |  |
| 19 | parent_office_id | [x] | integer | 0 |  |
| 20 | audit_user_id | [x] | integer | 0 |  |
| 21 | audit_ts | [x] | timestamp with time zone | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 19 | [parent_office_id](../core/offices.md) | offices_parent_office_id_fkey | core.offices.office_id |
| 20 | [audit_user_id](../account/users.md) | offices_audit_user_id_fkey | account.users.user_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| offices_pkey | frapid_db_user | btree | office_id |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | office_id | nextval('core.offices_office_id_seq'::regclass) |
| 21 | audit_ts | now() |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

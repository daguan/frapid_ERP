# core.offices table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | office_id | NOT NULL | integer | 0 |  |
| 2 | office_code | NOT NULL | character varying | 12 |  |
| 3 | office_name | NOT NULL | character varying | 150 |  |
| 4 | nick_name |  | character varying | 50 |  |
| 5 | registration_date |  | date | 0 |  |
| 6 | po_box |  | character varying | 128 |  |
| 7 | address_line_1 |  | character varying | 128 |  |
| 8 | address_line_2 |  | character varying | 128 |  |
| 9 | street |  | character varying | 50 |  |
| 10 | city |  | character varying | 50 |  |
| 11 | state |  | character varying | 50 |  |
| 12 | zip_code |  | character varying | 24 |  |
| 13 | country |  | character varying | 50 |  |
| 14 | phone |  | character varying | 24 |  |
| 15 | fax |  | character varying | 24 |  |
| 16 | email |  | character varying | 128 |  |
| 17 | url |  | character varying | 50 |  |
| 18 | logo |  | image | 0 |  |
| 19 | parent_office_id |  | integer | 0 |  |
| 20 | audit_user_id |  | integer | 0 |  |
| 21 | audit_ts |  | timestamp with time zone | 0 |  |



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

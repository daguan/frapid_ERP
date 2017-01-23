# finance.bank_accounts table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | bank_account_id | [ ] | integer | 0 |  |
| 2 | account_id | [x] | integer | 0 |  |
| 3 | maintained_by_user_id | [ ] | integer | 0 |  |
| 4 | is_merchant_account | [ ] | boolean | 0 |  |
| 5 | office_id | [ ] | integer | 0 |  |
| 6 | bank_name | [ ] | character varying | 128 |  |
| 7 | bank_branch | [ ] | character varying | 128 |  |
| 8 | bank_contact_number | [x] | character varying | 128 |  |
| 9 | bank_account_number | [x] | character varying | 128 |  |
| 10 | bank_account_type | [x] | character varying | 128 |  |
| 11 | street | [x] | character varying | 50 |  |
| 12 | city | [x] | character varying | 50 |  |
| 13 | state | [x] | character varying | 50 |  |
| 14 | country | [x] | character varying | 50 |  |
| 15 | phone | [x] | character varying | 50 |  |
| 16 | fax | [x] | character varying | 50 |  |
| 17 | cell | [x] | character varying | 50 |  |
| 18 | relationship_officer_name | [x] | character varying | 128 |  |
| 19 | relationship_officer_contact_number | [x] | character varying | 128 |  |
| 20 | audit_user_id | [x] | integer | 0 |  |
| 21 | audit_ts | [x] | timestamp with time zone | 0 |  |
| 22 | deleted | [x] | boolean | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |
| 2 | [account_id](../finance/accounts.md) | bank_accounts_account_id_fkey | finance.accounts.account_id |
| 3 | [maintained_by_user_id](../account/users.md) | bank_accounts_maintained_by_user_id_fkey | account.users.user_id |
| 5 | [office_id](../core/offices.md) | bank_accounts_office_id_fkey | core.offices.office_id |
| 20 | [audit_user_id](../account/users.md) | bank_accounts_audit_user_id_fkey | account.users.user_id |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| bank_accounts_pkey | frapid_db_user | btree | bank_account_id |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |
| bank_accounts_account_id_chk CHECK (finance.get_account_master_id_by_account_id(account_id::bigint) = 10102) |  |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | bank_account_id | nextval('finance.bank_accounts_bank_account_id_seq'::regclass) |
| 4 | is_merchant_account | false |
| 21 | audit_ts | now() |
| 22 | deleted | false |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

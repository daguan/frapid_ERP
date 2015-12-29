# config.email_queue table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | queue_id | [ ] | bigint | 0 |  |
| 2 | from_name | [ ] | character varying | 256 |  |
| 3 | reply_to | [ ] | character varying | 256 |  |
| 4 | subject | [ ] | character varying | 256 |  |
| 5 | send_to | [ ] | character varying | 256 |  |
| 6 | attachments | [x] | text | 0 |  |
| 7 | message | [ ] | text | 0 |  |
| 8 | added_on | [ ] | timestamp with time zone | 0 |  |
| 9 | delivered | [ ] | boolean | 0 |  |
| 10 | delivered_on | [x] | timestamp with time zone | 0 |  |
| 11 | canceled | [ ] | boolean | 0 |  |
| 12 | canceled_on | [x] | timestamp with time zone | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| email_queue_pkey | frapid_db_user | btree | queue_id |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | queue_id | nextval('config.email_queue_queue_id_seq'::regclass) |
| 8 | added_on | now() |
| 9 | delivered | false |
| 11 | canceled | false |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

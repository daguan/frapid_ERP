# website.email_subscriptions table



| # | Column Name | Nullable | Data Type | Max Length | Description |
| --- | --- | --- | --- | --- | --- |
| 1 | email_subscription_id | [ ] | uuid | 0 |  |
| 2 | email | [ ] | character varying | 100 |  |
| 3 | browser | [x] | text | 0 |  |
| 4 | ip_address | [x] | character varying | 50 |  |
| 5 | unsubscribed | [x] | boolean | 0 |  |
| 6 | subscribed_on | [x] | timestamp with time zone | 0 |  |
| 7 | unsubscribed_on | [x] | timestamp with time zone | 0 |  |



**Foreign Keys**

| # | Column Name | Key Name | References |
| --- | --- | --- | --- |



**Indices**

| Index Name | Owner | Access Method | Definition | Description |
| --- | --- | --- | --- | --- |
| email_subscriptions_pkey | frapid_db_user | btree | email_subscription_id |  |
| email_subscriptions_email_key | frapid_db_user | btree | email |  |



**Check Constraints**

| Constraint Name | Description |
| --- | --- |



**Default Values**

| # | Column Name | Default |
| --- | --- | --- |
| 1 | email_subscription_id | gen_random_uuid() |
| 5 | unsubscribed | false |
| 6 | subscribed_on | now() |


**Triggers**

| Trigger Name | Targets | On Event | Timing | Condition | Order | Orientation | Description |
| --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../../schemas.md)
* [Table List](../../tables.md)
* [Sequence List](../../sequences.md)
* [Table of Contents](../../README.md)

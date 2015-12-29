# account schema

**Tables**

| # | Table Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |
| 1 | [access_tokens](../tables/account/access_tokens.md) | frapid_db_user | DEFAULT |  |
| 2 | [applications](../tables/account/applications.md) | frapid_db_user | DEFAULT |  |
| 3 | [configuration_profiles](../tables/account/configuration_profiles.md) | frapid_db_user | DEFAULT |  |
| 4 | [fb_access_tokens](../tables/account/fb_access_tokens.md) | frapid_db_user | DEFAULT |  |
| 5 | [google_access_tokens](../tables/account/google_access_tokens.md) | frapid_db_user | DEFAULT |  |
| 6 | [installed_domains](../tables/account/installed_domains.md) | frapid_db_user | DEFAULT |  |
| 7 | [logins](../tables/account/logins.md) | frapid_db_user | DEFAULT |  |
| 8 | [registrations](../tables/account/registrations.md) | frapid_db_user | DEFAULT |  |
| 9 | [reset_requests](../tables/account/reset_requests.md) | frapid_db_user | DEFAULT |  |
| 10 | [roles](../tables/account/roles.md) | frapid_db_user | DEFAULT |  |
| 11 | [users](../tables/account/users.md) | frapid_db_user | DEFAULT |  |



**Sequences**

| # | Sequence Name | Owner | Data Type | Start Value | Increment | Description |
| --- | --- | --- | --- | --- | --- | --- |
| 1 | configuration_profiles_profile_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 2 | installed_domains_domain_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 3 | logins_login_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 4 | users_user_id_seq | frapid_db_user | bigint | 1 | 1 |  |


**Views**

| # | View Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |
| 1 | [sign_in_view](../views/account/sign_in_view.md) | frapid_db_user | DEFAULT |  |



**Materialized Views**

| # | Matview Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |



**Functions**

| # | Function | Owner | Description |
| --- | --- | --- | --- |
| 1 | [add_installed_domain(_domain_name text, _admin_email text)RETURNS void](../functions/account/add_installed_domain-5105379.md) | frapid_db_user |  |
| 2 | [can_confirm_registration(_token uuid)RETURNS boolean](../functions/account/can_confirm_registration-5105380.md) | frapid_db_user |  |
| 3 | [can_register_with_facebook()RETURNS boolean](../functions/account/can_register_with_facebook-5105381.md) | frapid_db_user |  |
| 4 | [can_register_with_google()RETURNS boolean](../functions/account/can_register_with_google-5105382.md) | frapid_db_user |  |
| 5 | [complete_reset(_request_id uuid, _password text)RETURNS void](../functions/account/complete_reset-5105383.md) | frapid_db_user |  |
| 6 | [confirm_registration(_token uuid)RETURNS boolean](../functions/account/confirm_registration-5105384.md) | frapid_db_user |  |
| 7 | [email_exists(_email character varying)RETURNS boolean](../functions/account/email_exists-5105385.md) | frapid_db_user |  |
| 8 | [fb_sign_in(_fb_user_id text, _email text, _office_id integer, _name text, _token text, _browser text, _ip_address text, _culture text)RETURNS TABLE(login_id bigint, status boolean, message text)](../functions/account/fb_sign_in-5105386.md) | frapid_db_user |  |
| 9 | [fb_user_exists(_user_id integer)RETURNS boolean](../functions/account/fb_user_exists-5105387.md) | frapid_db_user |  |
| 10 | [get_registration_office_id()RETURNS integer](../functions/account/get_registration_office_id-5105388.md) | frapid_db_user |  |
| 11 | [get_registration_role_id(_email text)RETURNS integer](../functions/account/get_registration_role_id-5105389.md) | frapid_db_user |  |
| 12 | [get_user_id_by_email(_email character varying)RETURNS integer](../functions/account/get_user_id_by_email-5105390.md) | frapid_db_user |  |
| 13 | [google_sign_in(_email text, _office_id integer, _name text, _token text, _browser text, _ip_address text, _culture text)RETURNS TABLE(login_id bigint, status boolean, message text)](../functions/account/google_sign_in-5105391.md) | frapid_db_user |  |
| 14 | [google_user_exists(_user_id integer)RETURNS boolean](../functions/account/google_user_exists-5105392.md) | frapid_db_user |  |
| 15 | [has_account(_email character varying)RETURNS boolean](../functions/account/has_account-5105393.md) | frapid_db_user |  |
| 16 | [has_active_reset_request(_email text)RETURNS boolean](../functions/account/has_active_reset_request-5105394.md) | frapid_db_user |  |
| 17 | [is_restricted_user(_email character varying)RETURNS boolean](../functions/account/is_restricted_user-5105395.md) | frapid_db_user |  |
| 18 | [is_valid_client_token(_client_token text, _ip_address text, _user_agent text)RETURNS boolean](../functions/account/is_valid_client_token-5105396.md) | frapid_db_user |  |
| 19 | [reset_account(_email text, _browser text, _ip_address text)RETURNS SETOF account.reset_requests](../functions/account/reset_account-5105397.md) | frapid_db_user |  |
| 20 | [sign_in(_email text, _office_id integer, _password text, _browser text, _ip_address text, _culture text)RETURNS TABLE(login_id bigint, status boolean, message text)](../functions/account/sign_in-5105398.md) | frapid_db_user |  |
| 21 | [user_exists(_email character varying)RETURNS boolean](../functions/account/user_exists-5105399.md) | frapid_db_user |  |



**Triggers**

| # | Trigger | Owner | Description |
| --- | --- | --- | --- |
| 1 | [token_auto_expiry_trigger()RETURNS TRIGGER](../functions/account/token_auto_expiry_trigger-5105405.md) | frapid_db_user |  |



**Types**

| # | Type | Base Type | Owner | Collation | Default | Type | StoreType | NotNull | Description |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../schemas.md)
* [Table of Contents](../../README.md)
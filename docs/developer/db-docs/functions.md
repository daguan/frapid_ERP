# Functions

| # | Function | Owner | Description |
| - | -------- | ----- | ----------- |
| 1 | [website](schemas/website.md).[add_email_subscription(_email text) RETURNS boolean](functions/website/add_email_subscription-5105895.md) | frapid_db_user |   |
| 2 | [account](schemas/account.md).[add_installed_domain(_domain_name text, _admin_email text) RETURNS void](functions/account/add_installed_domain-5105379.md) | frapid_db_user |   |
| 3 | [account](schemas/account.md).[can_confirm_registration(_token uuid) RETURNS boolean](functions/account/can_confirm_registration-5105380.md) | frapid_db_user |   |
| 4 | [account](schemas/account.md).[can_register_with_facebook() RETURNS boolean](functions/account/can_register_with_facebook-5105381.md) | frapid_db_user |   |
| 5 | [account](schemas/account.md).[can_register_with_google() RETURNS boolean](functions/account/can_register_with_google-5105382.md) | frapid_db_user |   |
| 6 | [account](schemas/account.md).[complete_reset(_request_id uuid, _password text) RETURNS void](functions/account/complete_reset-5105383.md) | frapid_db_user |   |
| 7 | [account](schemas/account.md).[confirm_registration(_token uuid) RETURNS boolean](functions/account/confirm_registration-5105384.md) | frapid_db_user |   |
| 8 | [core](schemas/core.md).[create_app(_app_name text, _name text, _version_number text, _publisher text, _published_on date, _icon text, _landing_url text, _dependencies text[]) RETURNS void](functions/core/create_app-5105160.md) | frapid_db_user |   |
| 9 | [auth](schemas/auth.md).[create_app_menu_policy(_role_name text, _office_id integer, _app_name text, _menu_names text[]) RETURNS void](functions/auth/create_app_menu_policy-5105538.md) | frapid_db_user |   |
| 10 | [config](schemas/config.md).[create_flag(user_id_ integer, flag_type_id_ integer, resource_ text, resource_key_ text, resource_id_ text) RETURNS void](functions/config/create_flag-5105759.md) | frapid_db_user |   |
| 11 | [core](schemas/core.md).[create_menu(_app_name text, _menu_name text, _url text, _icon text, _parent_menu_name text) RETURNS integer](functions/core/create_menu-5105163.md) | frapid_db_user |   |
| 12 | [core](schemas/core.md).[create_menu(_sort integer, _app_name text, _menu_name text, _url text, _icon text, _parent_menu_name text) RETURNS integer](functions/core/create_menu-5105162.md) | frapid_db_user |   |
| 13 | [core](schemas/core.md).[create_menu(_sort integer, _app_name text, _menu_name text, _url text, _icon text, _parent_menu_id integer) RETURNS integer](functions/core/create_menu-5105161.md) | frapid_db_user |   |
| 14 | [account](schemas/account.md).[email_exists(_email character varying) RETURNS boolean](functions/account/email_exists-5105385.md) | frapid_db_user |   |
| 15 | [account](schemas/account.md).[fb_sign_in(_fb_user_id text, _email text, _office_id integer, _name text, _token text, _browser text, _ip_address text, _culture text) RETURNS TABLE(login_id bigint, status boolean, message text)](functions/account/fb_sign_in-5105386.md) | frapid_db_user |   |
| 16 | [account](schemas/account.md).[fb_user_exists(_user_id integer) RETURNS boolean](functions/account/fb_user_exists-5105387.md) | frapid_db_user |   |
| 17 | [website](schemas/website.md).[get_category_id_by_category_alias(_alias text) RETURNS integer](functions/website/get_category_id_by_category_alias-5105897.md) | frapid_db_user |   |
| 18 | [website](schemas/website.md).[get_category_id_by_category_name(_category_name text) RETURNS integer](functions/website/get_category_id_by_category_name-5105896.md) | frapid_db_user |   |
| 19 | [config](schemas/config.md).[get_custom_field_definition(_table_name text, _resource_id text) RETURNS TABLE(table_name character varying, key_name character varying, custom_field_setup_id integer, form_name character varying, field_order integer, field_name character varying, field_label character varying, description text, data_type character varying, is_number boolean, is_date boolean, is_boolean boolean, is_long_text boolean, resource_id text, value text)](functions/config/get_custom_field_definition-5105760.md) | frapid_db_user |   |
| 20 | [config](schemas/config.md).[get_custom_field_form_name(_table_name character varying) RETURNS character varying](functions/config/get_custom_field_form_name-5105761.md) | frapid_db_user |   |
| 21 | [config](schemas/config.md).[get_custom_field_setup_id_by_table_name(_schema_name character varying, _table_name character varying, _field_name character varying) RETURNS integer](functions/config/get_custom_field_setup_id_by_table_name-5105762.md) | frapid_db_user |   |
| 22 | [config](schemas/config.md).[get_flag_type_id(user_id_ integer, resource_ text, resource_key_ text, resource_id_ text) RETURNS integer](functions/config/get_flag_type_id-5105763.md) | frapid_db_user |   |
| 23 | [auth](schemas/auth.md).[get_group_menu_policy(_role_id integer, _office_id integer, _culture text) RETURNS TABLE(row_number integer, menu_id integer, app_name text, menu_name text, allowed boolean, url text, sort integer, icon character varying, parent_menu_id integer)](functions/auth/get_group_menu_policy-5105539.md) | frapid_db_user |   |
| 24 | [auth](schemas/auth.md).[get_menu(_user_id integer, _office_id integer, _culture text) RETURNS TABLE(menu_id integer, app_name character varying, menu_name character varying, url text, sort integer, icon character varying, parent_menu_id integer)](functions/auth/get_menu-5105540.md) | frapid_db_user |   |
| 25 | [core](schemas/core.md).[get_office_id_by_office_name(_office_name text) RETURNS integer](functions/core/get_office_id_by_office_name-5105164.md) | frapid_db_user |   |
| 26 | [account](schemas/account.md).[get_registration_office_id() RETURNS integer](functions/account/get_registration_office_id-5105388.md) | frapid_db_user |   |
| 27 | [account](schemas/account.md).[get_registration_role_id(_email text) RETURNS integer](functions/account/get_registration_role_id-5105389.md) | frapid_db_user |   |
| 28 | [account](schemas/account.md).[get_user_id_by_email(_email character varying) RETURNS integer](functions/account/get_user_id_by_email-5105390.md) | frapid_db_user |   |
| 29 | [config](schemas/config.md).[get_user_id_by_login_id(_login_id bigint) RETURNS integer](functions/config/get_user_id_by_login_id-5105764.md) | frapid_db_user |   |
| 30 | [auth](schemas/auth.md).[get_user_menu_policy(_user_id integer, _office_id integer, _culture text) RETURNS TABLE(row_number integer, menu_id integer, app_name text, menu_name text, allowed boolean, disallowed boolean, url text, sort integer, icon character varying, parent_menu_id integer)](functions/auth/get_user_menu_policy-5105541.md) | frapid_db_user |   |
| 31 | [account](schemas/account.md).[google_sign_in(_email text, _office_id integer, _name text, _token text, _browser text, _ip_address text, _culture text) RETURNS TABLE(login_id bigint, status boolean, message text)](functions/account/google_sign_in-5105391.md) | frapid_db_user |   |
| 32 | [account](schemas/account.md).[google_user_exists(_user_id integer) RETURNS boolean](functions/account/google_user_exists-5105392.md) | frapid_db_user |   |
| 33 | [auth](schemas/auth.md).[has_access(_user_id integer, _entity text, _access_type_id integer) RETURNS boolean](functions/auth/has_access-5105542.md) | frapid_db_user |   |
| 34 | [account](schemas/account.md).[has_account(_email character varying) RETURNS boolean](functions/account/has_account-5105393.md) | frapid_db_user |   |
| 35 | [account](schemas/account.md).[has_active_reset_request(_email text) RETURNS boolean](functions/account/has_active_reset_request-5105394.md) | frapid_db_user |   |
| 36 | [account](schemas/account.md).[is_restricted_user(_email character varying) RETURNS boolean](functions/account/is_restricted_user-5105395.md) | frapid_db_user |   |
| 37 | [account](schemas/account.md).[is_valid_client_token(_client_token text, _ip_address text, _user_agent text) RETURNS boolean](functions/account/is_valid_client_token-5105396.md) | frapid_db_user |   |
| 38 | [website](schemas/website.md).[remove_email_subscription(_email text) RETURNS boolean](functions/website/remove_email_subscription-5105898.md) | frapid_db_user |   |
| 39 | [account](schemas/account.md).[reset_account(_email text, _browser text, _ip_address text) RETURNS SETOF account.reset_requests](functions/account/reset_account-5105397.md) | frapid_db_user |   |
| 40 | [auth](schemas/auth.md).[save_group_menu_policy(_role_id integer, _office_id integer, _menu_ids integer[]) RETURNS void](functions/auth/save_group_menu_policy-5105543.md) | frapid_db_user |   |
| 41 | [auth](schemas/auth.md).[save_user_menu_policy(_user_id integer, _office_id integer, _allowed_menu_ids integer[], _disallowed_menu_ids integer[]) RETURNS void](functions/auth/save_user_menu_policy-5105544.md) | frapid_db_user |   |
| 42 | [account](schemas/account.md).[sign_in(_email text, _office_id integer, _password text, _browser text, _ip_address text, _culture text) RETURNS TABLE(login_id bigint, status boolean, message text)](functions/account/sign_in-5105398.md) | frapid_db_user |   |
| 43 | [account](schemas/account.md).[user_exists(_email character varying) RETURNS boolean](functions/account/user_exists-5105399.md) | frapid_db_user |   |



[Table of Contents](README.md)

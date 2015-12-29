# core schema

**Tables**

| # | Table Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |
| 1 | [app_dependencies](../tables/core/app_dependencies.md) | frapid_db_user | DEFAULT |  |
| 2 | [apps](../tables/core/apps.md) | frapid_db_user | DEFAULT |  |
| 3 | [menu_locale](../tables/core/menu_locale.md) | frapid_db_user | DEFAULT |  |
| 4 | [menus](../tables/core/menus.md) | frapid_db_user | DEFAULT |  |
| 5 | [offices](../tables/core/offices.md) | frapid_db_user | DEFAULT |  |



**Sequences**

| # | Sequence Name | Owner | Data Type | Start Value | Increment | Description |
| --- | --- | --- | --- | --- | --- | --- |
| 1 | app_dependencies_app_dependency_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 2 | menu_locale_menu_locale_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 3 | menus_menu_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 4 | offices_office_id_seq | frapid_db_user | bigint | 1 | 1 |  |


**Views**

| # | View Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |



**Materialized Views**

| # | Matview Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |



**Functions**

| # | Function | Owner | Description |
| --- | --- | --- | --- |
| 1 | [create_app(_app_name text, _name text, _version_number text, _publisher text, _published_on date, _icon text, _landing_url text, _dependencies text[])RETURNS void](../functions/core/create_app-5105160.md) | frapid_db_user |  |
| 2 | [create_menu(_sort integer, _app_name text, _menu_name text, _url text, _icon text, _parent_menu_id integer)RETURNS integer](../functions/core/create_menu-5105161.md) | frapid_db_user |  |
| 3 | [create_menu(_sort integer, _app_name text, _menu_name text, _url text, _icon text, _parent_menu_name text)RETURNS integer](../functions/core/create_menu-5105162.md) | frapid_db_user |  |
| 4 | [create_menu(_app_name text, _menu_name text, _url text, _icon text, _parent_menu_name text)RETURNS integer](../functions/core/create_menu-5105163.md) | frapid_db_user |  |
| 5 | [get_office_id_by_office_name(_office_name text)RETURNS integer](../functions/core/get_office_id_by_office_name-5105164.md) | frapid_db_user |  |



**Triggers**

| # | Trigger | Owner | Description |
| --- | --- | --- | --- |



**Types**

| # | Type | Base Type | Owner | Collation | Default | Type | StoreType | NotNull | Description |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |


### Related Contents
* [Schema List](../schemas.md)
* [Table of Contents](../../README.md)
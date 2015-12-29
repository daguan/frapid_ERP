# account.sign_in_view view

| Schema | [account](../../schemas/account.md) |
| --- | --- |
| Materialized View Name | sign_in_view |
| Owner | frapid_db_user |
| Tablespace | DEFAULT |
| Description |  |

**Source:**

```plpgsql
 CREATE OR REPLACE VIEW account.sign_in_view
 AS
 SELECT logins.login_id,
    users.email,
    logins.user_id,
    roles.role_id,
    roles.role_name,
    roles.is_administrator,
    logins.browser,
    logins.ip_address,
    logins.login_timestamp,
    logins.culture,
    logins.office_id,
    offices.office_name,
    ((offices.office_code::text || ' ('::text) || offices.office_name::text) || ')'::text AS office
   FROM account.logins
     JOIN account.users ON users.user_id = logins.user_id
     JOIN account.roles ON roles.role_id = users.role_id
     JOIN core.offices ON offices.office_id = logins.office_id;
```


### Related Contents
* [Schema List](../../schemas.md)
* [View List](../../views.md)
* [Materialized View List](../../materialized-views.md)
* [Table of Contents](../../README.md)


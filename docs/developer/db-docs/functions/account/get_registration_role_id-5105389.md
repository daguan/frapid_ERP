# account.get_registration_role_id function:

```plpgsql
CREATE OR REPLACE FUNCTION account.get_registration_role_id(_email text)
RETURNS integer
```
* Schema : [account](../../schemas/account.md)
* Function Name : get_registration_role_id
* Arguments : _email text
* Owner : frapid_db_user
* Result Type : integer
* Description : 


**Source:**
```sql
CREATE OR REPLACE FUNCTION account.get_registration_role_id(_email text)
 RETURNS integer
 LANGUAGE plpgsql
 STABLE
AS $function$
    DECLARE _is_admin               boolean = false;
    DECLARE _role_id                integer;
BEGIN
    IF EXISTS
    (
        SELECT * FROM account.installed_domains
        WHERE admin_email = _email
    ) THEN
        _is_admin = true;
    END IF;
   
    IF(_is_admin) THEN
        SELECT
            role_id
        INTO
            _role_id
        FROM account.roles
        WHERE is_administrator
        LIMIT 1;
    ELSE
        SELECT 
            registration_role_id
        INTO
            _role_id
        FROM account.configuration_profiles
        WHERE is_active;
    END IF;

    RETURN _role_id;
END
$function$

```

### Related Contents
* [Schema List](../../schemas.md)
* [Function List](../../functions.md)
* [Trigger List](../../triggers.md)
* [Table of Contents](../../README.md)


# auth.create_app_menu_policy function:

```plpgsql
CREATE OR REPLACE FUNCTION auth.create_app_menu_policy(_role_name text, _office_id integer, _app_name text, _menu_names text[])
RETURNS void
```
* Schema : [auth](../../schemas/auth.md)
* Function Name : create_app_menu_policy
* Arguments : _role_name text, _office_id integer, _app_name text, _menu_names text[]
* Owner : frapid_db_user
* Result Type : void
* Description : 


**Source:**
```sql
CREATE OR REPLACE FUNCTION auth.create_app_menu_policy(_role_name text, _office_id integer, _app_name text, _menu_names text[])
 RETURNS void
 LANGUAGE plpgsql
AS $function$
    DECLARE _role_id                integer;
    DECLARE _menu_ids               int[];
BEGIN
    SELECT
        role_id
    INTO
        _role_id
    FROM account.roles
    WHERE role_name = _role_name;

    IF(_menu_names = '{*}'::text[]) THEN
        SELECT
            array_agg(menu_id)
        INTO
            _menu_ids
        FROM core.menus
        WHERE app_name = _app_name;
    ELSE
        SELECT
            array_agg(menu_id)
        INTO
            _menu_ids
        FROM core.menus
        WHERE app_name = _app_name
        AND menu_name = ANY(_menu_names);
    END IF;
    
    PERFORM auth.save_group_menu_policy(_role_id, _office_id, _menu_ids);    
END
$function$

```

### Related Contents
* [Schema List](../../schemas.md)
* [Function List](../../functions.md)
* [Trigger List](../../triggers.md)
* [Table of Contents](../../README.md)


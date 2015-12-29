# account.email_exists function:

```plpgsql
CREATE OR REPLACE FUNCTION account.email_exists(_email character varying)
RETURNS boolean
```
* Schema : [account](../../schemas/account.md)
* Function Name : email_exists
* Arguments : _email character varying
* Owner : frapid_db_user
* Result Type : boolean
* Description : 


**Source:**
```sql
CREATE OR REPLACE FUNCTION account.email_exists(_email character varying)
 RETURNS boolean
 LANGUAGE plpgsql
AS $function$
    DECLARE _count                          integer;
BEGIN
    SELECT count(*) INTO _count FROM account.users WHERE lower(email) = LOWER(_email);

    IF(COALESCE(_count, 0) =0) THEN
        SELECT count(*) INTO _count FROM account.registrations WHERE lower(email) = LOWER(_email);
    END IF;
    
    RETURN COALESCE(_count, 0) > 0;
END
$function$

```

### Related Contents
* [Schema List](../../schemas.md)
* [Function List](../../functions.md)
* [Trigger List](../../triggers.md)
* [Table of Contents](../../README.md)


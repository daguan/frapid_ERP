# website.add_email_subscription function:

```plpgsql
CREATE OR REPLACE FUNCTION website.add_email_subscription(_email text)
RETURNS boolean
```
* Schema : [website](../../schemas/website.md)
* Function Name : add_email_subscription
* Arguments : _email text
* Owner : frapid_db_user
* Result Type : boolean
* Description : 


**Source:**
```sql
CREATE OR REPLACE FUNCTION website.add_email_subscription(_email text)
 RETURNS boolean
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF NOT EXISTS
    (
        SELECT * FROM website.email_subscriptions
        WHERE email = _email
    ) THEN
        INSERT INTO website.email_subscriptions(email)
        SELECT _email;
        
        RETURN true;
    END IF;

    RETURN false;
END
$function$

```

### Related Contents
* [Schema List](../../schemas.md)
* [Function List](../../functions.md)
* [Trigger List](../../triggers.md)
* [Table of Contents](../../README.md)


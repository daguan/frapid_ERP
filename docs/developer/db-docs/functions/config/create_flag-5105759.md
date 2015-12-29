# config.create_flag function:

```plpgsql
CREATE OR REPLACE FUNCTION config.create_flag(user_id_ integer, flag_type_id_ integer, resource_ text, resource_key_ text, resource_id_ text)
RETURNS void
```
* Schema : [config](../../schemas/config.md)
* Function Name : create_flag
* Arguments : user_id_ integer, flag_type_id_ integer, resource_ text, resource_key_ text, resource_id_ text
* Owner : frapid_db_user
* Result Type : void
* Description : 


**Source:**
```sql
CREATE OR REPLACE FUNCTION config.create_flag(user_id_ integer, flag_type_id_ integer, resource_ text, resource_key_ text, resource_id_ text)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF NOT EXISTS(SELECT * FROM config.flags WHERE user_id=user_id_ AND resource=resource_ AND resource_key=resource_key_ AND resource_id=resource_id_) THEN
        INSERT INTO config.flags(user_id, flag_type_id, resource, resource_key, resource_id)
        SELECT user_id_, flag_type_id_, resource_, resource_key_, resource_id_;
    ELSE
        UPDATE config.flags
        SET
            flag_type_id=flag_type_id_
        WHERE 
            user_id=user_id_ 
        AND 
            resource=resource_ 
        AND 
            resource_key=resource_key_ 
        AND 
            resource_id=resource_id_;
    END IF;
END
$function$

```

### Related Contents
* [Schema List](../../schemas.md)
* [Function List](../../functions.md)
* [Trigger List](../../triggers.md)
* [Table of Contents](../../README.md)


# config.get_flag_type_id function:

```plpgsql
CREATE OR REPLACE FUNCTION config.get_flag_type_id(user_id_ integer, resource_ text, resource_key_ text, resource_id_ text)
RETURNS integer
```
* Schema : [config](../../schemas/config.md)
* Function Name : get_flag_type_id
* Arguments : user_id_ integer, resource_ text, resource_key_ text, resource_id_ text
* Owner : frapid_db_user
* Result Type : integer
* Description : 


**Source:**
```sql
CREATE OR REPLACE FUNCTION config.get_flag_type_id(user_id_ integer, resource_ text, resource_key_ text, resource_id_ text)
 RETURNS integer
 LANGUAGE plpgsql
 STABLE
AS $function$
BEGIN
    RETURN flag_type_id
    FROM config.flags
    WHERE user_id=$1
    AND resource=$2
    AND resource_key=$3
    AND resource_id=$4;
END
$function$

```

### Related Contents
* [Schema List](../../schemas.md)
* [Function List](../../functions.md)
* [Trigger List](../../triggers.md)
* [Table of Contents](../../README.md)


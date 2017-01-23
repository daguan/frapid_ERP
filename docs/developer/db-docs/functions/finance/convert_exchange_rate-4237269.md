# finance.convert_exchange_rate function:

```plpgsql
CREATE OR REPLACE FUNCTION finance.convert_exchange_rate(office_id integer, source_currency_code character varying, destination_currency_code character varying)
RETURNS decimal_strict2
```
* Schema : [finance](../../schemas/finance.md)
* Function Name : convert_exchange_rate
* Arguments : office_id integer, source_currency_code character varying, destination_currency_code character varying
* Owner : frapid_db_user
* Result Type : decimal_strict2
* Description : 


**Source:**
```sql
CREATE OR REPLACE FUNCTION finance.convert_exchange_rate(office_id integer, source_currency_code character varying, destination_currency_code character varying)
 RETURNS decimal_strict2
 LANGUAGE plpgsql
AS $function$
    DECLARE _unit integer_strict2 = 0;
    DECLARE _exchange_rate decimal_strict2=0;
    DECLARE _from_source_currency decimal_strict2=0;
    DECLARE _from_destination_currency decimal_strict2=0;
BEGIN
    IF($2 = $3) THEN
        RETURN 1;
    END IF;


    _from_source_currency := finance.get_exchange_rate($1, $2);
    _from_destination_currency := finance.get_exchange_rate($1, $3);
        
    RETURN _from_source_currency / _from_destination_currency ; 
END
$function$

```

### Related Contents
* [Schema List](../../schemas.md)
* [Function List](../../functions.md)
* [Trigger List](../../triggers.md)
* [Table of Contents](../../README.md)


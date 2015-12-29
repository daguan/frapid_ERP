# config.get_custom_field_definition function:

```plpgsql
CREATE OR REPLACE FUNCTION config.get_custom_field_definition(_table_name text, _resource_id text)
RETURNS TABLE(table_name character varying, key_name character varying, custom_field_setup_id integer, form_name character varying, field_order integer, field_name character varying, field_label character varying, description text, data_type character varying, is_number boolean, is_date boolean, is_boolean boolean, is_long_text boolean, resource_id text, value text)
```
* Schema : [config](../../schemas/config.md)
* Function Name : get_custom_field_definition
* Arguments : _table_name text, _resource_id text
* Owner : frapid_db_user
* Result Type : TABLE(table_name character varying, key_name character varying, custom_field_setup_id integer, form_name character varying, field_order integer, field_name character varying, field_label character varying, description text, data_type character varying, is_number boolean, is_date boolean, is_boolean boolean, is_long_text boolean, resource_id text, value text)
* Description : 


**Source:**
```sql
CREATE OR REPLACE FUNCTION config.get_custom_field_definition(_table_name text, _resource_id text)
 RETURNS TABLE(table_name character varying, key_name character varying, custom_field_setup_id integer, form_name character varying, field_order integer, field_name character varying, field_label character varying, description text, data_type character varying, is_number boolean, is_date boolean, is_boolean boolean, is_long_text boolean, resource_id text, value text)
 LANGUAGE plpgsql
AS $function$
BEGIN
    DROP TABLE IF EXISTS definition_temp;
    CREATE TEMPORARY TABLE definition_temp
    (
        table_name              national character varying(100),
        key_name                national character varying(100),
        custom_field_setup_id   integer,
        form_name               national character varying(100),
        field_order             integer,
        field_name              national character varying(100),
        field_label             national character varying(100),
        description             text,
        data_type               national character varying(50),
        is_number               boolean,
        is_date                 boolean,
        is_boolean              boolean,
        is_long_text            boolean,
        resource_id             text,
        value                   text
    ) ON COMMIT DROP;
    
    INSERT INTO definition_temp
    SELECT * FROM config.custom_field_definition_view
    WHERE config.custom_field_definition_view.table_name = _table_name
    ORDER BY field_order;

    UPDATE definition_temp
    SET resource_id = _resource_id;

    UPDATE definition_temp
    SET value = config.custom_fields.value
    FROM config.custom_fields
    WHERE definition_temp.custom_field_setup_id = config.custom_fields.custom_field_setup_id
    AND config.custom_fields.resource_id = _resource_id;
    
    RETURN QUERY
    SELECT * FROM definition_temp;
END
$function$

```

### Related Contents
* [Schema List](../../schemas.md)
* [Function List](../../functions.md)
* [Trigger List](../../triggers.md)
* [Table of Contents](../../README.md)


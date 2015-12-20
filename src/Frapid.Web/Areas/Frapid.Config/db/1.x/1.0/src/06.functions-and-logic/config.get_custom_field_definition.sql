DROP FUNCTION IF EXISTS config.get_custom_field_definition
(
    _table_name             text,
    _resource_id            text
);

CREATE FUNCTION config.get_custom_field_definition
(
    _table_name             text,
    _resource_id            text
)
RETURNS TABLE
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
)
AS
$$
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
$$
LANGUAGE plpgsql;
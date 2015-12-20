DROP VIEW IF EXISTS config.custom_field_view;

CREATE VIEW config.custom_field_view
AS
SELECT
    config.custom_field_forms.table_name,
    config.custom_field_forms.key_name,
    config.custom_field_setup.custom_field_setup_id,
    config.custom_field_setup.form_name,
    config.custom_field_setup.field_order,
    config.custom_field_setup.field_name,
    config.custom_field_setup.field_label,
    config.custom_field_setup.description,
    config.custom_field_data_types.data_type,
    config.custom_field_data_types.is_number,
    config.custom_field_data_types.is_date,
    config.custom_field_data_types.is_boolean,
    config.custom_field_data_types.is_long_text,
    config.custom_fields.resource_id,
    config.custom_fields.value
FROM config.custom_field_setup
INNER JOIN config.custom_field_data_types
ON config.custom_field_data_types.data_type = config.custom_field_setup.data_type
INNER JOIN config.custom_field_forms
ON config.custom_field_forms.form_name = config.custom_field_setup.form_name
INNER JOIN config.custom_fields
ON config.custom_fields.custom_field_setup_id = config.custom_field_setup.custom_field_setup_id
ORDER BY field_order;
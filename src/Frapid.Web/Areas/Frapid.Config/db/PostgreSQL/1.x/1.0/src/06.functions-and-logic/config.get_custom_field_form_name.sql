DROP FUNCTION IF EXISTS config.get_custom_field_form_name
(
    _table_name character varying
);

CREATE FUNCTION config.get_custom_field_form_name
(
    _table_name character varying
)
RETURNS character varying
STABLE
AS
$$
BEGIN
    RETURN form_name 
    FROM config.custom_field_forms
    WHERE table_name = _table_name;
END
$$
LANGUAGE plpgsql;
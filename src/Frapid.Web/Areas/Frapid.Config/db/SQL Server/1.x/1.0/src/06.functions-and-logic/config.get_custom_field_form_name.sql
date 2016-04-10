IF OBJECT_ID('config.get_custom_field_form_name') IS NOT NULL
DROP FUNCTION config.get_custom_field_form_name;

GO

CREATE FUNCTION config.get_custom_field_form_name
(
    @table_name character varying
)
RETURNS character varying
BEGIN
    RETURN 
    (
		SELECT form_name 
		FROM config.custom_field_forms
		WHERE table_name = @table_name
	);
END;

GO

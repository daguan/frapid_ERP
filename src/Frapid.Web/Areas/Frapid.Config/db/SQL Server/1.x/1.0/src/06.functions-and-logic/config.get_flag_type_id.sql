IF OBJECT_ID('config.get_flag_type_id') IS NOT NULL
DROP FUNCTION config.get_flag_type_id;

GO


CREATE FUNCTION config.get_flag_type_id
(
    @user_id							integer,
    @resource							national character varying(500),
    @resource_key						national character varying(500),
    @resource_id						national character varying(500)
)
RETURNS integer
AS
BEGIN
    RETURN 
    (
		SELECT flag_type_id
		FROM config.flags
		WHERE user_id=@user_id
		AND resource=@resource
		AND resource_key=@resource_key
		AND resource_id=@resource_id
		AND config.flags.deleted = 0
		
	);
END;

GO

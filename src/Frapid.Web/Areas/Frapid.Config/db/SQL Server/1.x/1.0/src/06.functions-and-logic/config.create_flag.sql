IF OBJECT_ID('config.create_flag') IS NOT NULL
DROP PROCEDURE config.create_flag;

GO


CREATE PROCEDURE config.create_flag
(
    @user_id            integer,
    @flag_type_id       integer,
    @resource           national character varying(500),
    @resource_key       national character varying(500),
    @resource_id        national character varying(500)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
		SELECT * 
		FROM config.flags 
		WHERE user_id = @user_id 
		AND resource = @resource 
		AND resource_key = @resource_key 
		AND resource_id=@resource_id
	)
    BEGIN
        INSERT INTO config.flags(user_id, flag_type_id, resource, resource_key, resource_id)
        SELECT @user_id, @flag_type_id, @resource, @resource_key, @resource_id;
    END
    ELSE
    BEGIN
        UPDATE config.flags
        SET
            flag_type_id=@flag_type_id
        WHERE 
            user_id=@user_id 
        AND 
            resource=@resource 
        AND 
            resource_key=@resource_key 
        AND 
            resource_id=@resource_id;
    END;
END;



GO

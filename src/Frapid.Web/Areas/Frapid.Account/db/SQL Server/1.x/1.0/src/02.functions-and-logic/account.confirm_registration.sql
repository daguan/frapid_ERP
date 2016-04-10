IF OBJECT_ID('account.confirm_registration') IS NOT NULL
DROP PROCEDURE account.confirm_registration;

GO


CREATE PROCEDURE account.confirm_registration(@token uniqueidentifier)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @can_confirm        bit;
    DECLARE @office_id          integer;
    DECLARE @role_id            integer;

    SET @can_confirm = account.can_confirm_registration(@token);

    IF(@can_confirm = 0)
    BEGIN
        RETURN 0;
    END;

    SELECT
    TOP 1
        @office_id = registration_office_id        
    FROM account.configuration_profiles
    WHERE is_active = 1;

    INSERT INTO account.users(email, password, office_id, role_id, name, phone)
    SELECT email, password, @office_id, account.get_registration_role_id(email), name, phone
    FROM account.registrations
    WHERE registration_id = @token;

    UPDATE account.registrations
    SET 
        confirmed = 1, 
        confirmed_on = getutcdate()
    WHERE registration_id = @token;
    
    RETURN 1;
END;

GO

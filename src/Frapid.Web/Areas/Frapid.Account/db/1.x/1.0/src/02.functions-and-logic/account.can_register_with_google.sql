DROP FUNCTION IF EXISTS account.can_register_with_google();

CREATE FUNCTION account.can_register_with_google()
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT 1 FROM account.configuration_profiles
        WHERE is_active
        AND allow_registration
        AND allow_google_registration
    ) THEN
        RETURN true;
    END IF;
    
    RETURN false;
END
$$
LANGUAGE plpgsql;

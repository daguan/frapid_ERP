DROP FUNCTION IF EXISTS account.can_register_with_facebook();

CREATE FUNCTION account.can_register_with_facebook()
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT 1 FROM account.configuration_profiles
        WHERE is_active
        AND allow_registration
        AND allow_facebook_registration
    ) THEN
        RETURN true;
    END IF;
    
    RETURN false;
END
$$
LANGUAGE plpgsql;

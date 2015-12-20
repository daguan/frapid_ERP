DROP FUNCTION IF EXISTS account.get_registration_role_id();

CREATE FUNCTION account.get_registration_role_id()
RETURNS integer
AS
$$
BEGIN
    RETURN registration_role_id
    FROM account.configuration_profiles
    WHERE is_active;
END
$$
LANGUAGE plpgsql;

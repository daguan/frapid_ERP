DROP FUNCTION IF EXISTS account.get_registration_office_id();

CREATE FUNCTION account.get_registration_office_id()
RETURNS integer
AS
$$
BEGIN
    RETURN registration_office_id
    FROM account.configuration_profiles
    WHERE is_active;
END
$$
LANGUAGE plpgsql;
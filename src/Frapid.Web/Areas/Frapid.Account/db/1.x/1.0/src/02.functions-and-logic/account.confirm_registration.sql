DROP FUNCTION IF EXISTS account.confirm_registration(_token uuid);

CREATE FUNCTION account.confirm_registration(_token uuid)
RETURNS boolean
AS
$$
    DECLARE _can_confirm        boolean;
    DECLARE _office_id          integer;
    DECLARE _role_id            integer;
BEGIN
    _can_confirm := account.can_confirm_registration(_token);

    IF(NOT _can_confirm) THEN
        RETURN false;
    END IF;

    SELECT
        registration_office_id,
        registration_role_id
    INTO
        _office_id,
        _role_id
    FROM account.configuration_profiles
    WHERE is_active
    LIMIT 1;

    INSERT INTO account.users(email, password, office_id, role_id, name, phone)
    SELECT email, password, _office_id, _role_id, name, phone
    FROM account.registrations
    WHERE registration_id = _token;

    UPDATE account.registrations
    SET 
        confirmed = true, 
        confirmed_on = NOW()
    WHERE registration_id = _token;
    
    RETURN true;
END
$$
LANGUAGE plpgsql;

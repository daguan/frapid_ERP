DROP FUNCTION IF EXISTS account.get_registration_role_id(_email text);

CREATE FUNCTION account.get_registration_role_id(_email text)
RETURNS integer
STABLE
AS
$$
    DECLARE _is_admin               boolean = false;
    DECLARE _role_id                integer;
BEGIN
    IF EXISTS
    (
        SELECT * FROM account.installed_domains
        WHERE admin_email = _email
    ) THEN
        _is_admin = true;
    END IF;
   
    IF(_is_admin) THEN
        SELECT
            role_id
        INTO
            _role_id
        FROM account.roles
        WHERE is_administrator
        LIMIT 1;
    ELSE
        SELECT 
            registration_role_id
        INTO
            _role_id
        FROM account.configuration_profiles
        WHERE is_active;
    END IF;

    RETURN _role_id;
END
$$
LANGUAGE plpgsql;
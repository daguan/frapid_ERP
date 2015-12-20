DROP FUNCTION IF EXISTS config.get_user_id_by_login_id(_login_id bigint);

CREATE FUNCTION config.get_user_id_by_login_id(_login_id bigint)
RETURNS integer
AS
$$
BEGIN
    RETURN 
    user_id
    FROM account.logins
    WHERE login_id = _login_id;
END
$$
LANGUAGE plpgsql;

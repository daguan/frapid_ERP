DROP FUNCTION IF EXISTS account.google_user_exists(_user_id integer);

CREATE FUNCTION account.google_user_exists(_user_id integer)
RETURNS boolean
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT *
        FROM account.google_access_tokens
        WHERE account.google_access_tokens.user_id = _user_id
    ) THEN
        RETURN true;
    END IF;
    
    RETURN false;
END
$$
LANGUAGE plpgsql;

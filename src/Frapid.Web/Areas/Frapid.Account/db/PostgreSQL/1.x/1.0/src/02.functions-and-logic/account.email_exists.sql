DROP FUNCTION IF EXISTS account.email_exists(_email national character varying(100));

CREATE FUNCTION account.email_exists(_email national character varying(100))
RETURNS bool
AS
$$
    DECLARE _count                          integer;
BEGIN
    SELECT count(*) INTO _count FROM account.users WHERE lower(email) = LOWER(_email);

    IF(COALESCE(_count, 0) =0) THEN
        SELECT count(*) INTO _count FROM account.registrations WHERE lower(email) = LOWER(_email);
    END IF;
    
    RETURN COALESCE(_count, 0) > 0;
END
$$
LANGUAGE plpgsql;

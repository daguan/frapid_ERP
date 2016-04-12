IF OBJECT_ID('account.get_user_id_by_login_id') IS NOT NULL
DROP FUNCTION account.get_user_id_by_login_id;

GO

CREATE FUNCTION account.get_user_id_by_login_id(@login_id bigint)
RETURNS integer
AS
BEGIN
    RETURN
    (
		SELECT
		user_id
		FROM account.logins
		WHERE account.logins.login_id = @login_id
	);
END;


GO

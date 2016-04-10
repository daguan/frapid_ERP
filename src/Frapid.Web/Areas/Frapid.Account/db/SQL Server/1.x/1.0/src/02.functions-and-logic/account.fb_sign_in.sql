IF OBJECT_ID('account.fb_sign_in') IS NOT NULL
DROP PROCEDURE account.fb_sign_in;

GO

CREATE PROCEDURE account.fb_sign_in
(
    @fb_user_id                             national character varying(500),
    @email                                  national character varying(500),
    @office_id                              integer,
    @name                                   national character varying(500),
    @token                                  national character varying(500),
    @browser                                national character varying(500),
    @ip_address                             national character varying(500),
    @culture                                national character varying(500)
)
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @result TABLE
	(
		login_id                                bigint,
		status                                  bit,
		message                                 national character varying(500)
	);
	
    DECLARE @user_id                        integer;
    DECLARE @login_id                       bigint;
    DECLARE @auto_register                  bit = 0;

    IF(COALESCE(@office_id, 0) = 0)
    BEGIN
        IF(SELECT COUNT(*) FROM core.offices) = 1
        BEGIN
            SELECT @office_id = office_id
            FROM core.offices;
        END;
    END;

    IF account.is_restricted_user(@email) = 1
    BEGIN
        --LOGIN IS RESTRICTED TO THIS USER
        INSERT INTO @result
        SELECT CAST(NULL AS bigint), 0, 'Access is denied';
		
		SELECT * FROM @result;
        RETURN;
    END;

    SELECT @user_id = user_id
    FROM account.users
    WHERE account.users.email = @email;

    IF account.user_exists(@email) = 0 AND account.can_register_with_facebook() = 1
    BEGIN
        INSERT INTO account.users(role_id, office_id, email, name)
        SELECT account.get_registration_role_id(@email), account.get_registration_office_id(), @email, @name;
        
        SET @user_id = SCOPE_IDENTITY();
    END;

    IF account.fb_user_exists(@user_id) = 0
    BEGIN
        INSERT INTO account.fb_access_tokens(user_id, fb_user_id, token)
        SELECT COALESCE(@user_id, account.get_user_id_by_email(@email)), @fb_user_id, @token;
    END
    ELSE
    BEGIN
        UPDATE account.fb_access_tokens
        SET token = @token
        WHERE user_id = @user_id;    
    END;

    IF(@user_id IS NULL)
    BEGIN
        SELECT @user_id = user_id
        FROM account.users
        WHERE account.users.email = @email;
    END;
    
	UPDATE account.logins 
	SET is_active = 0 
	WHERE user_id=@user_id 
	AND office_id = @office_id 
	AND browser = @browser
	AND ip_address = @ip_address;

    INSERT INTO account.logins(user_id, office_id, browser, ip_address, login_timestamp, culture)
    SELECT @user_id, @office_id, @browser, @ip_address, getutcdate(), COALESCE(@culture, '');

    SET @login_id = SCOPE_IDENTITY();
	
    INSERT INTO @result
    SELECT @login_id, 1, 'Welcome';

	SELECT * FROM @result;
    RETURN;    
END;


GO
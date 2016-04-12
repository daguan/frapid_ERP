IF OBJECT_ID('auth.get_menu') IS NOT NULL
DROP FUNCTION auth.get_menu;

GO


CREATE FUNCTION auth.get_menu
(
    @user_id                            integer, 
    @office_id                          integer, 
    @culture                            national character varying(500)
)
RETURNS @result TABLE
(
	menu_id                             integer,
	app_name                            national character varying(500),
	menu_name                           national character varying(500),
	url                                 national character varying(500),
	sort                                integer,
	icon                                national character varying(500),
	parent_menu_id                      integer
)
AS
BEGIN


    DECLARE @role_id                    integer;

    SELECT
        @role_id = role_id        
    FROM account.users
    WHERE user_id = @user_id;

    --GROUP POLICY
    INSERT INTO @result(menu_id)
    SELECT auth.group_menu_access_policy.menu_id
    FROM auth.group_menu_access_policy
    WHERE office_id = @office_id
    AND role_id = @role_id;

    --USER POLICY : ALLOWED MENUS
    INSERT INTO @result(menu_id)
    SELECT auth.menu_access_policy.menu_id
    FROM auth.menu_access_policy
    WHERE office_id = @office_id
    AND user_id = @user_id
    AND allow_access = 1
    AND auth.menu_access_policy.menu_id NOT IN
    (
        SELECT menu_id
        FROM @result
    );

    --USER POLICY : DISALLOWED MENUS
    DELETE FROM @result
    WHERE menu_id
    IN
    (
        SELECT auth.menu_access_policy.menu_id
        FROM auth.menu_access_policy
        WHERE office_id = @office_id
        AND user_id = @user_id
        AND disallow_access = 1
    );

    
    UPDATE @result
    SET
        app_name        = core.menus.app_name,
        menu_name       = core.menus.menu_name,
        url             = core.menus.url,
        sort            = core.menus.sort,
        icon            = core.menus.icon,
        parent_menu_id  = core.menus.parent_menu_id
    FROM @result AS result
    INNER JOIN core.menus
    ON core.menus.menu_id = result.menu_id;

    UPDATE @result
    SET
        menu_name       = core.menu_locale.menu_text
    FROM @result AS result
    INNER JOIN core.menu_locale    
    ON core.menu_locale.menu_id = result.menu_id
    WHERE core.menu_locale.culture = @culture;
    

    RETURN;
END;

--EXECUTE auth.get_menu 1, 1, '';


GO

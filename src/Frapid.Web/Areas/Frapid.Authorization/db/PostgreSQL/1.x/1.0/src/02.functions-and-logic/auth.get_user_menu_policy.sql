DROP FUNCTION IF EXISTS auth.get_user_menu_policy
(
    _user_id        integer,
    _office_id      integer,
    _culture        text
);

CREATE FUNCTION auth.get_user_menu_policy
(
    _user_id        integer,
    _office_id      integer,
    _culture        text
)
RETURNS TABLE
(
    row_number                      integer,
    menu_id                         integer,
    app_name                        text,
    menu_name                       text,
    allowed                         boolean,
    disallowed                      boolean,
    url                             text,
    sort                            integer,
    icon                            national character varying(100),
    parent_menu_id                  integer
)
AS
$$
    DECLARE _role_id                    integer;
BEGIN
    SELECT
        role_id
    INTO
        _role_id
    FROM account.users
    WHERE account.users.user_id = _user_id
	AND NOT account.users.deleted;

    DROP TABLE IF EXISTS _temp_menu;
    CREATE TEMPORARY TABLE _temp_menu
    (
        row_number                      SERIAL,
        menu_id                         integer,
        app_name                        text,
        menu_name                       text,
        allowed                         boolean,
        disallowed                      boolean,
        url                             text,
        sort                            integer,
        icon                            national character varying(100),
        parent_menu_id                  integer
    ) ON COMMIT DROP;

    INSERT INTO _temp_menu(menu_id)
    SELECT core.menus.menu_id
    FROM core.menus
    ORDER BY core.menus.app_name, core.menus.sort, core.menus.menu_id;

    --GROUP POLICY
    UPDATE _temp_menu
    SET allowed = true
    FROM  auth.group_menu_access_policy
    WHERE auth.group_menu_access_policy.menu_id = _temp_menu.menu_id
    AND office_id = _office_id
    AND role_id = _role_id;
    
    --USER POLICY : ALLOWED MENUS
    UPDATE _temp_menu
    SET allowed = true
    FROM  auth.menu_access_policy
    WHERE auth.menu_access_policy.menu_id = _temp_menu.menu_id
    AND office_id = _office_id
    AND user_id = _user_id
    AND allow_access;


    --USER POLICY : DISALLOWED MENUS
    UPDATE _temp_menu
    SET disallowed = true
    FROM auth.menu_access_policy
    WHERE _temp_menu.menu_id = auth.menu_access_policy.menu_id 
    AND office_id = _office_id
    AND user_id = _user_id
    AND disallow_access;
   
    
    UPDATE _temp_menu
    SET
        app_name        = core.menus.app_name,
        menu_name       = core.menus.menu_name,
        url             = core.menus.url,
        sort            = core.menus.sort,
        icon            = core.menus.icon,
        parent_menu_id  = core.menus.parent_menu_id
    FROM core.menus
    WHERE core.menus.menu_id = _temp_menu.menu_id;

    UPDATE _temp_menu
    SET
        menu_name       = core.menu_locale.menu_text
    FROM core.menu_locale
    WHERE core.menu_locale.menu_id = _temp_menu.menu_id
    AND core.menu_locale.culture = _culture;
    

    RETURN QUERY
    SELECT * FROM _temp_menu
    ORDER BY app_name, sort, menu_id;
END
$$
LANGUAGE plpgsql;

--SELECT * FROM auth.get_user_menu_policy(1, 1, '');
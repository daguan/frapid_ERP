DROP FUNCTION IF EXISTS auth.get_apps(_user_id integer, _office_id integer, _culture text);

CREATE FUNCTION auth.get_apps(_user_id integer, _office_id integer, _culture text)
RETURNS TABLE
(
    app_id                              integer,
    app_name                            text,
    name                                text,
    version_number                      text,
    publisher                           text,
    published_on                        date,
    icon                                text,
    landing_url                         text
)
AS
$$
BEGIN
    RETURN QUERY
    SELECT
        core.apps.app_id,
        core.apps.app_name::text,
        core.apps.name::text,
        core.apps.version_number::text,
        core.apps.publisher::text,
        core.apps.published_on::date,
        core.apps.icon::text,
        core.apps.landing_url::text
    FROM core.apps
    WHERE core.apps.app_name IN
    (
        SELECT DISTINCT menus.app_name
        FROM auth.get_menu(_user_id, _office_id, _culture)
        AS menus
    );
END
$$
LANGUAGE plpgsql;

DROP FUNCTION IF EXISTS config.create_app
(
    _app_name                                   text,
    _name                                       text,
    _version_number                             text,
    _publisher                                  text,
    _published_on                               date,
    _icon                                       text,
    _landing_url                                text,
    _dependencies                               text[]
);

CREATE FUNCTION config.create_app
(
    _app_name                                   text,
    _name                                       text,
    _version_number                             text,
    _publisher                                  text,
    _published_on                               date,
    _icon                                       text,
    _landing_url                                text,
    _dependencies                               text[]
)
RETURNS void
AS
$$
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM config.apps
        WHERE LOWER(config.apps.app_name) = LOWER(_app_name)
    ) THEN
        UPDATE config.apps
        SET
            name = _name,
            version_number = _version_number,
            publisher = _publisher,
            published_on = _published_on,
            icon = _icon,
            landing_url = _landing_url
        WHERE
            app_name = _app_name;
    ELSE
        INSERT INTO config.apps(app_name, name, version_number, publisher, published_on, icon, landing_url)
        SELECT _app_name, _name, _version_number, _publisher, _published_on, _icon, _landing_url;
    END IF;

    DELETE FROM config.app_dependencies
    WHERE app_name = _app_name;

    INSERT INTO config.app_dependencies(app_name, depends_on)
    SELECT _app_name, UNNEST(_dependencies);
END
$$
LANGUAGE plpgsql;

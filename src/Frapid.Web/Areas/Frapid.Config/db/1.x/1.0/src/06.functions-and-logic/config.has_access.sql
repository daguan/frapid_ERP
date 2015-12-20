DROP FUNCTION IF EXISTS config.has_access(_user_id integer, _entity text, _access_type_id integer);

CREATE FUNCTION config.has_access(_user_id integer, _entity text, _access_type_id integer)
RETURNS boolean
AS
$$
    DECLARE _role_id                    integer;
    DECLARE _group_config               boolean = NULL;
    DECLARE _user_config                boolean = NULL;
    DECLARE _config                     boolean = true;
BEGIN
    SELECT role_id INTO _role_id FROM account.users WHERE user_id = _user_id;

    --GROUP config BASED ON ALL ENTITIES AND ALL ACCESS TYPES
    IF EXISTS
    (
        SELECT * FROM config.default_entity_access
        WHERE role_id = _role_id
        AND NOT allow_access
        AND access_type_id IS NULL
        AND COALESCE(entity_name, '') = ''
    ) THEN
        _group_config = false;
    END IF;

    --GROUP config BASED ON ALL ENTITIES AND SPECIFIED ACCESS TYPE
    IF EXISTS
    (
        SELECT * FROM config.default_entity_access
        WHERE role_id = _role_id
        AND NOT allow_access
        AND access_type_id = _access_type_id
        AND COALESCE(entity_name, '') = ''
    ) THEN
        _group_config = false;
    END IF;
 

    --GROUP config BASED ON SPECIFIED ENTITY AND ALL ACCESS TYPES
    IF EXISTS
    (
        SELECT * FROM config.default_entity_access
        WHERE role_id = _role_id
        AND NOT allow_access
        AND access_type_id IS NULL
        AND entity_name = _entity
    ) THEN
        _group_config = false;
    END IF;

    --GROUP config BASED ON SPECIFIED ENTITY AND SPECIFIED ACCESS TYPE
    IF EXISTS
    (
        SELECT * FROM config.default_entity_access
        WHERE role_id = _role_id
        AND NOT allow_access
        AND access_type_id = _access_type_id
        AND entity_name = _entity
    ) THEN
        _group_config = false;
    END IF;


    --USER config BASED ON ALL ENTITIES AND ALL ACCESS TYPES
    SELECT allow_access INTO _user_config FROM config.entity_access
    WHERE user_id = _user_id
    AND access_type_id IS NULL
    AND COALESCE(entity_name, '') = '';

    --USER config BASED ON SPECIFIED ENTITY AND ALL ACCESS TYPES
    IF(_user_config IS NULL) THEN
        SELECT allow_access INTO _user_config 
        FROM config.entity_access
        WHERE user_id = _user_id
        AND access_type_id IS NULL
        AND entity_name = _entity;
    END IF;
 
    --USER config BASED ON ALL ENTITIES AND SPECIFIED ACCESS TYPE
    IF(_user_config IS NULL) THEN
        SELECT allow_access INTO _user_config FROM config.entity_access
        WHERE user_id = _user_id
        AND access_type_id = _access_type_id
        AND COALESCE(entity_name, '') = '';
    END IF;
 

    --USER config BASED ON SPECIFIED ENTITY AND SPECIFIED ACCESS TYPE
    IF(_user_config IS NULL) THEN
        SELECT allow_access INTO _user_config FROM config.entity_access
        WHERE user_id = _user_id
        AND access_type_id = _access_type_id
        AND entity_name = _entity;
    END IF;

    IF(_group_config IS NOT NULL) THEN
        _config := _group_config;
    END IF;

    IF(_user_config IS NOT NULL) THEN
        _config := _user_config;
    END IF;

    RETURN _config;
END
$$
LANGUAGE plpgsql;
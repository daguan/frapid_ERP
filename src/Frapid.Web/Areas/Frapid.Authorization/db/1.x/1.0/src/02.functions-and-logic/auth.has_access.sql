DROP FUNCTION IF EXISTS auth.has_access(_user_id integer, _entity text, _access_type_id integer);

CREATE FUNCTION auth.has_access(_user_id integer, _entity text, _access_type_id integer)
RETURNS boolean
AS
$$
    DECLARE _role_id                                    integer;
    DECLARE _group_all_policy                           boolean = false;
    DECLARE _group_all_entity_specific_access_type      boolean = false;
    DECLARE _group_specific_entity_all_access_type      boolean = false;
    DECLARE _group_explicit_policy                      boolean = false;
    DECLARE _effective_group_policy                     boolean = false;
    DECLARE _user_all_policy                            boolean = false;
    DECLARE _user_all_entity_specific_access_type       boolean = false;
    DECLARE _user_specific_entity_all_access_type       boolean = false;
    DECLARE _user_explicit_policy                       boolean = false;
    DECLARE _effective_user_policy                      boolean = false;
BEGIN    
    --USER AUTHORIZATION BASED ON ALL ENTITIES AND ALL ACCESS TYPES
    SELECT 
        allow_access 
    INTO 
        _user_all_policy
    FROM auth.entity_access_policy
    WHERE user_id = _user_id
    AND access_type_id IS NULL
    AND COALESCE(entity_name, '') = '';

    --USER AUTHORIZATION BASED ON ALL ENTITIES AND SPECIFIED ACCESS TYPE
    SELECT 
        allow_access
    INTO
        _user_all_entity_specific_access_type
    FROM auth.entity_access_policy
    WHERE user_id = _user_id
    AND access_type_id = _access_type_id
    AND COALESCE(entity_name, '') = '';

    --USER AUTHORIZATION BASED ON SPECIFIED ENTITY AND ALL ACCESS TYPES
    SELECT
        allow_access
    INTO
        _user_specific_entity_all_access_type
    FROM auth.entity_access_policy
    WHERE user_id = _user_id
    AND access_type_id IS NULL
    AND entity_name = _entity;

    --USER AUTHORIZATION BASED ON SPECIFIED ENTITY AND SPECIFIED ACCESS TYPE
    SELECT 
        allow_access
    INTO
        _user_explicit_policy
    FROM auth.entity_access_policy
    WHERE user_id = _user_id
    AND access_type_id = _access_type_id
    AND entity_name = _entity;

    --EFFECTIVE USER POLICY BASED ON PRECEDENCE.
    _effective_user_policy := COALESCE(_user_explicit_policy, _user_specific_entity_all_access_type, _user_all_entity_specific_access_type, _user_all_policy);

    IF(_effective_user_policy IS NOT NULL) THEN
        RETURN _effective_user_policy;
    END IF;

    SELECT role_id INTO _role_id FROM account.users WHERE user_id = _user_id;

    --GROUP AUTHORIZATION BASED ON ALL ENTITIES AND ALL ACCESS TYPES
    SELECT 
        allow_access 
    INTO 
        _group_all_policy
    FROM auth.group_entity_access_policy
    WHERE role_id = _role_id
    AND access_type_id IS NULL
    AND COALESCE(entity_name, '') = '';

    --GROUP AUTHORIZATION BASED ON ALL ENTITIES AND SPECIFIED ACCESS TYPE
    SELECT 
        allow_access
    INTO
        _group_all_entity_specific_access_type
    FROM auth.group_entity_access_policy
    WHERE role_id = _role_id
    AND access_type_id = _access_type_id
    AND COALESCE(entity_name, '') = '';

    --GROUP AUTHORIZATION BASED ON SPECIFIED ENTITY AND ALL ACCESS TYPES
    SELECT
        allow_access
    INTO
        _group_specific_entity_all_access_type
    FROM auth.group_entity_access_policy
    WHERE role_id = _role_id
    AND access_type_id IS NULL
    AND entity_name = _entity;

    --GROUP AUTHORIZATION BASED ON SPECIFIED ENTITY AND SPECIFIED ACCESS TYPE
    SELECT 
        allow_access
    INTO
        _group_explicit_policy
    FROM auth.group_entity_access_policy
    WHERE role_id = _role_id
    AND access_type_id = _access_type_id
    AND entity_name = _entity;

    --EFFECTIVE GROUP POLICY BASED ON PRECEDENCE.
    _effective_group_policy := COALESCE(_group_explicit_policy, _group_specific_entity_all_access_type, _group_all_entity_specific_access_type, _group_all_policy);

    RETURN COALESCE(_effective_group_policy, false);    
END
$$
LANGUAGE plpgsql;

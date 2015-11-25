DROP SCHEMA IF EXISTS auth CASCADE;
CREATE SCHEMA auth;


CREATE TABLE auth.users
(
    user_id                         SERIAL PRIMARY KEY,
    email                           national character varying(100) NOT NULL UNIQUE,
    password                        text,
    api_key                         text UNIQUE,
    status                          boolean DEFAULT(true)
);

INSERT INTO auth.users(email)
SELECT 'binodnp@outlook.com';

CREATE TABLE auth.fb_access_tokens
(
    user_id                         integer PRIMARY KEY REFERENCES auth.users UNIQUE,
    fb_user_id                      bigint,
    token                           text
);

INSERT INTO auth.fb_access_tokens
SELECT 1, 908672099210103;

CREATE TABLE auth.sign_ins
(
    sign_in_id                              BIGSERIAL PRIMARY KEY,
    user_id                                 integer REFERENCES auth.users,
    browser                                 text,
    ip_address                              national character varying(50) NOT NULL,
    login_timestamp                         TIMESTAMP WITH TIME ZONE NOT NULL 
                                            DEFAULT(NOW()),
    culture                                 national character varying(12) NOT NULL    
);

CREATE FUNCTION auth.fb_sign_in
(
    _fb_user_id                             bigint,
    _token                                  text,
    _browser                                text,
    _ip_address                             national character varying(50),
    _culture                                national character varying(12)
)
RETURNS TABLE
(
    sign_in_id                              bigint,
    status                                  boolean,
    message                                 text
)
AS
$$
    DECLARE _user_id                        integer;
    DECLARE _sign_in_id                     bigint;
BEGIN
    
    IF NOT EXISTS
    (
        SELECT *
        FROM auth.fb_access_tokens
        INNER JOIN auth.users
        ON auth.users.user_id = auth.fb_access_tokens.user_id
        WHERE auth.fb_access_tokens.fb_user_id = _fb_user_id
        AND auth.users.status        
    ) THEN
        RETURN QUERY
        SELECT NULL::bigint, false, 'Access is denied'::text;

        RETURN;
    END IF;

    SELECT user_id INTO _user_id
    FROM auth.fb_access_tokens
    WHERE auth.fb_access_tokens.fb_user_id = _fb_user_id;
    
    INSERT INTO auth.sign_ins(user_id, browser, ip_address, login_timestamp, culture)
    SELECT _user_id, _browser, _ip_address, NOW(), _culture
    RETURNING auth.sign_ins.sign_in_id INTO _sign_in_id;

    UPDATE auth.fb_access_tokens
    SET token = _token
    WHERE fb_user_id = _fb_user_id;

    RETURN QUERY
    SELECT _sign_in_id, true, 'Welcome'::text;
    RETURN;    
END
$$
LANGUAGE plpgsql;

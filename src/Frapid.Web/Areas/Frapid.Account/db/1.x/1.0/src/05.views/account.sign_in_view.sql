DROP VIEW IF EXISTS account.sign_in_view;

CREATE VIEW account.sign_in_view
AS
SELECT
    account.logins.login_id,
    account.users.email,
    account.logins.user_id,
    account.roles.role_id,
    account.roles.role_name,
    account.roles.is_administrator,
    account.logins.browser,
    account.logins.ip_address,
    account.logins.login_timestamp,
    account.logins.culture,
    account.logins.office_id,
    config.offices.office_name,
    config.offices.office_code || ' (' || config.offices.office_name || ')' AS office
FROM account.logins
INNER JOIN account.users
ON account.users.user_id = account.logins.user_id
INNER JOIN account.roles
ON account.roles.role_id = account.users.role_id
INNER JOIN config.offices
ON config.offices.office_id = account.logins.office_id;

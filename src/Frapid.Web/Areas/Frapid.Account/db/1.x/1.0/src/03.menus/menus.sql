SELECT * FROM config.create_app('Frapid.Account', 'Account', '1.0', 'MixERP Inc.', 'December 1, 2015', 'grey lock', '/dashboard/account/configuration-profile', '{Frapid.WebsiteBuilder}'::text[]);

SELECT * FROM config.create_menu('Frapid.Account', 'Roles', '/dashboard/account/roles', 'users', '');
SELECT * FROM config.create_menu('Frapid.Account', 'User Management', '/dashboard/account/user-management', 'user', '');
SELECT * FROM config.create_menu('Frapid.Account', 'Configuration Profile', '/dashboard/account/configuration-profile', 'configure', '');
SELECT * FROM config.create_menu('Frapid.Account', 'Email Templates', '', 'mail', '');
SELECT * FROM config.create_menu('Frapid.Account', 'Account Verification', '/dashboard/account/email-templates/account-verification', 'checkmark box', 'Email Templates');
SELECT * FROM config.create_menu('Frapid.Account', 'Password Reset', '/dashboard/account/email-templates/password-reset', 'key', 'Email Templates');
SELECT * FROM config.create_menu('Frapid.Account', 'Welcome Email', '/dashboard/account/email-templates/welcome-email', 'star', 'Email Templates');
SELECT * FROM config.create_menu('Frapid.Account', 'Welcome Email (3rd Party)', '/dashboard/account/email-templates/welcome-email-other', 'star outline', 'Email Templates');

SELECT * FROM core.create_app('Frapid.Account', 'Account', '1.0', 'MixERP Inc.', 'December 1, 2015', 'grey lock', '/dashboard/account/configuration-profile', NULL::text[]);

SELECT * FROM core.create_menu('Frapid.Account', 'Roles', '/dashboard/account/roles', 'users', '');
SELECT * FROM core.create_menu('Frapid.Account', 'Users', '', 'user', '');
SELECT * FROM core.create_menu('Frapid.Account', 'Add New User', '/dashboard/account/user/add', 'user', 'Users');
SELECT * FROM core.create_menu('Frapid.Account', 'Change Password', '/dashboard/account/user/change-password', 'user', 'Users');
SELECT * FROM core.create_menu('Frapid.Account', 'List Users', '/dashboard/account/user/list', 'user', 'Users');

SELECT * FROM core.create_menu('Frapid.Account', 'Configuration Profile', '/dashboard/account/configuration-profile', 'configure', '');
SELECT * FROM core.create_menu('Frapid.Account', 'Email Templates', '', 'mail', '');
SELECT * FROM core.create_menu('Frapid.Account', 'Account Verification', '/dashboard/account/email-templates/account-verification', 'checkmark box', 'Email Templates');
SELECT * FROM core.create_menu('Frapid.Account', 'Password Reset', '/dashboard/account/email-templates/password-reset', 'key', 'Email Templates');
SELECT * FROM core.create_menu('Frapid.Account', 'Welcome Email', '/dashboard/account/email-templates/welcome-email', 'star', 'Email Templates');
SELECT * FROM core.create_menu('Frapid.Account', 'Welcome Email (3rd Party)', '/dashboard/account/email-templates/welcome-email-other', 'star outline', 'Email Templates');

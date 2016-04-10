EXECUTE core.create_app 'Frapid.Account', 'Account', '1.0', 'MixERP Inc.', 'December 1, 2015', 'grey lock', '/dashboard/account/configuration-profile', NULL;

EXECUTE core.create_menu 'Frapid.Account', 'Roles', '/dashboard/account/roles', 'users', '';
EXECUTE core.create_menu 'Frapid.Account', 'User Management', '/dashboard/account/user-management', 'user', '';
EXECUTE core.create_menu 'Frapid.Account', 'Configuration Profile', '/dashboard/account/configuration-profile', 'configure', '';
EXECUTE core.create_menu 'Frapid.Account', 'Email Templates', '', 'mail', '';
EXECUTE core.create_menu 'Frapid.Account', 'Account Verification', '/dashboard/account/email-templates/account-verification', 'checkmark box', 'Email Templates';
EXECUTE core.create_menu 'Frapid.Account', 'Password Reset', '/dashboard/account/email-templates/password-reset', 'key', 'Email Templates';
EXECUTE core.create_menu 'Frapid.Account', 'Welcome Email', '/dashboard/account/email-templates/welcome-email', 'star', 'Email Templates';
EXECUTE core.create_menu 'Frapid.Account', 'Welcome Email (3rd Party)', '/dashboard/account/email-templates/welcome-email-other', 'star outline', 'Email Templates';

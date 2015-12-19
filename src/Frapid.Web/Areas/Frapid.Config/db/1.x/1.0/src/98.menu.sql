SELECT * FROM config.create_app('Frapid.Config', 'Config', '1.0', 'MixERP Inc.', 'December 1, 2015', 'orange configure', '/dashboard/config/offices', null);

SELECT * FROM config.create_menu('Frapid.Config', 'Office', '/dashboard/config/offices', 'building outline', '');
SELECT * FROM config.create_menu('Frapid.Config', 'Currency', '/dashboard/config/currencies', 'money', '');
SELECT * FROM config.create_menu('Frapid.Config', 'Flags', '/dashboard/config/flags', 'flag', '');
SELECT * FROM config.create_menu('Frapid.Config', 'SMTP', '/dashboard/config/smtp', 'at', '');
SELECT * FROM config.create_menu('Frapid.Config', 'Menu Access', '/dashboard/config/menu-access', 'lock', '');
SELECT * FROM config.create_menu('Frapid.Config', 'API Access', '/dashboard/config/api-access', 'key', '');

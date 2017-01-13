EXECUTE core.create_app 'Frapid.Config', 'Config', '1.0', 'MixERP Inc.', 'December 1, 2015', 'orange configure', '/dashboard/config/offices', null;
EXECUTE core.create_menu 'Frapid.Config', 'Offices', '/dashboard/config/offices', 'building outline', '';
EXECUTE core.create_menu 'Frapid.Config', 'SMTP', '/dashboard/config/smtp', 'at', '';
EXECUTE core.create_menu 'Frapid.Config', 'File Manager', '/dashboard/config/file-manager', 'file national character varying(500) outline', '';

GO

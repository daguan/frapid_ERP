EXECUTE core.create_app 'Frapid.Authorization', 'Authorization', '1.0', 'MixERP Inc.', 'December 1, 2015', 'purple privacy', '/dashboard/authorization/menu-access/group-policy', '{Frapid.Account}';



EXECUTE core.create_menu 'Frapid.Authorization', 'Entity Access Policy', '', 'lock', '';
EXECUTE core.create_menu 'Frapid.Authorization', 'Group Entity Access Policy', '/dashboard/authorization/entity-access/group-policy', 'users', 'Entity Access Policy';
EXECUTE core.create_menu 'Frapid.Authorization', 'User Entity Access Policy', '/dashboard/authorization/entity-access/user-policy', 'user', 'Entity Access Policy';
EXECUTE core.create_menu 'Frapid.Authorization', 'Menu Access Policy', '', 'toggle on', '';
EXECUTE core.create_menu 'Frapid.Authorization', 'Group Policy', '/dashboard/authorization/menu-access/group-policy', 'users', 'Menu Access Policy';
EXECUTE core.create_menu 'Frapid.Authorization', 'User Policy', '/dashboard/authorization/menu-access/user-policy', 'user', 'Menu Access Policy';

GO

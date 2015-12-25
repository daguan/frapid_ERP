SELECT * FROM core.create_app('Frapid.Authorization', 'Authorization', '1.0', 'MixERP Inc.', 'December 1, 2015', 'grey lock', '/dashboard/authorization/menu-access/group-policy', '{Frapid.Account}'::text[]);

SELECT * FROM core.create_menu('Frapid.Authorization', 'Entity Access Policy', '', 'lock', '');
SELECT * FROM core.create_menu('Frapid.Authorization', 'Group Policy', '/dashboard/authorization/entity-access/group-policy', 'users', 'Entity Access Policy');
SELECT * FROM core.create_menu('Frapid.Authorization', 'User Policy', '/dashboard/authorization/entity-access/user-policy', 'user', 'Entity Access Policy');
SELECT * FROM core.create_menu('Frapid.Authorization', 'Menu Access Policy', '', 'toggle on', '');
SELECT * FROM core.create_menu('Frapid.Authorization', 'Group Policy', '/dashboard/authorization/menu-access/group-policy', 'users', 'Menu Access Policy');
SELECT * FROM core.create_menu('Frapid.Authorization', 'User Policy', '/dashboard/authorization/menu-access/user-policy', 'user', 'Menu Access Policy');


SELECT * FROM auth.create_app_menu_policy
(
    'Administrator', 
    core.get_office_id_by_office_name('Default'), 
    'Frapid.Authorization',
    '{*}'::text[]
);

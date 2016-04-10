SELECT * FROM auth.create_app_menu_policy
(
    'User', 
    core.get_office_id_by_office_name('Default'), 
    'Frapid.Config',
    '{Offices, Flags}'::text[]
);

SELECT * FROM auth.create_app_menu_policy
(
    'Admin', 
    core.get_office_id_by_office_name('Default'), 
    'Frapid.Config',
    '{*}'::text[]
);

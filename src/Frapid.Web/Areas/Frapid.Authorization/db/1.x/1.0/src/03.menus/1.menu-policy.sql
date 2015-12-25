SELECT * FROM auth.create_app_menu_policy
(
    'Administrator', 
    core.get_office_id_by_office_name('Default'), 
    'Frapid.Authorization',
    '{*}'::text[]
);

DELETE FROM auth.menu_access_policy
WHERE menu_id IN
(
    SELECT menu_id FROM core.menus
    WHERE app_name = 'Social'
);

DELETE FROM auth.group_menu_access_policy
WHERE menu_id IN
(
    SELECT menu_id FROM core.menus
    WHERE app_name = 'Social'
);

DELETE FROM core.menus
WHERE app_name = 'Social';


EXECUTE core.create_app 'Social', 'Social', '1.0', 'MixERP Inc.', 'December 1, 2015', 'orange users', '/dashboard/social', NULL;

EXECUTE core.create_menu 'Social', 'Tasks', '', 'lightning', '';
EXECUTE core.create_menu 'Social', 'Home', '/dashboard/social', 'users', 'Tasks';
EXECUTE core.create_menu 'Social', 'Edit Profile', '/dashboard/social/edit-profile', 'photo', 'Tasks';

DECLARE @office_id integer = core.get_office_id_by_office_name('Default');
EXECUTE auth.create_app_menu_policy
'Admin', 
@office_id, 
'Social',
'{*}';


GO

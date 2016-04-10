EXECUTE core.create_app 'Frapid.WebsiteBuilder', 'Website', '1.0', 'MixERP Inc.', 'December 1, 2015', 'world blue', '/dashboard/website/contents', null;

EXECUTE core.create_menu 'Frapid.WebsiteBuilder', 'Tasks', '', 'tasks icon', '';
EXECUTE core.create_menu 'Frapid.WebsiteBuilder', 'Configuration', '/dashboard/website/configuration', 'configure icon', 'Tasks';
EXECUTE core.create_menu 'Frapid.WebsiteBuilder', 'Manage Categories', '/dashboard/website/categories', 'sitemap icon', 'Tasks';
EXECUTE core.create_menu 'Frapid.WebsiteBuilder', 'Add New Content', '/dashboard/website/contents/new', 'file', 'Tasks';
EXECUTE core.create_menu 'Frapid.WebsiteBuilder', 'View Contents', '/dashboard/website/contents', 'desktop', 'Tasks';
EXECUTE core.create_menu 'Frapid.WebsiteBuilder', 'Menus', '/dashboard/website/menus', 'star', 'Tasks';
EXECUTE core.create_menu 'Frapid.WebsiteBuilder', 'Contacts', '/dashboard/website/contacts', 'phone', 'Tasks';
EXECUTE core.create_menu 'Frapid.WebsiteBuilder', 'Subscriptions', '/dashboard/website/subscriptions', 'newspaper', 'Tasks';
EXECUTE core.create_menu 'Frapid.WebsiteBuilder', 'Layout Manager', '/dashboard/website/layouts', 'grid layout', '';
EXECUTE core.create_menu 'Frapid.WebsiteBuilder', 'Email Templates', '', 'mail', '';
EXECUTE core.create_menu 'Frapid.WebsiteBuilder', 'Subscription Added', '/dashboard/website/subscription/welcome', 'plus circle', 'Email Templates';
EXECUTE core.create_menu 'Frapid.WebsiteBuilder', 'Subscription Removed', '/dashboard/website/subscription/removed', 'minus circle', 'Email Templates';

GO

DECLARE @office_id integer = core.get_office_id_by_office_name('Default');

EXECUTE auth.create_app_menu_policy
'Content Editor', 
@office_id, 
'Frapid.WebsiteBuilder',
'{Tasks, Manage Categories, Add New Content, View Contents}';

EXECUTE auth.create_app_menu_policy
'User', 
@office_id, 
'Frapid.WebsiteBuilder',
'{
    Tasks, Manage Categories, Add New Content, View Contents, 
    Menus, Contacts, Subscriptions, 
    Layout Manager, Edit Master Layout (Homepage), Edit Master Layout, Edit Header, Edit Footer, 404 Not Found Document
}';


EXECUTE auth.create_app_menu_policy
'Admin', 
@office_id, 
'Frapid.WebsiteBuilder',
'{*}';



GO

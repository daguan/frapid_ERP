SELECT * FROM core.create_app('Frapid.WebsiteBuilder', 'Website', '1.0', 'MixERP Inc.', 'December 1, 2015', 'world blue', '/dashboard/website/contents', null);

SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Tasks', '', 'tasks icon', '');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Configuration', '/dashboard/website/configuration', 'configure icon', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Manage Categories', '/dashboard/website/categories', 'sitemap icon', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Add New Content', '/dashboard/website/contents/new', 'file', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'View Contents', '/dashboard/website/contents', 'desktop', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Menus', '/dashboard/website/menus', 'star', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Contacts', '/dashboard/website/contacts', 'phone', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Subscriptions', '/dashboard/website/subscriptions', 'newspaper', 'Tasks');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Layout Manager', '/dashboard/website/layouts', 'grid layout', '');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Email Templates', '', 'mail', '');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Subscription Added', '/dashboard/website/subscription/welcome', 'plus circle', 'Email Templates');
SELECT * FROM core.create_menu('Frapid.WebsiteBuilder', 'Subscription Removed', '/dashboard/website/subscription/removed', 'minus circle', 'Email Templates');


SELECT * FROM auth.create_app_menu_policy
(
    'Content Editor', 
    core.get_office_id_by_office_name('Default'), 
    'Frapid.WebsiteBuilder',
    '{Tasks, Manage Categories, Add New Content, View Contents}'::text[]
);

SELECT * FROM auth.create_app_menu_policy
(
    'User', 
    core.get_office_id_by_office_name('Default'), 
    'Frapid.WebsiteBuilder',
    '{
        Tasks, Manage Categories, Add New Content, View Contents, 
        Menus, Contacts, Subscriptions, 
        Layout Manager, Edit Master Layout (Homepage), Edit Master Layout, Edit Header, Edit Footer, 404 Not Found Document
    }'::text[]
);


SELECT * FROM auth.create_app_menu_policy
(
    'Admin', 
    core.get_office_id_by_office_name('Default'), 
    'Frapid.WebsiteBuilder',
    '{*}'::text[]
);

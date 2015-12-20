SELECT * FROM config.create_app('Frapid.WebsiteBuilder', 'Website', '1.0', 'MixERP Inc.', 'December 1, 2015', 'world blue', '/dashboard/website/contents', null);

SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Tasks', '', 'tasks icon', '');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Manage Categories', '/dashboard/website/categories', 'sitemap icon', 'Tasks');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Add New Content', '/dashboard/website/contents/new', 'file', 'Tasks');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'View Contents', '/dashboard/website/contents', 'desktop', 'Tasks');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Menus', '/dashboard/website/menus', 'star', 'Tasks');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Contacts', '/dashboard/website/contacts', 'phone', 'Tasks');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Subscriptions', '/dashboard/website/subscriptions', 'newspaper', 'Tasks');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Layout Manager', '', 'grid layout', '');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Edit Master Layout (Homepage)', '/dashboard/website/layouts/master/home', 'block layout', 'Layout Manager');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Edit Master Layout', '/dashboard/website/layouts/master', 'block layout', 'Layout Manager');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Edit Header', '/dashboard/website/layouts/header', 'arrow circle outline up', 'Layout Manager');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Edit Footer', '/dashboard/website/layouts/footer', 'arrow circle outline down', 'Layout Manager');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', '404 Not Found Document', '/dashboard/website/layouts/404-not-found-document', 'warning circle', 'Layout Manager');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Email Templates', '', 'mail', '');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Subscription Added', '/dashboard/website/subscription/welcome', 'plus circle', 'Email Templates');
SELECT * FROM config.create_menu('Frapid.WebsiteBuilder', 'Subscription Removed', '/dashboard/website/subscription/removed', 'minus circle', 'Email Templates');

INSERT INTO account.roles
SELECT     1,   'Guest',    false UNION ALL
SELECT    10,   'Client',   false UNION ALL
SELECT   100,   'Partner',  false UNION ALL
SELECT  1000,   'User',     false UNION ALL
SELECT 10000,   'Admin',    true;


INSERT INTO account.configuration_profiles(profile_name, is_active, allow_registration, registration_role_id, registration_office_id)
SELECT 'Default', true, true, 1, 1;

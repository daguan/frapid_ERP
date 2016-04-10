IF NOT EXISTS 
(
	SELECT 1
	FROM master.sys.server_principals
	WHERE name = 'frapid_db_user'
)
BEGIN
    CREATE LOGIN frapid_db_user WITH PASSWORD = N'change-on-deployment';
END;

IF NOT EXISTS 
(
	SELECT 1
	FROM master.sys.server_principals
	WHERE name = 'report_user'
)
BEGIN
    CREATE LOGIN report_user WITH PASSWORD = N'change-on-deployment';
END;
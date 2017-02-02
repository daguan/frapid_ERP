-->-->-- src/Frapid.Web/Areas/Frapid.Calendar/db/Sql Server/2.x/2.0/src/01.types-domains-tables-and-constraints/tables-and-constraints.sql --<--<--


-->-->-- src/Frapid.Web/Areas/Frapid.Calendar/db/Sql Server/2.x/2.0/src/03.menus/menus.sql --<--<--


-->-->-- src/Frapid.Web/Areas/Frapid.Calendar/db/Sql Server/2.x/2.0/src/04.default-values/01.default-values.sql --<--<--


-->-->-- src/Frapid.Web/Areas/Frapid.Calendar/db/Sql Server/2.x/2.0/src/99.ownership.sql --<--<--
EXEC sp_addrolemember  @rolename = 'db_owner', @membername  = 'frapid_db_user'
GO

EXEC sp_addrolemember  @rolename = 'db_datareader', @membername  = 'report_user'
GO

DECLARE @proc sysname
DECLARE @cmd varchar(8000)

DECLARE cur CURSOR FOR 
SELECT '[' + schema_name(schema_id) + '].[' + name + ']' FROM sys.objects
WHERE type IN('FN')
AND is_ms_shipped = 0
ORDER BY 1
OPEN cur
FETCH next from cur into @proc
WHILE @@FETCH_STATUS = 0
BEGIN
     SET @cmd = 'GRANT EXEC ON ' + @proc + ' TO report_user';
     EXEC (@cmd)

     FETCH next from cur into @proc
END
CLOSE cur
DEALLOCATE cur

GO


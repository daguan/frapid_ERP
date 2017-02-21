-->-->-- src/Frapid.Web/Areas/Frapid.AddressBook/db/Sql Server/2.x/2.0/src/01.types-domains-tables-and-constraints/tables-and-constraints.sql --<--<--
EXECUTE dbo.drop_schema 'calendar';
GO
CREATE SCHEMA calendar;

GO
CREATE TABLE calendar.categories
(
	category_id								integer IDENTITY PRIMARY KEY,
	user_id									integer NOT NULL REFERENCES account.users,
	category_name							national character varying(200) NOT NULL,
	color_code								national character varying(50) NOT NULL,
	is_local								bit NOT NULL DEFAULT(1),
	category_order							smallint,
    audit_user_id                           integer REFERENCES account.users,
    audit_ts                                DATETIMEOFFSET NULL DEFAULT(GETUTCDATE()),
    deleted                                 bit DEFAULT(0)  
);

CREATE UNIQUE INDEX categories_user_id_category_name_uix
ON calendar.categories(user_id, category_name)
WHERE deleted = 0;

CREATE TABLE calendar.events
(
	event_id								uniqueidentifier PRIMARY KEY DEFAULT(NEWSEQUENTIALID()),
	category_id								integer NOT NULL REFERENCES calendar.categories,
	user_id									integer NOT NULL REFERENCES account.users,
	name									national character varying(500) NOT NULL,
	location								national character varying(2000),
	starts_at								DATETIMEOFFSET NOT NULL,
	ends_on									DATETIMEOFFSET NOT NULL,
	time_zone								national character varying(200) NOT NULL,
	all_day									bit NOT NULL DEFAULT(0),
	recurrence								national character varying(MAX),--JSON data
	until									DATETIMEOFFSET,
	alarm									integer,--minutes before
	reminder_types							national character varying(MAX),--JSON data
	is_private								bit,
	url										national character varying(500),
	note									national character varying(MAX),
    audit_user_id                           integer REFERENCES account.users,
    audit_ts                                DATETIMEOFFSET NULL DEFAULT(GETUTCDATE()),
    deleted                                 bit DEFAULT(0)
);

GO


-->-->-- src/Frapid.Web/Areas/Frapid.AddressBook/db/Sql Server/2.x/2.0/src/03.menus/menus.sql --<--<--
DELETE FROM auth.menu_access_policy
WHERE menu_id IN
(
 SELECT menu_id FROM core.menus
 WHERE app_name = 'Calendar'
);

DELETE FROM auth.group_menu_access_policy
WHERE menu_id IN
(
 SELECT menu_id FROM core.menus
 WHERE app_name = 'Calendar'
);

DELETE FROM core.menus
WHERE app_name = 'Calendar';


EXECUTE core.create_app 'Calendar', 'Calendar', 'Calendar', '1.0', 'MixERP Inc.', 'December 1, 2015', 'violet calendar', '/dashboard/calendar', NULL;

EXECUTE core.create_menu 'Calendar', 'Tasks', 'Tasks', '', 'lightning', '';
EXECUTE core.create_menu 'Calendar', 'Calendar', 'Calendar', '/dashboard/calendar', 'calendar', 'Tasks';


GO
DECLARE @office_id integer  = core.get_office_id_by_office_name('Default');

EXECUTE auth.create_app_menu_policy
'Admin', 
@office_id, 
'Calendar',
'{*}';

GO


-->-->-- src/Frapid.Web/Areas/Frapid.AddressBook/db/Sql Server/2.x/2.0/src/04.default-values/01.default-values.sql --<--<--


-->-->-- src/Frapid.Web/Areas/Frapid.AddressBook/db/Sql Server/2.x/2.0/src/99.ownership.sql --<--<--
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


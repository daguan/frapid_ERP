-->-->-- src/Frapid.Web/db/SQL Server/meta/1.x/1.0/src/00.db core/sql-server-roles.sql --<--<--
IF NOT EXISTS 
(
	SELECT 1
	FROM master.sys.server_principals
	WHERE name = 'frapid_db_user'
)
BEGIN
    CREATE LOGIN frapid_db_user WITH PASSWORD = N'change-on-deployment@123';
END;

IF NOT EXISTS 
(
	SELECT 1
	FROM master.sys.server_principals
	WHERE name = 'report_user'
)
BEGIN
    CREATE LOGIN report_user WITH PASSWORD = N'change-on-deployment@123';
END;

IF NOT EXISTS 
(
	SELECT * FROM sys.database_principals 
	WHERE name = 'frapid_db_user'
)
BEGIN
    CREATE USER frapid_db_user FOR LOGIN frapid_db_user;
    EXEC sp_addrolemember 'db_datareader', 'frapid_db_user';
    EXEC sp_addrolemember 'db_datawriter', 'frapid_db_user';
END;
GO


-->-->-- src/Frapid.Web/db/SQL Server/meta/1.x/1.0/src/01.types-domains-tables-and-constraints/tables-and-constraints.sql --<--<--
IF NOT EXISTS
(
	SELECT * FROM sys.schemas
	WHERE name = 'i18n'
)
BEGIN
	EXEC('CREATE SCHEMA i18n');
END;

GO


CREATE TABLE i18n.resources
(
  resource_id                                   int IDENTITY PRIMARY KEY,
  resource_class                                national character varying(4000),
  [key]                                         national character varying(4000),
  value                                         national character varying(MAX)
);

CREATE UNIQUE INDEX resources_class_key
ON i18n.resources(resource_class, [key]);

CREATE TABLE i18n.localized_resources
(
    localized_resource_id                       bigint IDENTITY PRIMARY KEY,
    resource_id                                 integer NOT NULL REFERENCES i18n.resources,
    culture_code                                national character varying(5) NOT NULL,
    value                                       national character varying(4000)
);


GO

-->-->-- src/Frapid.Web/db/SQL Server/meta/1.x/1.0/src/05.views/0.resource_view.sql --<--<--
GO
IF OBJECT_ID('i18n.resource_view') IS NOT NULL
DROP VIEW i18n.resource_view;

GO

CREATE VIEW i18n.resource_view
AS
SELECT 
    resource_class, '' as culture, [key], value
FROM i18n.resources
UNION ALL
SELECT resource_class, culture_code, [key], i18n.localized_resources.value 
FROM i18n.localized_resources
INNER JOIN i18n.resources
ON i18n.localized_resources.resource_id = i18n.resources.resource_id;


GO


-->-->-- src/Frapid.Web/db/SQL Server/meta/1.x/1.0/src/05.views/localized_resource_view.sql --<--<--
IF OBJECT_ID('i18n.localized_resource_view') IS NOT NULL
DROP VIEW i18n.localized_resource_view;

GO

CREATE VIEW i18n.localized_resource_view
AS
SELECT
    resource_class + 
    CASE WHEN COALESCE(culture, '') = '' THEN '' ELSE '.' + culture END 
    + '.' + [key] as [key], value 
FROM i18n.resource_view;


GO


-->-->-- src/Frapid.Web/db/SQL Server/meta/1.x/1.0/src/06.functions-and-logic/add_localized_resource.sql --<--<--
IF OBJECT_ID('i18n.add_localized_resource') IS NOT NULL
DROP PROCEDURE i18n.add_localized_resource;

GO

CREATE PROCEDURE i18n.add_localized_resource
(
    @resource_class		national character varying(4000),
    @culture_code		national character varying(4000),
    @key				national character varying(4000),
    @value				national character varying(4000)
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @resource_id    integer;

    BEGIN TRY
        DECLARE @tran_count int = @@TRANCOUNT;
        
        IF(@tran_count= 0)
        BEGIN
            BEGIN TRANSACTION
        END;

        IF(COALESCE(@culture_code, '') = '')
        BEGIN
            EXECUTE i18n.add_resource @resource_class, @key, @value;
            RETURN;
        END;
           
        SELECT @resource_id = resource_id
        FROM i18n.resources
        WHERE resource_class = @resource_class
        AND [key] = @key;

        IF(@resource_id IS NOT NULL)
        BEGIN
            IF EXISTS
            (
                SELECT 1 FROM i18n.localized_resources 
                WHERE i18n.localized_resources.resource_id=@resource_id
                AND culture_code = @culture_code
            )
            BEGIN
                UPDATE i18n.localized_resources
                SET value=@value
                WHERE i18n.localized_resources.resource_id=@resource_id
                AND culture_code = @culture_code;

                RETURN;
            END;

            INSERT INTO i18n.localized_resources(resource_id, culture_code, value)
            SELECT @resource_id, @culture_code, @value;
        END;

        IF(@tran_count = 0)
        BEGIN
            COMMIT TRANSACTION;
        END;
    END TRY
    BEGIN CATCH
        IF(XACT_STATE() <> 0 AND @tran_count = 0) 
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        DECLARE @ErrorMessage national character varying(4000)  = ERROR_MESSAGE();
        DECLARE @ErrorSeverity int                              = ERROR_SEVERITY();
        DECLARE @ErrorState int                                 = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;

GO

-->-->-- src/Frapid.Web/db/SQL Server/meta/1.x/1.0/src/06.functions-and-logic/add_resource.sql --<--<--
IF OBJECT_ID('i18n.add_resource') IS NOT NULL
DROP PROCEDURE i18n.add_resource;

GO


CREATE PROCEDURE i18n.add_resource
(
    @resource_class		national character varying(4000),
    @key				national character varying(4000),
    @value				national character varying(4000)
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS
    (
		SELECT 1 
		FROM i18n.resources 
		WHERE i18n.resources.resource_class=@resource_class 
		AND i18n.resources.[key]=@key
	)
	BEGIN
        INSERT INTO i18n.resources(resource_class, [key], value)
        SELECT @resource_class, @key, @value;
    END;
END;


GO

-->-->-- src/Frapid.Web/db/SQL Server/meta/1.x/1.0/src/06.functions-and-logic/get_localization_table.sql --<--<--
IF OBJECT_ID('i18n.get_localization_table') IS NOT NULL
DROP FUNCTION i18n.get_localization_table;

GO

CREATE FUNCTION i18n.get_localization_table
(
    @culture			national character varying(4000)
)
RETURNS @result TABLE
(
    id                  bigint,
    resource_class      national character varying(4000),
    [key]               national character varying(4000),
    original            national character varying(4000),
    translated          national character varying(4000)
)
AS
BEGIN       
    INSERT INTO @result
    SELECT
        i18n.resources.resource_id, 
        i18n.resources.resource_class, 
        i18n.resources.[key], 
        i18n.resources.value,
        ''
    FROM i18n.resources;
	
	RETURN;
END;

GO


-->-->-- src/Frapid.Web/db/SQL Server/meta/1.x/1.0/src/06.functions-and-logic/get_output_for.sql --<--<--
IF OBJECT_ID('i18n.get_output_for') IS NOT NULL
DROP FUNCTION i18n.get_output_for;

GO

CREATE FUNCTION i18n.get_output_for(@culture national character varying(3))
RETURNS national character varying(MAX)
AS
BEGIN
	DECLARE @resources national character varying(MAX) = '';
	
    SELECT
        @resources = COALESCE(@resources + CHAR(13), '') +
        'SELECT * FROM i18n.add_localized_resource(''' +
        i18n.resources.resource_class + ''', ''' + @culture + ''', ''' +
        i18n.resources.[key] + ''', ''' +
        REPLACE(i18n.localized_resources.value, '''', '''''') + ''');--' +
        i18n.resources.value
    FROM i18n.localized_resources
    LEFT JOIN i18n.resources
    ON i18n.localized_resources.resource_id = i18n.resources.resource_id
    WHERE culture_code = @culture
    ORDER BY i18n.resources.resource_class, i18n.resources.[key];
    
    RETURN @resources;
END;

GO


-->-->-- src/Frapid.Web/db/SQL Server/meta/1.x/1.0/src/06.functions-and-logic/get_resource.sql --<--<--
IF OBJECT_ID('i18n.get_resource') IS NOT NULL
DROP FUNCTION i18n.get_resource;

GO

CREATE FUNCTION i18n.get_resource
(
	@culture_code national character varying(4000), 
	@resource_class national character varying(4000), 
	@key national character varying(4000)
)
RETURNS national character varying(4000)
AS
BEGIN
    DECLARE @resource_id    integer;
    DECLARE @resource       national character varying(4000);
    DECLARE @value          national character varying(4000);

    SELECT 
        @resource_id = resource_id,
        @resource = value
    FROM i18n.resources
    WHERE resource_class = @resource_class
    AND [key] = @key;

    SELECT
        @value = value
    FROM i18n.localized_resources
    WHERE culture_code = @culture_code
    AND resource_id = @resource_id;


    SET @resource = COALESCE(@value, @resource);
    
    RETURN @resource;
END;

GO


-->-->-- src/Frapid.Web/db/SQL Server/meta/1.x/1.0/src/10.Localization/0.neutral-resource(en)/language.sql --<--<--
EXECUTE i18n.add_localized_resource 'Titles', '', 'SignIn', 'Sign In';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Username', 'Username';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Password', 'Password';
EXECUTE i18n.add_localized_resource 'Titles', '', 'ReturnToWebsite', 'Return to Website';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Or', 'Or';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Add', 'Add';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Save', 'Save';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Actions', 'Actions';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Edit', 'Edit';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Delete', 'Delete';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Update', 'Update';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Select', 'Select';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Approve', 'Approve';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Reject', 'Reject';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Close', 'Close';
EXECUTE i18n.add_localized_resource 'Titles', '', 'ReturnToView', 'Return to View';
EXECUTE i18n.add_localized_resource 'Titles', '', 'CreateDuplicate', 'Create Duplicate';
EXECUTE i18n.add_localized_resource 'Titles', '', 'None', 'None';
EXECUTE i18n.add_localized_resource 'Titles', '', 'First', 'First';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Previous', 'Previous';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Next', 'Next';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Last', 'Last';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Loading', 'Loading';
EXECUTE i18n.add_localized_resource 'Titles', '', 'CreateAFlag', 'Create a Flag';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Update', 'Update';
EXECUTE i18n.add_localized_resource 'Titles', '', 'CreateNew', 'Create New';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Verification', 'Verification';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Reason', 'Reason';
EXECUTE i18n.add_localized_resource 'Titles', '', 'AddAKanbanList', 'Add a Kanban List';
EXECUTE i18n.add_localized_resource 'Titles', '', 'KanbanId', 'KanbanId';
EXECUTE i18n.add_localized_resource 'Titles', '', 'KanbanName', 'KanbanName';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Description', 'Description';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Cancel', 'Cancel';
EXECUTE i18n.add_localized_resource 'Titles', '', 'OK', 'OK';
EXECUTE i18n.add_localized_resource 'Titles', '', 'AddNew', 'Add New';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Flag', 'Flag';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Filter', 'Filter';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Export', 'Export';
EXECUTE i18n.add_localized_resource 'Titles', '', 'ExportThisDocument', 'Export This Document';
EXECUTE i18n.add_localized_resource 'Titles', '', 'ExportToDoc', 'Export to Doc';
EXECUTE i18n.add_localized_resource 'Titles', '', 'ExportToExcel', 'Export to Excel';
EXECUTE i18n.add_localized_resource 'Titles', '', 'ExportToPDF', 'Export to PDF';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Print', 'Print';
EXECUTE i18n.add_localized_resource 'Titles', '', 'SelectAColumn', 'Select a Column';
EXECUTE i18n.add_localized_resource 'Titles', '', 'FilterCondition', 'Filter Condition';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Value', 'Value';
EXECUTE i18n.add_localized_resource 'Titles', '', 'And', 'And';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Add', 'Add';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Actions', 'Actions';
EXECUTE i18n.add_localized_resource 'Titles', '', 'ColumnName', 'Column Name';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Condition', 'Condition';
EXECUTE i18n.add_localized_resource 'Titles', '', 'SaveThisFilter', 'Save This Filter';
EXECUTE i18n.add_localized_resource 'Titles', '', 'FilterName', 'Filter Name';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Save', 'Save';
EXECUTE i18n.add_localized_resource 'Titles', '', 'ManageFilters', 'Manage Filters';
EXECUTE i18n.add_localized_resource 'Titles', '', 'RemoveAsDefault', 'Remove As Default';
EXECUTE i18n.add_localized_resource 'Titles', '', 'MakeAsDefault', 'Make As Default';
EXECUTE i18n.add_localized_resource 'Titles', '', 'DataImport', 'Data Import';
EXECUTE i18n.add_localized_resource 'Titles', '', 'ExportData', 'Export Data';
EXECUTE i18n.add_localized_resource 'Titles', '', 'ImportData', 'Import Data';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Yes', 'Yes';
EXECUTE i18n.add_localized_resource 'Titles', '', 'No', 'No';
EXECUTE i18n.add_localized_resource 'Titles', '', 'View', 'View';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Rating', 'Rating';
EXECUTE i18n.add_localized_resource 'Titles', '', 'AddNewChecklist', 'Add New Checklist';
EXECUTE i18n.add_localized_resource 'Titles', '', 'EditThisChecklist', 'Edit This Checklist';
EXECUTE i18n.add_localized_resource 'Titles', '', 'DeleteThisChecklist', 'Delete This Checklist';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Verify', 'Verify';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Back', 'Back';
EXECUTE i18n.add_localized_resource 'Titles', '', 'PageN', 'Page {0}';
EXECUTE i18n.add_localized_resource 'Titles', '', 'SelectAFilter', 'Select a Filter';
EXECUTE i18n.add_localized_resource 'Titles', '', 'CustomFields', 'Custom Fields';
EXECUTE i18n.add_localized_resource 'Titles', '', 'Untitled', 'Untitled';
EXECUTE i18n.add_localized_resource 'Labels', '', 'UploadInvalidTryAgain', 'Your upload is of invalid file type "{0}". Please try again.';
EXECUTE i18n.add_localized_resource 'Labels', '', 'ProcessingYourCSVFile', 'Processing  your CSV file.';
EXECUTE i18n.add_localized_resource 'Labels', '', 'FlagSaved', 'Flag was saved.';
EXECUTE i18n.add_localized_resource 'Labels', '', 'SuccessfullyProcessedYourFile', 'Successfully processed your file.';
EXECUTE i18n.add_localized_resource 'Labels', '', 'FlagRemoved', 'Flag was removed.';
EXECUTE i18n.add_localized_resource 'Labels', '', 'RequestingImport', 'Requesting import. This may take several minutes to complete.';
EXECUTE i18n.add_localized_resource 'Labels', '', 'NoFormFound', 'No instance of form was found.';
EXECUTE i18n.add_localized_resource 'Labels', '', 'ImportedNItems', 'Successfully imported {0} items.';
EXECUTE i18n.add_localized_resource 'Labels', '', 'RollingBackChanges', 'Rolling back changes.';
EXECUTE i18n.add_localized_resource 'Warnings', '', 'InvalidFileExtension', 'Invalid file extension.';
EXECUTE i18n.add_localized_resource 'Questions', '', 'AreYouSure', 'Are you sure?';
EXECUTE i18n.add_localized_resource 'Questions', '', 'ColumnInvalidAreYouSure', 'The column "{0}" does not exist or is invalid. Are you sure you want to continue?';
EXECUTE i18n.add_localized_resource 'Questions', '', 'TaskCompletedSuccessfullyReturnToView', 'Task completed successfully. Return to view?';
EXECUTE i18n.add_localized_resource 'Labels', '', 'NHours', '{0} hour(s).';
EXECUTE i18n.add_localized_resource 'Labels', '', 'NMinutes', '{0} minute(s).';
EXECUTE i18n.add_localized_resource 'Labels', '', 'ItemDuplicated', 'Item duplicated.';
EXECUTE i18n.add_localized_resource 'Labels', '', 'TaskCompletedSuccessfully', 'Task completed successfully.';
EXECUTE i18n.add_localized_resource 'Labels', '', 'ThisFieldIsRequired', 'This field is required.';
EXECUTE i18n.add_localized_resource 'Labels', '', 'NamedFilter', 'Filter: {0}.';
EXECUTE i18n.add_localized_resource 'DbErrors', '', 'TableNotFound', 'The table was not found.';


-->-->-- src/Frapid.Web/db/SQL Server/meta/1.x/1.0/src/99.permission.sql --<--<--
GRANT EXECUTE ON SCHEMA::i18n TO frapid_db_user;


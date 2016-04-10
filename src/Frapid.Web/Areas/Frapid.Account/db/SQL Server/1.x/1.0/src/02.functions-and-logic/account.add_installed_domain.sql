IF OBJECT_ID('account.add_installed_domain') IS NOT NULL
DROP PROCEDURE account.add_installed_domain;

GO

CREATE PROCEDURE account.add_installed_domain
(
    @domain_name                                national character varying(500),
    @admin_email                                national character varying(500)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT * FROM account.installed_domains
        WHERE domain_name = @domain_name        
    )
    BEGIN
        UPDATE account.installed_domains
        SET admin_email = @admin_email
        WHERE domain_name = @domain_name;        
    END;

    INSERT INTO account.installed_domains(domain_name, admin_email)
    SELECT @domain_name, @admin_email;
END;


GO

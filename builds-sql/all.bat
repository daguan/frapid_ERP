@echo off
set current=%~dp0
set root="%current%..\src\Frapid.Web\"

call bunlder.bat %root%, "db\PostgreSQL\meta\1.x\1.0\", "rebundle.bat"
cd %current%
call bunlder.bat %root%, "db\SQL Server\meta\1.x\1.0\", "rebundle.bat"
cd %current%

call bunlder.bat %root%, "Areas\Frapid.Account\db\PostgreSQL\1.x\1.0\", "rebundle-db-without-sample.bat"
cd %current%
call bunlder.bat %root%, "Areas\Frapid.Account\db\SQL Server\1.x\1.0\", "rebundle-db-without-sample.bat"
cd %current%


call bunlder.bat %root%, "Areas\Frapid.Authorization\db\PostgreSQL\1.x\1.0\", "rebundle-db-without-sample.bat"
cd %current%
call bunlder.bat %root%, "Areas\Frapid.Authorization\db\SQL Server\1.x\1.0\", "rebundle-db-without-sample.bat"
cd %current%

call bunlder.bat %root%, "Areas\Frapid.Config\db\PostgreSQL\1.x\1.0\", "rebundle.bat"
cd %current%
call bunlder.bat %root%, "Areas\Frapid.Config\db\SQL Server\1.x\1.0\", "rebundle.bat"
cd %current%

call bunlder.bat %root%, "Areas\Frapid.Core\db\PostgreSQL\1.x\1.0\", "rebundle-db-without-sample.bat"
cd %current%
call bunlder.bat %root%, "Areas\Frapid.Core\db\SQL Server\1.x\1.0\", "rebundle-db-without-sample.bat"
cd %current%

call bunlder.bat %root%, "Areas\Frapid.Forms\db\PostgreSQL\1.x\1.0\", "rebundle.bat"
cd %current%

call bunlder.bat %root%, "Areas\Frapid.WebsiteBuilder\db\PostgreSQL\1.x\1.0\", "rebundle-db-with-sample.bat"
cd %current%
call bunlder.bat %root%, "Areas\Frapid.WebsiteBuilder\db\PostgreSQL\1.x\1.0\", "rebundle-db-without-sample.bat"
cd %current%
call bunlder.bat %root%, "Areas\Frapid.WebsiteBuilder\db\SQL Server\1.x\1.0\", "rebundle-db-with-sample.bat"
cd %current%
call bunlder.bat %root%, "Areas\Frapid.WebsiteBuilder\db\SQL Server\1.x\1.0\", "rebundle-db-without-sample.bat"
cd %current%

call bunlder.bat %root%, "Areas\MixERP.Finance\db\PostgreSQL\2.x\2.0\", "rebundle-db-with-sample.bat"
cd %current%
call bunlder.bat %root%, "Areas\MixERP.Finance\db\PostgreSQL\2.x\2.0\", "rebundle-db-without-sample.bat"
cd %current%
call bunlder.bat %root%, "Areas\MixERP.Forums\db\PostgreSQL\1.x\1.0\", "rebundle-db-with-sample.bat"
cd %current%

call bunlder.bat %root%, "Areas\MixERP.Forums\db\PostgreSQL\1.x\1.0\", "rebundle-db-without-sample.bat"
cd %current%

call bunlder.bat %root%, "Areas\MixERP.Helpdesk\db\PostgreSQL\1.x\1.0\", "rebundle-db-without-sample.bat"
cd %current%

call bunlder.bat %root%, "Areas\MixERP.HRM\db\PostgreSQL\2.x\2.0\db\", "rebundle-db-with-sample.bat"
cd %current%
call bunlder.bat %root%, "Areas\MixERP.HRM\db\PostgreSQL\2.x\2.0\db\", "rebundle-db-without-sample.bat"
cd %current%
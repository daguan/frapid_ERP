@echo off
SET builddir=%~dp0

cd %builddir%..\src\Frapid.Web\bin\

@echo Creating PostgreSQL Tenant
call frapid.exe create site postgresql.test provider Npgsql cleanup when done
call frapid.exe update database schema on site postgresql.test


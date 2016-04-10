@echo off
bundler\SqlBundler.exe ..\..\..\..\..\ "db/SQL Server/meta/1.x/1.0" false
copy meta.sql ..\..\..\meta.sql
pause
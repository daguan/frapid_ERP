@echo off
bundler\SqlBundler.exe ..\..\..\..\ "db/SQL Server/2.x/2.0" true
copy sparkpostmail.sql sparkpostmail-sample.sql
del sparkpostmail.sql
copy sparkpostmail-sample.sql ..\..\sparkpostmail-sample.sql
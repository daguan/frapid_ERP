@echo off
bundler\MixERP.Net.Utility.SqlBundler.exe ..\..\..\..\ "db/meta/1.x/1.0" false false
copy meta.sql ..\..\..\meta.sql
pause
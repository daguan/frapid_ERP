@echo off
bundler\MixERP.Net.Utility.SqlBundler.exe ..\..\..\..\ "db/core/1.x/1.0" false false
copy core.sql ..\..\..\core.sql
pause
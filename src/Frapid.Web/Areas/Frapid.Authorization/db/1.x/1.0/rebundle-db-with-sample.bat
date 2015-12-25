@echo off
bundler\MixERP.Net.Utility.SqlBundler.exe ..\..\..\ "db/1.x/1.0" false true
copy auth.sql auth-sample.sql
del auth.sql
copy auth-sample.sql ..\..\auth-sample.sql
pause
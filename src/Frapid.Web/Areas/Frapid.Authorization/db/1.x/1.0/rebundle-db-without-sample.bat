@echo off
bundler\MixERP.Net.Utility.SqlBundler.exe ..\..\..\ "db/1.x/1.0" false false
copy auth.sql auth-blank.sql
del auth.sql
copy auth-blank.sql ..\..\auth-blank.sql
pause
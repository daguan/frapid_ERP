@echo off
bundler\MixERP.Net.Utility.SqlBundler.exe ..\..\..\ "db/1.x/1.0" false true
copy db.sql db-sample.sql
del db.sql
pause
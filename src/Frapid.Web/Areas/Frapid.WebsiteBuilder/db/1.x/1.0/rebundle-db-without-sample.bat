@echo off
bundler\MixERP.Net.Utility.SqlBundler.exe ..\..\..\ "db/1.x/1.0" false false
copy db.sql db-blank.sql
del db.sql
pause
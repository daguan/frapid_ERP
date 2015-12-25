@echo off
bundler\MixERP.Net.Utility.SqlBundler.exe ..\..\..\ "db/1.x/1.0" false false
copy account.sql account-blank.sql
del account.sql
copy account-blank.sql ..\..\account-blank.sql
pause
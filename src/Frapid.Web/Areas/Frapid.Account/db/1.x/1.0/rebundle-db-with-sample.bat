@echo off
bundler\MixERP.Net.Utility.SqlBundler.exe ..\..\..\ "db/1.x/1.0" false true
copy account.sql account-sample.sql
del account.sql
copy account-sample.sql ..\..\account-sample.sql
pause
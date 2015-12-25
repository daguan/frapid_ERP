@echo off
bundler\MixERP.Net.Utility.SqlBundler.exe ..\..\..\ "db/1.x/1.0" false true
copy core.sql core-sample.sql
del core.sql
copy core-sample.sql ..\..\core-sample.sql
pause
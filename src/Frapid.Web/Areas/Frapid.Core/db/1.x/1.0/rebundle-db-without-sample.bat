@echo off
bundler\MixERP.Net.Utility.SqlBundler.exe ..\..\..\ "db/1.x/1.0" false false
copy core.sql core-blank.sql
del core.sql
copy core-blank.sql ..\..\core-blank.sql
pause
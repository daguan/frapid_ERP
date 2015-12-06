@echo off
bundler\MixERP.Net.Utility.SqlBundler.exe ..\..\..\ "db/1.x/1.0" false false
copy config.sql ..\..\config.sql
pause
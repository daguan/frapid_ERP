@echo off
bundler\MixERP.Net.Utility.SqlBundler.exe ..\..\..\ "db/1.x/1.0" false true
copy website.sql website-sample.sql
del website.sql
copy website-sample.sql ..\..\website-sample.sql
pause
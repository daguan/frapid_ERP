@echo off
rmdir %~dp0..\src\Frapid.Web\bin /Q /S
@echo Building Frapid.Web.sln
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" /verbosity:quiet /nologo /property:Configuration=Debug ../src/Frapid.Web.sln /p:VisualStudioVersion=14.0
@echo Building Frapid Dashboard
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" /verbosity:quiet /nologo /property:Configuration=Debug ../src/Frapid.Web/Areas/Frapid.Dashboard/Frapid.Dashboard.sln /p:VisualStudioVersion=14.0
@echo Building Config Module
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" /verbosity:quiet /nologo /property:Configuration=Debug ../src/Frapid.Web/Areas/Frapid.Config/Frapid.Config.sln /p:VisualStudioVersion=14.0
@echo Building Core Module
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" /verbosity:quiet /nologo /property:Configuration=Debug ../src/Frapid.Web/Areas/Frapid.Core/Frapid.Core.sln /p:VisualStudioVersion=14.0
@echo Building WebsiteBuilder Module
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" /verbosity:quiet /nologo /property:Configuration=Debug ../src/Frapid.Web/Areas/Frapid.WebsiteBuilder/Frapid.WebsiteBuilder.sln /p:VisualStudioVersion=14.0
@echo Building Account Module
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" /verbosity:quiet /nologo /property:Configuration=Debug ../src/Frapid.Web/Areas/Frapid.Account/Frapid.Account.sln /p:VisualStudioVersion=14.0
@echo Building Forms Module
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" /verbosity:quiet /nologo /property:Configuration=Debug ../src/Frapid.Web/Areas/Frapid.Forms/Frapid.Forms.sln /p:VisualStudioVersion=14.0
@echo Building Forums Module
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" /verbosity:quiet /nologo /property:Configuration=Debug ../src/Frapid.Web/Areas/MixERP.Forums/MixERP.Forums.sln /p:VisualStudioVersion=14.0
@echo Building Helpdesk Module
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" /verbosity:quiet /nologo /property:Configuration=Debug ../src/Frapid.Web/Areas/MixERP.Helpdesk/MixERP.Helpdesk.sln /p:VisualStudioVersion=14.0
@echo Done
pause
# Installing Frapid on IIS

This document assumes :

* you have already installed PostgreSQL Server 9.3 or above and have password for `postgres` user handy.
* you have cloned frapid github repository locally.
* you have understood the [installation basics](README.md).

Open IIS and create a new website as shown in this image.

![Frapid IIS Website](images/iis.png)

| Field                         | Description                                                                           |
|-------------------------------|---------------------------------------------------------------------------------------|
| Site name                     | The website name.                                                                     |
| Application pool              | Will be automatically created.                                                        |
| Physical path                 | `~/src/Frapid.Web`                                                                    |
| Binding->Type                 | https (Frapid works with HTTPS only)                                                  |
| IP address                    | All unassigned.                                                                       |
| Port                          | Leave this to 443.                                                                    |
| Host name                     | Empty                                                                                 |
| SSL certificate               | Select IIS Express Development Certificate or whatever applicable in your case.       |

Click Ok.

# Build Frapid

Open the frapid solutions in the mentioned order and perform a build:

* ~/src/Frapid.Web.sln
* ~/src/Frapid.Web/Areas/Frapid.Dashboard/Frapid.Dashboard.sln
* ~/src/Frapid.Web/Areas/Frapid.Config/Frapid.Config.sln
* ~/src/Frapid.Web/Areas/Frapid.Core/Frapid.Core.sln
* ~/src/Frapid.Web/Areas/Frapid.WebsiteBuilder/Frapid.WebsiteBuilder.sln
* ~/src/Frapid.Web/Areas/Frapid.Account/Frapid.Account.sln


Alternatively, you can also build frapid by running this batch file:

* ~/builds/all.bat

# Edit DbServer.config

Edit the configuration file [DbServer.config](../configs/DbServer.config.md).

# Browse Frapid

Browse frapid on this url : [https://localhost](https://localhost) and wait a few minutes for the installation to complete.

Reload the page.
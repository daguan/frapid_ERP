# Database Server Configuration

The database server parameters are configured on `~/Resources/Configs/DBServer.config` file.
Please be advised that once you correctly configure this file, frapid will automatically create database(s) as and when required.

**~/Resources/Configs/DBServer.config**
```xml
<?xml version="1.0"?>
<configuration>
  <appSettings>
    <add key="Server" value="localhost" />
    <add key="Port" value="5432" />
    <add key="SuperUserId" value="postgres" />
    <add key="SuperUserPassword" value="binod" />
    <add key="UserId" value="frapid_db_user" />
    <add key="Password" value="change-on-deployment" />
    <add key="MetaDatabase" value="postgres" />
    <add key="ReportUserId" value="report_user" />
    <add key="ReportUserPassword" value="change-on-deployment" />
    <add key="PostgreSQLBinDirectory" value="C:\Program Files\PostgreSQL\9.4\bin\" />
  </appSettings>
</configuration>
```

## Configuration Explained

| Key                         | Configuration |
|-----------------------------| -------------|
| Server                      | The hostname or IP address of your PostgreSQL Server instance. Usually "localhost". |
| Port                        | The port on which the PostgreSQL server is listening. Usually "5432". |
| SuperUserId                 | The user name of PostgreSQL superuser account. This account is used only when frapid needs to create a new database. |
| Password                    | Password for the above user. |
| UserId                      | If not found, frapid will create a user called "frapid_db_user". Frapid uses this account to communicate with the database server. This user has (or should have) minimum permission sets to perform database manipulation. Leave it as it is if you want frapid to take care of this for you. |
| Password                    | Password for the above user. The default password is "change-on-deployment". If you changed password for this account on PostgreSQL Server, change it here as well. |
| MetaDatabase                | The **master database** which contains multi-instance meta information. |
| ReportUserId                | If not found, frapid will create a user called "report_user". This user **must have a read-only access to the database**. Leave it as it is if you want frapid to take care of this for you. |
| ReportUserPassword          | Password for the above user. The default password is "change-on-deployment". If you changed password for this account on PostgreSQL Server, change it here as well. |
| PostgreSQLBinDirectory      | Depending on where you installed PostgreSQL server, enter the correct location of the PostgreSQL bin directory. |

### Related Contents

* [Installing Frapid](../installation/README.md)
* [Developer Documentation](../developer/README.md)
* [Documentation Home](../../README.md)

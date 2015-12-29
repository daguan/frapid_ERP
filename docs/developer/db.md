# Database of Frapid

Follow the [database documentation](db-docs/README.md) for more info.

## Meta Database

Meta database contains global objects which are common to all instances. The meta database script is located on

`~/src/Frapid.Web/db/meta.sql`

| Table                                 | Description                                   |
|---------------------------------------|-----------------------------------------------|
| i18n.resources                        | Neutral resource language table.              |
| i18n.localized_resources              | Contains localized resources.                 |


## Conventions

* A frapid module should create objects under its own single schema.
* A blank database script should be located on `~/src/Frapid.Web/Areas/<module>/db`.
* Optionally, sample database script can also be placed on `~/src/Frapid.Web/Areas/<module>/db`.

## Sql Bundler

It is advised to use [MixERP Sql Bundler Utility](mixerp-sql-bunlder.md).

**Conventions**

* do not create a single SQL script and keep everything on it. It is difficult to manage that way.
* create your own directory structure [similar to frapid](#) and store individual sql files there.
* use SqlBundler.exe to bundle everything together to generate a single SQL file which contains.

**How Sql Bundler Works?**

Create a `.sqlbundle` file (`yaml` format) on your [db directory]().

```yaml
- script-directory : db/1.x/1.0/src
- output-directory:db/1.x/1.0
```


* **script-directory**: The path containing your SQL files. Every single file having `.sql` (but not `.sample.sql`) extension is merged together to create a bundle.
* **output-directory**: The directory where the bundled file will be created. The bundled file will have the filename of `.sqlbundle` file.

**Syntax**

```
path-to\SqlBundler.exe root-path sqlbundle-directory include_sample
```

**Example**

[https://github.com/frapid/frapid/blob/master/src/Frapid.Web/db/meta/1.x/1.0/rebundle.bat](https://github.com/frapid/frapid/blob/master/src/Frapid.Web/db/meta/1.x/1.0/rebundle.bat)

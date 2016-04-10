GO
EXECUTE dbo.drop_schema 'core';
GO
CREATE SCHEMA core;
GO

IF TYPE_ID(N'dbo.money_strict') IS NULL
BEGIN
	CREATE TYPE dbo.money_strict
	FROM DECIMAL(24, 4);
END;

IF TYPE_ID(N'dbo.money_strict2') IS NULL
BEGIN
	CREATE TYPE dbo.money_strict2
	FROM DECIMAL(24, 4);
END;

IF TYPE_ID(N'dbo.integer_strict') IS NULL
BEGIN
	CREATE TYPE dbo.integer_strict
	FROM integer;
END;

IF TYPE_ID(N'dbo.integer_strict2') IS NULL
BEGIN
	CREATE TYPE dbo.integer_strict2
	FROM integer;
END;

IF TYPE_ID(N'dbo.smallint_strict') IS NULL
BEGIN
	CREATE TYPE dbo.smallint_strict
	FROM smallint;
END;

IF TYPE_ID(N'dbo.smallint_strict2') IS NULL
BEGIN
	CREATE TYPE dbo.smallint_strict2
	FROM smallint;
END;

IF TYPE_ID(N'dbo.decimal_strict') IS NULL
BEGIN
	CREATE TYPE dbo.decimal_strict
	FROM decimal;
END;

IF TYPE_ID(N'dbo.decimal_strict2') IS NULL
BEGIN
	CREATE TYPE dbo.decimal_strict2
	FROM decimal;
END;

IF TYPE_ID(N'dbo.color') IS NULL
BEGIN
	CREATE TYPE dbo.color
	FROM national character varying(50);
END;

IF TYPE_ID(N'dbo.photo') IS NULL
BEGIN
	CREATE TYPE dbo.photo
	FROM national character varying(MAX);
END;


IF TYPE_ID(N'dbo.html') IS NULL
BEGIN
	CREATE TYPE dbo.html
	FROM national character varying(MAX);
END;


IF TYPE_ID(N'dbo.password') IS NULL
BEGIN
	CREATE TYPE dbo.password
	FROM national character varying(MAX);
END;

DROP FUNCTION IF EXISTS public.text_to_int_array
(
    _input                              text,
    _remove_nulls                       boolean
);

CREATE FUNCTION public.text_to_int_array
(
    _input                              text,
    _remove_nulls                       boolean = true
)
RETURNS integer[]
STABLE
AS
$$
    DECLARE _int_array                  integer[];
BEGIN
    WITH items
    AS
    (
        SELECT
            item, 
            item ~ '^[0-9]+$' AS is_number
        FROM 
            unnest(regexp_split_to_array(_input, ',')) AS item
    )
    SELECT
        array_agg(item)
    INTO
        _int_array
    FROM items
    WHERE is_number;

    RETURN _int_array;   
END
$$
LANGUAGE plpgsql;


DROP FUNCTION IF EXISTS public.poco_get_table_function_annotation
(
    _schema_name            text,
    _table_name             text
);

CREATE FUNCTION public.poco_get_table_function_annotation
(
    _schema_name            text,
    _table_name             text
)
RETURNS TABLE
(
    id                      integer, 
    column_name             text, 
    nullable                text, 
    udt_name                text, 
    column_default          text, 
    max_length              integer, 
    primary_key             text
) AS
$$
    DECLARE _args           text;
BEGIN
    DROP TABLE IF EXISTS temp_annonation;

    CREATE TEMPORARY TABLE temp_annonation
    (
        id                      SERIAL,
        column_name             text,
        is_nullable             text DEFAULT('NO'),
        udt_name                text,
        column_default          text,
        max_length              integer DEFAULT(0),
        is_primary_key          text DEFAULT('NO')
    ) ON COMMIT DROP;


    SELECT
        pg_catalog.pg_get_function_arguments(pg_proc.oid) AS arguments
    INTO
        _args
    FROM pg_proc
    INNER JOIN pg_namespace
    ON pg_proc.pronamespace = pg_namespace.oid
    INNER JOIN pg_type
    ON pg_proc.prorettype = pg_type.oid
    INNER JOIN pg_namespace type_namespace
    ON pg_type.typnamespace = type_namespace.oid
    WHERE typname != ANY(ARRAY['trigger'])
    AND pg_namespace.nspname = _schema_name
    AND proname::text = _table_name;

    INSERT INTO temp_annonation(column_name, udt_name)
    SELECT split_part(trim(unnest(regexp_split_to_array(_args, ','))), ' ', 1), trim(unnest(regexp_split_to_array(_args, ',')));

    UPDATE temp_annonation
    SET udt_name = TRIM(REPLACE(temp_annonation.udt_name, temp_annonation.column_name, ''));

    
    RETURN QUERY
    SELECT * FROM temp_annonation;
END
$$
LANGUAGE plpgsql;

  
DROP FUNCTION IF EXISTS get_app_data_type(_db_data_type text);

CREATE FUNCTION get_app_data_type(_db_data_type text)
RETURNS text
STABLE
AS
$$
BEGIN
    IF(_db_data_type IN('int4', 'int', 'integer')) THEN
        RETURN 'int';
    END IF;

    IF(_db_data_type IN('varchar', 'character varying', 'text')) THEN
        RETURN 'string';
    END IF;
    
    IF(_db_data_type IN('date', 'time', 'timestamp', 'timestamptz')) THEN
        RETURN 'System.DateTime';
    END IF;
    
    IF(_db_data_type IN('bool', 'boolean')) THEN
        RETURN 'bool';
    END IF;

    RETURN $1;
END
$$
LANGUAGE plpgsql;


DROP FUNCTION IF EXISTS public.poco_get_table_function_definition(_schema national character varying(63), _name national character varying(63));

CREATE FUNCTION public.poco_get_table_function_definition(_schema national character varying(63), _name  national character varying(63))
RETURNS TABLE
(
    id                      bigint,
    column_name             text,
    nullable                text,
    udt_name                text,
    column_default          text,
    max_length              integer,
    primary_key             text,
    data_type               text
)
AS
$$
    DECLARE _oid            oid;
    DECLARE _typoid         oid;
BEGIN
    CREATE TEMPORARY TABLE temp_poco
    (
        id                      bigint,
        column_name             text,
        is_nullable             text,
        udt_name                text,
        column_default          text,
        max_length              integer default(0),
        is_primary_key          text,
        data_type               text
    ) ON COMMIT DROP;

    SELECT        
        pg_proc.oid,
        pg_proc.prorettype
    INTO 
        _oid,
        _typoid
    FROM pg_proc
    INNER JOIN pg_namespace
    ON pg_proc.pronamespace = pg_namespace.oid
    WHERE pg_proc.proname=_name
    AND pg_namespace.nspname=_schema
    LIMIT 1;


    IF EXISTS
    (
        SELECT 1
        FROM information_schema.columns 
        WHERE table_schema=_schema 
        AND table_name=_name
    ) THEN
        INSERT INTO temp_poco
        SELECT
            row_number() OVER(ORDER BY attnum),
            attname AS column_name,
            CASE WHEN attnotnull THEN 'NO' ELSE 'YES' END,
            pg_type.typname,
            public.parse_default(adsrc),
            CASE WHEN atttypmod <> -1 
            THEN atttypmod - 4
            ELSE 0 END,
            CASE WHEN indisprimary THEN 'YES' ELSE 'NO' END
        FROM   pg_attribute
        LEFT JOIN pg_index
        ON pg_attribute.ATTRELID = pg_index.indrelid
        AND pg_attribute.attnum = ANY(pg_index.indkey)
        AND pg_index.indisprimary
        INNER JOIN pg_type
        ON pg_attribute.atttypid = pg_type.oid
        LEFT   JOIN pg_catalog.pg_attrdef
        ON (pg_attribute.attrelid, pg_attribute.attnum) = (pg_attrdef.adrelid,  pg_attrdef.adnum)
        WHERE  attrelid = (_schema || '.' || _name)::regclass
        AND    attnum > 0
        AND    NOT attisdropped
        ORDER  BY attnum;

        UPDATE temp_poco
        SET data_type = public.get_app_data_type(temp_poco.udt_name);
        
        RETURN QUERY
        SELECT * FROM temp_poco;
        RETURN;
    END IF;

    IF EXISTS(SELECT * FROM pg_type WHERE oid = _typoid AND typtype='c') THEN
        --Composite Type
        INSERT INTO temp_poco
        SELECT
            row_number() OVER(ORDER BY attnum),
            attname::text               AS column_name,
            'NO'::text                  AS is_nullable, 
            format_type(t.oid,NULL)     AS udt_name,
            ''::text                    AS column_default
        FROM pg_attribute att
        JOIN pg_type t ON t.oid=atttypid
        JOIN pg_namespace nsp ON t.typnamespace=nsp.oid
        LEFT OUTER JOIN pg_type b ON t.typelem=b.oid
        LEFT OUTER JOIN pg_collation c ON att.attcollation=c.oid
        LEFT OUTER JOIN pg_namespace nspc ON c.collnamespace=nspc.oid
        WHERE att.attrelid=(SELECT typrelid FROM pg_type WHERE pg_type.oid = _typoid)
        AND att.attnum > 0
        ORDER by attnum;

        UPDATE temp_poco
        SET data_type = public.get_app_data_type(temp_poco.udt_name);

        RETURN QUERY
        SELECT * FROM temp_poco;
        RETURN;
    END IF;

    IF(_oid IS NOT NULL) THEN
        INSERT INTO temp_poco
        WITH procs
        AS
        (
            SELECT 
            row_number() OVER(ORDER BY proallargtypes),
            explode_array(proargnames) as column_name,
            explode_array(proargmodes) as column_mode,
            explode_array(proallargtypes) as argument_type
            FROM pg_proc
            WHERE oid = _oid
        )
        SELECT
            row_number() OVER(ORDER BY 1),
            procs.column_name::text,
            'NO'::text AS is_nullable, 
            format_type(procs.argument_type, null) as udt_name,
            ''::text AS column_default
        FROM procs
        WHERE column_mode=ANY(ARRAY['t', 'o']);

        UPDATE temp_poco
        SET data_type = public.get_app_data_type(temp_poco.udt_name);

        RETURN QUERY
        SELECT * FROM temp_poco;
        RETURN;
    END IF;

    INSERT INTO temp_poco
    SELECT 
        row_number() OVER(ORDER BY attnum),
        attname::text               AS column_name,
        'NO'::text                  AS is_nullable, 
        format_type(t.oid,NULL)     AS udt_name,
        ''::text                    AS column_default
    FROM pg_attribute att
    JOIN pg_type t ON t.oid=atttypid
    JOIN pg_namespace nsp ON t.typnamespace=nsp.oid
    LEFT OUTER JOIN pg_type b ON t.typelem=b.oid
    LEFT OUTER JOIN pg_collation c ON att.attcollation=c.oid
    LEFT OUTER JOIN pg_namespace nspc ON c.collnamespace=nspc.oid
    WHERE att.attrelid=
    (
        SELECT typrelid 
        FROM pg_type
        INNER JOIN pg_namespace
        ON pg_type.typnamespace = pg_namespace.oid
        WHERE typname=_name
        AND pg_namespace.nspname=_schema
    )
    AND att.attnum > 0
    ORDER by attnum;

    UPDATE temp_poco
    SET data_type = public.get_app_data_type(temp_poco.udt_name);

    RETURN QUERY
    SELECT * FROM temp_poco;
END;
$$
LANGUAGE plpgsql;

DROP FUNCTION IF EXISTS public.poco_get_tables(_schema text);

CREATE FUNCTION public.poco_get_tables(_schema text)
RETURNS TABLE
(
    table_schema                name, 
    table_name                  name, 
    table_type                  text, 
    has_duplicate               boolean
) AS
$$
BEGIN
    CREATE TEMPORARY TABLE _t
    (
        table_schema            name,
        table_name              name,
        table_type              text,
        has_duplicate           boolean DEFAULT(false)
    ) ON COMMIT DROP;

    INSERT INTO _t
    SELECT 
        information_schema.tables.table_schema, 
        information_schema.tables.table_name, 
        information_schema.tables.table_type
    FROM information_schema.tables 
    WHERE (information_schema.tables.table_type='BASE TABLE' OR information_schema.tables.table_type='VIEW')
    AND information_schema.tables.table_schema = _schema
    UNION ALL
    SELECT DISTINCT 
        pg_namespace.nspname::text, 
        pg_proc.proname::text, 
        'FUNCTION' AS table_type
    FROM pg_proc
    INNER JOIN pg_namespace
    ON pg_proc.pronamespace = pg_namespace.oid
    INNER JOIN pg_language 
    ON pg_proc.prolang = pg_language .oid
    INNER JOIN pg_type
    ON pg_proc.prorettype=pg_type.oid
    WHERE ('t' = ANY(pg_proc.proargmodes) OR 'o' = ANY(pg_proc.proargmodes) OR pg_type.typtype = 'c')
    AND lanname NOT IN ('c','internal')
    AND nspname=_schema;


    UPDATE _t
    SET has_duplicate = TRUE
    FROM
    (
        SELECT
            information_schema.tables.table_name,
            COUNT(information_schema.tables.table_name) AS table_count
        FROM information_schema.tables
        GROUP BY information_schema.tables.table_name
        
    ) subquery
    WHERE subquery.table_name = _t.table_name
    AND subquery.table_count > 1;

    

    RETURN QUERY
    SELECT * FROM _t;
END
$$
LANGUAGE plpgsql;

DROP FUNCTION IF EXISTS public.parse_default(text);

CREATE OR REPLACE FUNCTION public.parse_default(text)
RETURNS text 
AS
$$
    DECLARE _sql text;
    DECLARE _val text;
BEGIN
    IF($1 NOT LIKE 'nextval%') THEN
        _sql := 'SELECT ' || $1;
        EXECUTE _sql INTO _val;
        RAISE NOTICE '%', _sql;
        RETURN _val;
    END IF;

    IF($1 = 'now()') THEN
        RETURN '';
    END IF;

    RETURN $1;
END
$$
LANGUAGE plpgsql;

DROP FUNCTION IF EXISTS public.explode_array(in_array anyarray);

CREATE FUNCTION public.explode_array(in_array anyarray)
RETURNS SETOF anyelement
IMMUTABLE
AS
$$
    SELECT ($1)[s] FROM generate_series(1,array_upper($1, 1)) AS s;
$$
LANGUAGE sql;
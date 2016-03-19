DROP FUNCTION IF EXISTS i18n.get_localization_table(text);

CREATE FUNCTION i18n.get_localization_table
(
    culture_code        text
)
RETURNS TABLE
(
    id                  bigint,
    resource_class      text,
    key                 text,
    original            text,
    translated          text
)
AS
$$
BEGIN   
    CREATE TEMPORARY TABLE t
    (
        resource_id         integer,
        resource_class      text,
        key                 text,
        original            text,
        translated          text
    ) ON COMMIT DROP;
    
    INSERT INTO t
    SELECT
        i18n.resources.resource_id, 
        i18n.resources.resource_class, 
        i18n.resources.key, 
        i18n.resources.value,
        ''
    FROM i18n.resources;

    UPDATE t
    SET translated = i18n.localized_resources.value
    FROM i18n.localized_resources
    WHERE t.resource_id = i18n.localized_resources.resource_id
    AND i18n.localized_resources.culture_code=$1;

    RETURN QUERY
    SELECT
        row_number() OVER(ORDER BY t.resource_class, t.key),
        t.resource_class, 
        t.key, 
        t.original,
        t.translated
    FROM t;
END
$$
LANGUAGE plpgsql;

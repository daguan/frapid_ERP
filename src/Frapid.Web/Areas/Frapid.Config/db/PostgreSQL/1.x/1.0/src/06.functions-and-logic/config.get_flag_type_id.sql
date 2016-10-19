DROP FUNCTION IF EXISTS config.get_flag_type_id
(
    user_id_        integer,
    resource_       text,
    resource_key_   text,
    resource_id_    text
);

CREATE FUNCTION config.get_flag_type_id
(
    user_id_        integer,
    resource_       text,
    resource_key_   text,
    resource_id_    text
)
RETURNS integer
STABLE
AS
$$
BEGIN
    RETURN flag_type_id
    FROM config.flags
    WHERE config.flags.user_id=$1
    AND config.flags.resource=$2
    AND config.flags.resource_key=$3
    AND config.flags.resource_id=$4
	AND NOT config.flags.deleted;
END
$$
LANGUAGE plpgsql;

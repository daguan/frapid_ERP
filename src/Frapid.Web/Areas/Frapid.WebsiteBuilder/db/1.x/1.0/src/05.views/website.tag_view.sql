DROP VIEW IF EXISTS website.tag_view;

CREATE VIEW website.tag_view
AS
WITH tags
AS
(
SELECT DISTINCT unnest(regexp_split_to_array(tags, ',')) AS tag FROM website.contents
)
SELECT
    ROW_NUMBER() OVER (ORDER BY tag) AS tag_id,
    tag
FROM tags;

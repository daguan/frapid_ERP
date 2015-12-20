DROP VIEW IF EXISTS website.tag_view;

CREATE VIEW website.tag_view
AS
SELECT DISTINCT unnest(regexp_split_to_array(tags, ',')) AS tag FROM website.contents;

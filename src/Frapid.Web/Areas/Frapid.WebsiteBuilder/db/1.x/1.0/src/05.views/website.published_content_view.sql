DROP VIEW IF EXISTS website.published_content_view;

CREATE VIEW website.published_content_view
AS
SELECT
    website.contents.content_id,
    website.contents.category_id,
    website.categories.category_name,
    website.categories.alias AS category_alias,
    website.contents.title,
    website.contents.alias,
    website.contents.author_id,
    website.contents.markdown,
    website.contents.contents,
    website.contents.tags,
    website.contents.seo_keywords,
    website.contents.seo_description,
    website.contents.is_homepage
FROM website.contents
INNER JOIN website.categories
ON website.categories.category_id = website.contents.category_id
WHERE NOT is_draft
AND publish_on <= NOW();
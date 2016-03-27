DROP VIEW IF EXISTS website.yesterdays_email_subscriptions;

CREATE VIEW website.yesterdays_email_subscriptions
AS
SELECT
    email,
    first_name,
    last_name,
    'subscribed' AS subscription_type
FROM website.email_subscriptions
WHERE subscribed_on::date = 'yesterday'::date
AND NOT confirmed_on::date = 'yesterday'::date
UNION ALL
SELECT
    email,
    first_name,
    last_name,
    'unsubscribed'
FROM website.email_subscriptions
WHERE unsubscribed_on::date = 'yesterday'::date
UNION ALL
SELECT
    email,
    first_name,
    last_name,
    'confirmed'
FROM website.email_subscriptions
WHERE confirmed_on::date = 'yesterday'::date;
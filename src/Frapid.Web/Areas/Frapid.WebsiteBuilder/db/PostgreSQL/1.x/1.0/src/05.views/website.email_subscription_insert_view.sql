DROP VIEW IF EXISTS website.email_subscription_insert_view;

CREATE VIEW website.email_subscription_insert_view
AS
SELECT * FROM website.email_subscriptions
WHERE 1 = 0;


SELECT * FROM website.email_subscription_insert_view;

CREATE RULE log_subscriptions AS 
ON INSERT TO website.email_subscription_insert_view
DO INSTEAD 
INSERT INTO website.email_subscriptions
(
    email, 
    browser, 
    ip_address, 
    unsubscribed, 
    subscribed_on, 
    unsubscribed_on, 
    first_name, 
    last_name, 
    confirmed
)
SELECT
    NEW.email, 
    NEW.browser, 
    NEW.ip_address, 
    NEW.unsubscribed, 
    COALESCE(NEW.subscribed_on, NOW()), 
    NEW.unsubscribed_on, 
    NEW.first_name, 
    NEW.last_name, 
    NEW.confirmed
WHERE NOT EXISTS
(
    SELECT 1 
    FROM website.email_subscriptions
    WHERE email = NEW.email
);


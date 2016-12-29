DELETE FROM social.feeds;

INSERT INTO social.feeds(event_timestamp, formatted_text, created_by, scope, is_public)
SELECT created_on, '<a href="/social/user/' + CAST(user_id AS character varying(100)) + '">' + name + ' (' + account.get_role_name_by_role_id(role_id) + ')<a/> joined us on <a class="formatted date">' + CAST(created_on AS character varying(100)) + '</a>', 1, 'Notifications', 1
FROM account.users;

GO

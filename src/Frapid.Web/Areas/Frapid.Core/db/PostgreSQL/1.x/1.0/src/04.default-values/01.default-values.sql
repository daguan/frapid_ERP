INSERT INTO core.offices(office_code, office_name, currency_code)
SELECT 'DEF', 'Default', 'USD';

INSERT INTO core.genders(gender_code, gender_name)
SELECT 'M', 'Male' UNION ALL
SELECT 'F', 'Female';

INSERT INTO core.marital_statuses(marital_status_code, marital_status_name, is_legally_recognized_marriage)
SELECT 'NEM', 'Never Married',          false UNION ALL
SELECT 'SEP', 'Separated',              false UNION ALL
SELECT 'MAR', 'Married',                true UNION ALL
SELECT 'LIV', 'Living Relationship',    false UNION ALL
SELECT 'DIV', 'Divorced',               false UNION ALL
SELECT 'WID', 'Widower',                false UNION ALL
SELECT 'CIV', 'Civil Union',            true;

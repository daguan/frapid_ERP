INSERT INTO core.offices(office_code, office_name)
SELECT 'DEF', 'Default';

INSERT INTO core.genders(gender_code, gender_name)
SELECT 'M', 'Male' UNION ALL
SELECT 'F', 'Female';

INSERT INTO core.marital_statuses(marital_status_code, marital_status_name, is_legally_recognized_marriage)
SELECT 'NEM', 'Never Married',          0 UNION ALL
SELECT 'SEP', 'Separated',              0 UNION ALL
SELECT 'MAR', 'Married',                1 UNION ALL
SELECT 'LIV', 'Living Relationship',    0 UNION ALL
SELECT 'DIV', 'Divorced',               0 UNION ALL
SELECT 'WID', 'Widower',                0 UNION ALL
SELECT 'CIV', 'Civil Union',            1;

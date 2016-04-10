GO

IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='text')
BEGIN
    INSERT INTO config.custom_field_data_types(data_type, is_number)
    SELECT 'text', 0;
END;

IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='Number')
BEGIN
    INSERT INTO config.custom_field_data_types(data_type, is_number)
    SELECT 'Number', 1;
END;

IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='Date')
BEGIN
    INSERT INTO config.custom_field_data_types(data_type, is_date)
    SELECT 'Date', 1;
END;

IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='True/False')
BEGIN
    INSERT INTO config.custom_field_data_types(data_type, is_bit)
    SELECT 'True/False', 1;
END;

IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='Long Text')
BEGIN
    INSERT INTO config.custom_field_data_types(data_type, is_long_text)
    SELECT 'Long Text', 1;
END;


GO

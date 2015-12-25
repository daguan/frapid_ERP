DO
$$
BEGIN
    IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='Text') THEN
        INSERT INTO config.custom_field_data_types(data_type, is_number)
        SELECT 'Text', true;
    END IF;

    IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='Number') THEN
        INSERT INTO config.custom_field_data_types(data_type, is_number)
        SELECT 'Number', true;
    END IF;

    IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='Date') THEN
        INSERT INTO config.custom_field_data_types(data_type, is_date)
        SELECT 'Date', true;
    END IF;

    IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='True/False') THEN
        INSERT INTO config.custom_field_data_types(data_type, is_boolean)
        SELECT 'True/False', true;
    END IF;

    IF NOT EXISTS(SELECT * FROM config.custom_field_data_types WHERE data_type='Long Text') THEN
        INSERT INTO config.custom_field_data_types(data_type, is_long_text)
        SELECT 'Long Text', true;
    END IF;
END
$$
LANGUAGE plpgsql;

# finance schema

**Tables**

| # | Table Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |
| 1 | [account_masters](../tables/finance/account_masters.md) | frapid_db_user | DEFAULT |  |
| 2 | [accounts](../tables/finance/accounts.md) | frapid_db_user | DEFAULT |  |
| 3 | [auto_verification_policy](../tables/finance/auto_verification_policy.md) | frapid_db_user | DEFAULT |  |
| 4 | [bank_accounts](../tables/finance/bank_accounts.md) | frapid_db_user | DEFAULT |  |
| 5 | [card_types](../tables/finance/card_types.md) | frapid_db_user | DEFAULT |  |
| 6 | [cash_flow_headings](../tables/finance/cash_flow_headings.md) | frapid_db_user | DEFAULT |  |
| 7 | [cash_flow_setup](../tables/finance/cash_flow_setup.md) | frapid_db_user | DEFAULT |  |
| 8 | [cash_repositories](../tables/finance/cash_repositories.md) | frapid_db_user | DEFAULT |  |
| 9 | [cost_centers](../tables/finance/cost_centers.md) | frapid_db_user | DEFAULT |  |
| 10 | [day_operation](../tables/finance/day_operation.md) | frapid_db_user | DEFAULT |  |
| 11 | [day_operation_routines](../tables/finance/day_operation_routines.md) | frapid_db_user | DEFAULT |  |
| 12 | [exchange_rate_details](../tables/finance/exchange_rate_details.md) | frapid_db_user | DEFAULT |  |
| 13 | [exchange_rates](../tables/finance/exchange_rates.md) | frapid_db_user | DEFAULT |  |
| 14 | [fiscal_year](../tables/finance/fiscal_year.md) | frapid_db_user | DEFAULT |  |
| 15 | [frequencies](../tables/finance/frequencies.md) | frapid_db_user | DEFAULT |  |
| 16 | [frequency_setups](../tables/finance/frequency_setups.md) | frapid_db_user | DEFAULT |  |
| 17 | [journal_verification_policy](../tables/finance/journal_verification_policy.md) | frapid_db_user | DEFAULT |  |
| 18 | [merchant_fee_setup](../tables/finance/merchant_fee_setup.md) | frapid_db_user | DEFAULT |  |
| 19 | [payment_cards](../tables/finance/payment_cards.md) | frapid_db_user | DEFAULT |  |
| 20 | [routines](../tables/finance/routines.md) | frapid_db_user | DEFAULT |  |
| 21 | [tax_setups](../tables/finance/tax_setups.md) | frapid_db_user | DEFAULT |  |
| 22 | [transaction_details](../tables/finance/transaction_details.md) | frapid_db_user | DEFAULT |  |
| 23 | [transaction_documents](../tables/finance/transaction_documents.md) | frapid_db_user | DEFAULT |  |
| 24 | [transaction_master](../tables/finance/transaction_master.md) | frapid_db_user | DEFAULT |  |
| 25 | [transaction_types](../tables/finance/transaction_types.md) | frapid_db_user | DEFAULT |  |



**Sequences**

| # | Sequence Name | Owner | Data Type | Start Value | Increment | Description |
| --- | --- | --- | --- | --- | --- | --- |
| 1 | accounts_account_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 2 | auto_verification_policy_auto_verification_policy_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 3 | bank_accounts_bank_account_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 4 | cash_flow_setup_cash_flow_setup_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 5 | cash_repositories_cash_repository_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 6 | cost_centers_cost_center_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 7 | day_operation_day_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 8 | day_operation_routines_day_operation_routine_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 9 | exchange_rate_details_exchange_rate_detail_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 10 | exchange_rates_exchange_rate_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 11 | fiscal_year_fiscal_year_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 12 | frequencies_frequency_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 13 | frequency_setups_frequency_setup_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 14 | journal_verification_policy_journal_verification_policy_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 15 | merchant_fee_setup_merchant_fee_setup_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 16 | payment_cards_payment_card_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 17 | routines_routine_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 18 | tax_setups_tax_setup_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 19 | transaction_details_transaction_detail_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 20 | transaction_documents_document_id_seq | frapid_db_user | bigint | 1 | 1 |  |
| 21 | transaction_master_transaction_master_id_seq | frapid_db_user | bigint | 1 | 1 |  |


**Views**

| # | View Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |
| 1 | [account_scrud_view](../views/finance/account_scrud_view.md) | frapid_db_user | DEFAULT |  |
| 2 | [account_selector_view](../views/finance/account_selector_view.md) | frapid_db_user | DEFAULT |  |
| 3 | [account_view](../views/finance/account_view.md) | frapid_db_user | DEFAULT |  |
| 4 | [auto_verification_policy_scrud_view](../views/finance/auto_verification_policy_scrud_view.md) | frapid_db_user | DEFAULT |  |
| 5 | [bank_account_scrud_view](../views/finance/bank_account_scrud_view.md) | frapid_db_user | DEFAULT |  |
| 6 | [bank_account_selector_view](../views/finance/bank_account_selector_view.md) | frapid_db_user | DEFAULT |  |
| 7 | [cash_flow_heading_scrud_view](../views/finance/cash_flow_heading_scrud_view.md) | frapid_db_user | DEFAULT |  |
| 8 | [cash_flow_setup_scrud_view](../views/finance/cash_flow_setup_scrud_view.md) | frapid_db_user | DEFAULT |  |
| 9 | [cash_repository_scrud_view](../views/finance/cash_repository_scrud_view.md) | frapid_db_user | DEFAULT |  |
| 10 | [cost_center_scrud_view](../views/finance/cost_center_scrud_view.md) | frapid_db_user | DEFAULT |  |
| 11 | [fiscal_year_scrud_view](../views/finance/fiscal_year_scrud_view.md) | frapid_db_user | DEFAULT |  |
| 12 | [frequency_date_view](../views/finance/frequency_date_view.md) | frapid_db_user | DEFAULT |  |
| 13 | [frequency_setup_scrud_view](../views/finance/frequency_setup_scrud_view.md) | frapid_db_user | DEFAULT |  |
| 14 | [journal_verification_policy_scrud_view](../views/finance/journal_verification_policy_scrud_view.md) | frapid_db_user | DEFAULT |  |
| 15 | [merchant_fee_setup_scrud_view](../views/finance/merchant_fee_setup_scrud_view.md) | frapid_db_user | DEFAULT |  |
| 16 | [payable_account_selector_view](../views/finance/payable_account_selector_view.md) | frapid_db_user | DEFAULT |  |
| 17 | [payment_card_scrud_view](../views/finance/payment_card_scrud_view.md) | frapid_db_user | DEFAULT |  |
| 18 | [receivable_account_selector_view](../views/finance/receivable_account_selector_view.md) | frapid_db_user | DEFAULT |  |
| 19 | [transaction_verification_status_view](../views/finance/transaction_verification_status_view.md) | frapid_db_user | DEFAULT |  |
| 20 | [transaction_view](../views/finance/transaction_view.md) | frapid_db_user | DEFAULT |  |
| 21 | [verified_transaction_view](../views/finance/verified_transaction_view.md) | frapid_db_user | DEFAULT |  |



**Materialized Views**

| # | Matview Name | Owner | Tablespace | Description |
| --- | --- | --- | --- | --- |
| 1 | [trial_balance_view](../materialized-views/finance/trial_balance_view.md) | frapid_db_user | DEFAULT |  || 2 | [verified_cash_transaction_mat_view](../materialized-views/finance/verified_cash_transaction_mat_view.md) | frapid_db_user | DEFAULT |  || 3 | [verified_transaction_mat_view](../materialized-views/finance/verified_transaction_mat_view.md) | frapid_db_user | DEFAULT |  |


**Functions**

| # | Function | Owner | Description |
| --- | --- | --- | --- |
| 1 | [auto_verify(_tran_id bigint, _office_id integer)RETURNS void](../functions/finance/auto_verify-4237266.md) | frapid_db_user |  |
| 2 | [can_post_transaction(_login_id bigint, _user_id integer, _office_id integer, transaction_book text, _value_date date)RETURNS boolean](../functions/finance/can_post_transaction-4237267.md) | frapid_db_user |  |
| 3 | [can_post_transaction(_login_id bigint, _user_id integer, _office_id integer, transaction_book text, _value_date timestamp without time zone)RETURNS boolean](../functions/finance/can_post_transaction-4237268.md) | frapid_db_user |  |
| 4 | [convert_exchange_rate(office_id integer, source_currency_code character varying, destination_currency_code character varying)RETURNS decimal_strict2](../functions/finance/convert_exchange_rate-4237269.md) | frapid_db_user |  |
| 5 | [create_payment_card(_payment_card_code character varying, _payment_card_name character varying, _card_type_id integer)RETURNS void](../functions/finance/create_payment_card-4237332.md) | frapid_db_user |  |
| 6 | [create_routine(_routine_code character varying, _routine regproc, _order integer)RETURNS void](../functions/finance/create_routine-4237270.md) | frapid_db_user |  |
| 7 | [eod_required(_office_id integer)RETURNS boolean](../functions/finance/eod_required-4237280.md) | frapid_db_user |  |
| 8 | [get_account_id_by_account_name(text)RETURNS integer](../functions/finance/get_account_id_by_account_name-4237281.md) | frapid_db_user |  |
| 9 | [get_account_id_by_account_number(text)RETURNS integer](../functions/finance/get_account_id_by_account_number-4237282.md) | frapid_db_user |  |
| 10 | [get_account_ids(root_account_id integer)RETURNS SETOF integer](../functions/finance/get_account_ids-4237283.md) | frapid_db_user |  |
| 11 | [get_account_master_id_by_account_id(bigint)RETURNS integer](../functions/finance/get_account_master_id_by_account_id-4237284.md) | frapid_db_user |  |
| 12 | [get_account_master_id_by_account_master_code(_account_master_code text)RETURNS integer](../functions/finance/get_account_master_id_by_account_master_code-4237286.md) | frapid_db_user |  |
| 13 | [get_account_name_by_account_id(_account_id integer)RETURNS text](../functions/finance/get_account_name_by_account_id-4237287.md) | frapid_db_user |  |
| 14 | [get_account_statement(_value_date_from date, _value_date_to date, _user_id integer, _account_id integer, _office_id integer)RETURNS TABLE(id integer, transaction_id bigint, transaction_detail_id bigint, value_date date, book_date date, tran_code text, reference_number text, statement_reference text, reconciliation_memo text, debit numeric, credit numeric, balance numeric, office text, book text, account_id integer, account_number text, account text, posted_on timestamp with time zone, posted_by text, approved_by text, verification_status integer)](../functions/finance/get_account_statement-4237288.md) | frapid_db_user |  |
| 15 | [get_account_view_by_account_master_id(_account_master_id integer, _row_number integer)RETURNS TABLE(id bigint, account_id integer, account_name text)](../functions/finance/get_account_view_by_account_master_id-4237290.md) | frapid_db_user |  |
| 16 | [get_balance_sheet(_previous_period date, _current_period date, _user_id integer, _office_id integer, _factor integer)RETURNS TABLE(id bigint, item text, previous_period numeric, current_period numeric, account_id integer, account_number text, is_retained_earning boolean)](../functions/finance/get_balance_sheet-4237333.md) | frapid_db_user |  |
| 17 | [get_cash_flow_heading_id_by_cash_flow_heading_code(_cash_flow_heading_code character varying)RETURNS integer](../functions/finance/get_cash_flow_heading_id_by_cash_flow_heading_code-4237291.md) | frapid_db_user |  |
| 18 | [get_cash_flow_statement(_date_from date, _date_to date, _user_id integer, _office_id integer, _factor integer)RETURNS json](../functions/finance/get_cash_flow_statement-4237335.md) | frapid_db_user |  |
| 19 | [get_cash_repository_balance(_cash_repository_id integer, _currency_code character varying)RETURNS money_strict2](../functions/finance/get_cash_repository_balance-4237292.md) | frapid_db_user |  |
| 20 | [get_cash_repository_id_by_cash_repository_code(text)RETURNS integer](../functions/finance/get_cash_repository_id_by_cash_repository_code-4237293.md) | frapid_db_user |  |
| 21 | [get_cash_repository_id_by_cash_repository_name(text)RETURNS integer](../functions/finance/get_cash_repository_id_by_cash_repository_name-4237294.md) | frapid_db_user |  |
| 22 | [get_cost_center_id_by_cost_center_code(text)RETURNS integer](../functions/finance/get_cost_center_id_by_cost_center_code-4237295.md) | frapid_db_user |  |
| 23 | [get_date(_office_id integer)RETURNS date](../functions/finance/get_date-4237271.md) | frapid_db_user |  |
| 24 | [get_default_currency_code(cash_repository_id integer)RETURNS character varying](../functions/finance/get_default_currency_code-4237296.md) | frapid_db_user |  |
| 25 | [get_default_currency_code_by_office_id(office_id integer)RETURNS character varying](../functions/finance/get_default_currency_code_by_office_id-4237297.md) | frapid_db_user |  |
| 26 | [get_exchange_rate(office_id integer, currency_code character varying)RETURNS decimal_strict2](../functions/finance/get_exchange_rate-4237298.md) | frapid_db_user |  |
| 27 | [get_fiscal_half_end_date(_office_id integer)RETURNS date](../functions/finance/get_fiscal_half_end_date-4237276.md) | frapid_db_user |  |
| 28 | [get_fiscal_half_start_date(_office_id integer)RETURNS date](../functions/finance/get_fiscal_half_start_date-4237277.md) | frapid_db_user |  |
| 29 | [get_fiscal_year_end_date(_office_id integer)RETURNS date](../functions/finance/get_fiscal_year_end_date-4237278.md) | frapid_db_user |  |
| 30 | [get_fiscal_year_start_date(_office_id integer)RETURNS date](../functions/finance/get_fiscal_year_start_date-4237279.md) | frapid_db_user |  |
| 31 | [get_frequencies(_frequency_id integer)RETURNS integer[]](../functions/finance/get_frequencies-4237299.md) | frapid_db_user |  |
| 32 | [get_frequency_end_date(_frequency_id integer, _value_date date)RETURNS date](../functions/finance/get_frequency_end_date-4237300.md) | frapid_db_user |  |
| 33 | [get_frequency_setup_code_by_frequency_setup_id(_frequency_setup_id integer)RETURNS text](../functions/finance/get_frequency_setup_code_by_frequency_setup_id-4237301.md) | frapid_db_user |  |
| 34 | [get_frequency_setup_end_date_by_frequency_setup_id(_frequency_setup_id integer)RETURNS date](../functions/finance/get_frequency_setup_end_date_by_frequency_setup_id-4237302.md) | frapid_db_user |  |
| 35 | [get_frequency_setup_start_date_by_frequency_setup_id(_frequency_setup_id integer)RETURNS date](../functions/finance/get_frequency_setup_start_date_by_frequency_setup_id-4237303.md) | frapid_db_user |  |
| 36 | [get_frequency_setup_start_date_frequency_setup_id(_frequency_setup_id integer)RETURNS date](../functions/finance/get_frequency_setup_start_date_frequency_setup_id-4237304.md) | frapid_db_user |  |
| 37 | [get_income_tax_provison_amount(_office_id integer, _profit numeric, _balance numeric)RETURNS numeric](../functions/finance/get_income_tax_provison_amount-4237305.md) | frapid_db_user |  |
| 38 | [get_income_tax_rate(_office_id integer)RETURNS decimal_strict](../functions/finance/get_income_tax_rate-4237306.md) | frapid_db_user |  |
| 39 | [get_journal_view(_user_id integer, _office_id integer, _from date, _to date, _tran_id bigint, _tran_code character varying, _book character varying, _reference_number character varying, _statement_reference character varying, _posted_by character varying, _office character varying, _status character varying, _verified_by character varying, _reason character varying)RETURNS TABLE(transaction_master_id bigint, transaction_code text, book text, value_date date, book_date date, reference_number text, statement_reference text, posted_by text, office text, status text, verified_by text, verified_on timestamp with time zone, reason text, transaction_ts timestamp with time zone)](../functions/finance/get_journal_view-4237307.md) | frapid_db_user |  |
| 40 | [get_month_end_date(_office_id integer)RETURNS date](../functions/finance/get_month_end_date-4237272.md) | frapid_db_user |  |
| 41 | [get_month_start_date(_office_id integer)RETURNS date](../functions/finance/get_month_start_date-4237273.md) | frapid_db_user |  |
| 42 | [get_net_profit(_date_from date, _date_to date, _office_id integer, _factor integer, _no_provison boolean DEFAULT false)RETURNS numeric](../functions/finance/get_net_profit-4237337.md) | frapid_db_user |  |
| 43 | [get_new_transaction_counter(date)RETURNS integer](../functions/finance/get_new_transaction_counter-4237308.md) | frapid_db_user |  |
| 44 | [get_office_id_by_cash_repository_id(integer)RETURNS integer](../functions/finance/get_office_id_by_cash_repository_id-4237309.md) | frapid_db_user |  |
| 45 | [get_periods(_date_from date, _date_to date)RETURNS finance.period[]](../functions/finance/get_periods-4237310.md) | frapid_db_user |  |
| 46 | [get_profit_and_loss_statement(_date_from date, _date_to date, _user_id integer, _office_id integer, _factor integer, _compact boolean DEFAULT true)RETURNS json](../functions/finance/get_profit_and_loss_statement-4237338.md) | frapid_db_user |  |
| 47 | [get_quarter_end_date(_office_id integer)RETURNS date](../functions/finance/get_quarter_end_date-4237274.md) | frapid_db_user |  |
| 48 | [get_quarter_start_date(_office_id integer)RETURNS date](../functions/finance/get_quarter_start_date-4237275.md) | frapid_db_user |  |
| 49 | [get_retained_earnings(_date_to date, _office_id integer, _factor integer)RETURNS numeric](../functions/finance/get_retained_earnings-4237340.md) | frapid_db_user |  |
| 50 | [get_retained_earnings_statement(_date_to date, _office_id integer, _factor integer)RETURNS TABLE(id integer, value_date date, tran_code text, statement_reference text, debit numeric, credit numeric, balance numeric, office text, book text, account_id integer, account_number text, account text, posted_on timestamp with time zone, posted_by text, approved_by text, verification_status integer)](../functions/finance/get_retained_earnings_statement-4237311.md) | frapid_db_user |  |
| 51 | [get_root_account_id(_account_id integer, _parent integer DEFAULT 0)RETURNS integer](../functions/finance/get_root_account_id-4237313.md) | frapid_db_user |  |
| 52 | [get_sales_tax_account_id_by_office_id(_office_id integer)RETURNS integer](../functions/finance/get_sales_tax_account_id_by_office_id-4237314.md) | frapid_db_user |  |
| 53 | [get_second_root_account_id(_account_id integer, _parent bigint DEFAULT 0)RETURNS integer](../functions/finance/get_second_root_account_id-4237315.md) | frapid_db_user |  |
| 54 | [get_transaction_code(value_date date, office_id integer, user_id integer, login_id bigint)RETURNS text](../functions/finance/get_transaction_code-4237316.md) | frapid_db_user |  |
| 55 | [get_trial_balance(_date_from date, _date_to date, _user_id integer, _office_id integer, _compact boolean, _factor numeric, _change_side_when_negative boolean DEFAULT true, _include_zero_balance_accounts boolean DEFAULT true)RETURNS TABLE(id integer, account_id integer, account_number text, account text, previous_debit numeric, previous_credit numeric, debit numeric, credit numeric, closing_debit numeric, closing_credit numeric)](../functions/finance/get_trial_balance-4237341.md) | frapid_db_user |  |
| 56 | [get_value_date(_office_id integer)RETURNS date](../functions/finance/get_value_date-4237317.md) | frapid_db_user |  |
| 57 | [get_verification_status_name_by_verification_status_id(_verification_status_id integer)RETURNS text](../functions/finance/get_verification_status_name_by_verification_status_id-4237318.md) | frapid_db_user |  |
| 58 | [has_child_accounts(bigint)RETURNS boolean](../functions/finance/has_child_accounts-4237319.md) | frapid_db_user |  |
| 59 | [initialize_eod_operation(_user_id integer, _office_id integer, _value_date date)RETURNS void](../functions/finance/initialize_eod_operation-4237320.md) | frapid_db_user |  |
| 60 | [is_cash_account_id(_account_id integer)RETURNS boolean](../functions/finance/is_cash_account_id-4237321.md) | frapid_db_user |  |
| 61 | [is_eod_initialized(_office_id integer, _value_date date)RETURNS boolean](../functions/finance/is_eod_initialized-4237322.md) | frapid_db_user |  |
| 62 | [is_new_day_started(_office_id integer)RETURNS boolean](../functions/finance/is_new_day_started-4237323.md) | frapid_db_user |  |
| 63 | [is_normally_debit(_account_id integer)RETURNS boolean](../functions/finance/is_normally_debit-4237324.md) | frapid_db_user |  |
| 64 | [is_periodic_inventory(_office_id integer)RETURNS boolean](../functions/finance/is_periodic_inventory-4237325.md) | frapid_db_user |  |
| 65 | [is_restricted_mode()RETURNS boolean](../functions/finance/is_restricted_mode-4237326.md) | frapid_db_user |  |
| 66 | [is_transaction_restricted(_office_id integer)RETURNS boolean](../functions/finance/is_transaction_restricted-4237327.md) | frapid_db_user |  |
| 67 | [perform_eod_operation(_login_id bigint)RETURNS boolean](../functions/finance/perform_eod_operation-4237329.md) | frapid_db_user |  |
| 68 | [perform_eod_operation(_user_id integer, _login_id bigint, _office_id integer, _value_date date)RETURNS boolean](../functions/finance/perform_eod_operation-4237328.md) | frapid_db_user |  |
| 69 | [reconcile_account(_transaction_detail_id bigint, _user_id integer, _new_book_date date, _reconciliation_memo text)RETURNS void](../functions/finance/reconcile_account-4237330.md) | frapid_db_user |  |
| 70 | [verify_transaction(_transaction_master_id bigint, _office_id integer, _user_id integer, _login_id bigint, _verification_status_id smallint, _reason character varying)RETURNS bigint](../functions/finance/verify_transaction-4237331.md) | frapid_db_user |  |



**Triggers**

| # | Trigger | Owner | Description |
| --- | --- | --- | --- |
| 1 | [update_transaction_meta()RETURNS TRIGGER](../functions/finance/update_transaction_meta-4237343.md) | frapid_db_user |  |



**Types**

| # | Type | Base Type | Owner | Collation | Default | Type | StoreType | NotNull | Description |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | [period](../types/finance/period.md) | - | frapid_db_user |  |  | Composite Type | Compressed Inline/Seconary | False |  |


### Related Contents
* [Schema List](../schemas.md)
* [Table of Contents](../../README.md)
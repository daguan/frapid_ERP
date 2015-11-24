# Developer Guide

You need to sign the [contributor license agreement](cla.md) before you contribute to Frapid. Frapid is 
licensed under GNU/GPLv3 license. Please refer to the file contributing.md on the root directory.

# Internationalization

Internationalization is handled by the module `Frapid.i18n`. For more information, read the [internationalization guide](i18n.md).

# Data Access & Restful APIs

Frapid uses database first approach. Please follow these documents for more information:

- [Database of Frapid](db.md)
- [Creating Data Transfer Objects](dto.md)
- [Creating Data Access Repositories](data-access/repository.md)
- [Creating Data Access Classes](data-access/dac.md)
- [Creating Restful Web API](rest/api.md)
- [Writing Tests against Web APIs](rest/test.md)

# Frontend Development

For the frontend development tasks, Frapid uses:

- jQuery
- AngularJS
- Semantic UI
- MixERP ScrudFactory framework for CRUD operations.
- d3 and chartjs for Charting
- linq.js for client side LINQ
- papaparse for CSV parsing
- qunit for writing tests.
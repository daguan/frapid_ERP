### Overrides

Frapid searches for overridden views on the instance directory and loads them if found.

For example: the view:

`~/Areas/Frapid.Account/Views/SignUp/Index.cshtml`

would be overridden by:

`~/Catalogs/foo_com/Areas/Frapid.Account/Views/SignUp/Index.cshtml`

for foo.com and

`~/Catalogs/bar_com/Areas/Frapid.Account/Views/SignUp/Index.cshtml`

for bar.com.
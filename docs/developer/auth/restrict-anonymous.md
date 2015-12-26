# Preventing Anonymous Access to Controller Actions

You need to verify whether or not the user is allowed to access application resources. If you decorate a controller of action with ```RestrictAnonymous``` attribute, access to the controller of action will be forbidden to anonymous users.

The ```RestrictAnonymous``` attribute is extension of ASP.net **Authorize** attribute.


[Back to Developer Documentation](../readme.md)
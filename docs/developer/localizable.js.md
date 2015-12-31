# localizable.js

The localizable.js utility automatically translates localization literals depending on the current language of the application user.

## Usage

Include the localizable.js utility on your View.

```html
<script src="/Scripts/frapid/utilities/localizable.js"></script>
```

## Example

The localizable.js investigates html controls having the following attributes:

* data-localize
* data-localized-resource

### data-localize
**This**

```html
<div class="ui message">
    <span data-localize="Titles.CompanyName"></span>
</div>
```

** will be converted to (English) **

```html
<div class="ui message">
    Company Name
</div>
```

** or (German) **


```html
<div class="ui message">
    Firmenname
</div>
```

Note that the existing node (span) is replaced by the translated literal.


### data-localized-resource

**This**

```html
<input data-localized-resource="Titles.CompanyName" data-localization-target="value" />
```

** will be converted to (English) **

```html
<input data-localized-resource="Titles.CompanyName" data-localization-target="value"
    value="Company Name" />
```

** or (German) **

```html
<input data-localized-resource="Titles.CompanyName" data-localization-target="value"
    value="Firmenname" />
```

[Back to Internationalization](i18n.md)

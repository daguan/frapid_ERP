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
* data-localized-placeholder
* data-localized-title

### data-localize
**This**

```html
<div class="ui message">
    <span data-localize="Titles.CompanyName"></span>
</div>
```

**will be converted to (English)**

```html
<div class="ui message">
    <span>Company Name</span>
</div>
```

**or (German)**


```html
<div class="ui message">
    <span>Firmenname</span>
</div>
```

### data-localized-placeholder
**This**

```html
<input data-localized-placeholder="Titles.Customer" />
```

**will be converted to (English)**

```html
<input placeholder="Customer" />
```

**or (German)**


```html
<input placeholder="Kunde" />
```


### data-localized-title
**This**

```html
<input data-localized-title="Titles.Customer" />
```

**will be converted to (English)**

```html
<input title="Customer" />
```

**or (German)**


```html
<input title="Kunde" />
```


[Back to Internationalization](i18n.md)


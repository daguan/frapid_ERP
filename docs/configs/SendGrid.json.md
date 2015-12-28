# SendGrid Email API

When you start sending (several) hundreds of emails every day, you will soon realize that SMTP is not that efficient :

* The email conversion rate is low and declining.
* Most of your clients say that they do not get your emails at all.
* A few clients report back to you that your emails were found inside the "Junk" or "Spam" folder.
* An email server is not particularly happy with your speed of email submissions.
* An email server flags you as spammer, identifies you, and actively refuses your legitimate emails.

You lose business and this is a pretty sad.

![SendGrid Logo](images/sendgrid.png)

SendGrid specializes in email delivery service. This gives you a peace of mind that your emails are reaching out to your customers properly.
For up to 12000 emails per month, [SendGrid is absolutely free](https://sendgrid.com/pricing).

Since frapid has built-in support for SendGrid API, you just need to edit the configuration file `SendGrid.json` and you're good to go. 
Preferably, you can configure this from the [Admin Area](#) as well.

**~/Catalogs/<domain>/Configs/SMTP/SendGrid.json**
```json
{
    "FromName" : "",
    "FromEmail" : "",
	"ApiUser": "Your SendGrid User Name",
	"ApiKey": "Your SendGrid API Key",
	"Enabled": false
}
```

## Configuration Explained

| Key                           | Configuration|
|-------------------------------|---------------------------------------------------------|
| FromName                      | The name field for the `FromEmail` key. |
| FromEmail                     | The from email address to be displayed to the email recipients.|
| ApiUser                       | Your registered SendGrid user name. |
| ApiKey                        | The SendGrid API key. |
| Enabled                       | Set this to true if you want to use SendGrid API to send emails. If multiple email providers are enabled, the first one will be used. |


### Related Contents

* [Installing Frapid](../installation/README.md)
* [Developer Documentation](../developer/README.md)
* [Documentation Home](../../README.md)

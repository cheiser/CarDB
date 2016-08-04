# CarDB
This is a simple toy database for cars.

It uses outlook's SMTP service for sending confirmation emails when the user registers a new account for editing existing car entries. This requires a valid outlook account, the details of which are entered under the ```Web.config``` file (search for ```add key="mailAccount"``` to find where to enter details). If one wants to change the mail service, this can be done under ```App_Start/IdentityConfig.cs```.

## Getting Started
The SmallAPI service is a group of microservices that may come in handy when doing web/mobile development.
As of now it only has one API called SubscriberLists (see below for more info).

To use the SubscriberLists API, do the following:
* Obtain an API key
* Create a subscriber list
* Post an email to the subscriber list
* Retrieve all emails from the subscriber list

All these steps are outlined in detail below.


## Obtaining an API key

Issue the following request, substituting the email and company name for your own.

```httprequest
POST http://www.smallapi.com/apikey/
```

Body:
```
{ "Email":"someonesemail@gmail.com","Company":"SmallAPI" }
```

Save your API key somewhere safe!
You can only have one API key per email address.


## SubscriberLists API

The SubscriberList API allows you to create an unlimited number of lists and add emails to each list.

### Creating a new subscriber list

Request:
```httprequest
POST http://www.smallapi.com/subscriberlists/
```

Header:
```header
apikey: {API-KEY}
```

Body:
```
{ "Name" : "list1" }
```


### Getting all your lists 

Request:
```httprequest
GET http://www.smallapi.com/subscriberlists/
```

Header:
```header
apikey: {API-KEY}
```

### Adding a new email to a subscriber list

Request:
```httprequest
POST http://www.smallapi.com/subscriberlists/<listname>/emails/
```

Header:
```header
apikey: {API-KEY}
```

Body:
```
{ "Email": "john.doe@gmail.com" }
```

### Getting a list of all the emails on a subscriber list

Request:
```httprequest
GET http://www.smallapi.com/subscriberlists/<listname>/emails/
```

Header:
```header
apikey: {API-KEY}
```

### Deleting a subscriber list

#### Please note that all the subscribers will be deleted along with the list

Request:
```httprequest
DELETE http://www.smallapi.com/subscriberlists/?name=<listname>
```

Header:
```header
apikey: {API-KEY}
```

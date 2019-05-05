# RestfulUsersInventory

A .NET Core RESTful web service using a SQLite database to expose a user's item inventory.

## Pre-requisites

To run the service, .NET Core 2.2 SDK is required. The project is a standard .NET Core application so
can be run from the command line (though it is easier to run from Visual Studio by opening the solution).

## Example API calls

Upon running the app, here are some example API calls you can make (e.g. through Postman). The example responses are when the database is in its initialised state.

- **[Get all users](#Get-All-Users)**
- **[Get a user](#Get-A-User)**
- **[Get all items](#Get-All-Items)**
- **[Get an item](#Get-An-Item)**
- **[Get user's total inventory](#Get-Users-Total-Inventory)**
- **[Get user's inventory for specific item](#Get-Users-Inventory-For-Specific-Item)**
- **[Add items to a user's inventory](#Add-Items-To-A-Users-Inventory)**
- **[Remove items from a user's inventory](#Remove-Items-From-A-Users-Inventory)**

### **[Get all users](#Get-All-Users)**
[Return to API List](##Example-Api-Calls)

```
https://localhost:44301/api/users or
http://localhost:52608/api/users
HTTP GET
```
should return
```javascript
[
  {
    "id": 1,
    "name": "Rick Sanchez"
  },
  {
    "id": 2,
    "name": "Morty Smith"
  }
]
```
### **[Get a user](#Get-A-User)**
[Return to API List](##Example-Api-Calls)

```
https://localhost:44301/api/users/1 or
http://localhost:52608/api/users/1
HTTP GET
```
should return
```javascript
{
  "id": 1,
  "name": "Rick Sanchez"
}
```
### **[Get all items](#Get-All-Items)**
[Return to API List](##Example-Api-Calls)

```
https://localhost:44301/api/items or
http://localhost:52608/api/items
HTTP GET
```
should return
```javascript
[
  {
    "id": 1,
    "name": "Longsword"
  },
  {
    "id": 2,
    "name": "Claymore"
  },
  {
    "id": 3,
    "name": "Dagger"
  }
]
```
### **[Get an item](#Get-An-Item)**
[Return to API List](##Example-Api-Calls)

```
https://localhost:44301/api/items/1 or
http://localhost:52608/api/items/1
HTTP GET
```
should return
```javascript
{
  "id": 1,
  "name": "Longsword"
}
```
### **[Get user's total inventory](#Get-Users-Total-Inventory)**
[Return to API List](##Example-Api-Calls)

```
https://localhost:44301/api/users/1/items or
http://localhost:52608/api/users/1/items
HTTP GET
```
should return
```javascript
[
  {
    "userId": 1,
    "userName": "Rick Sanchez",
    "itemId": 1,
    "itemName": "Longsword",
    "itemCount": 46
  },
  {
    "userId": 1,
    "userName": "Rick Sanchez",
    "itemId": 2,
    "itemName": "Claymore",
    "itemCount": 1
  }
]
```
### **[Get user's inventory for specific item](#Get-Users-Inventory-For-Specific-Item)**
[Return to API List](##Example-Api-Calls)

```
https://localhost:44301/api/users/1/items/1 or
http://localhost:52608/api/users/1/items/1
HTTP GET
```
should return
```javascript
{
  "userId": 1,
  "userName": "Rick Sanchez",
  "itemId": 1,
  "itemName": "Longsword",
  "itemCount": 46
}
```
### **[Add items to a user's inventory](#Add-Items-To-A-Users-Inventory)**
[Return to API List](##Example-Api-Calls)
```
https://localhost:44301/api/users/1/items or
http://localhost:52608/api/users/1/items
HTTP POST
```
with one of the following payloads
```javascript
[{ "name": "Claymore" }]
or
[{ "id": 2 }]
or
[{"id": 2,"name": "Claymore"}]
```
should return
```javascript
[
  {
    "userId": 1,
    "userName": "Rick Sanchez",
    "itemId": 1,
    "itemName": "Longsword",
    "itemCount": 46
  },
  {
    "userId": 1,
    "userName": "Rick Sanchez",
    "itemId": 2,
    "itemName": "Claymore",
    "itemCount": 2
  }
]
```
Sending multiple items will add a range up until the maximum item count of 50 e.g. payload:
```javascript
[{ "name": "Longsword" },{ "name": "Longsword" },{ "name": "Longsword" },{ "name": "Longsword" },{ "name": "Longsword" }]
```
### **[Remove items from a user's inventory](#Remove-Items-From-A-Users-Inventory)**
[Return to API List](##Example-Api-Calls)
```
https://localhost:44301/api/users/1/items or
http://localhost:52608/api/users/1/items
HTTP DELETE
```
with one of the following payloads
```javascript
[{ "name": "Longsword" }]
or
[{ "id": 1 }]
or
[{"id": 1,"name": "Longsword"}]
```
should return
```javascript
[
  {
    "userId": 1,
    "userName": "Rick Sanchez",
    "itemId": 1,
    "itemName": "Longsword",
    "itemCount": 45
  }
]
```
Sending multiple items willremove a range until there are 0.
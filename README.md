# Basket Api Prototype

## .NET challenge Description
  Your company has decided to create a new line of business.  As a start to this effort, they’ve come to you to help develop a prototype.  It is expected that this prototype will be part of a beta test with some actual customers, and if successful, it is likely that the prototype will be expanded into a full product.
  Your part of the prototype will be to develop a Web API that will be used by customers to manage a basket of items. The business describes the basic workflow is as follows
  
* This API will allow our users to set up and manage an order of items.  The API will allow users to add and remove items and change the quantity of the items they want.  They should also be able to simply clear out all items from their order and start again.
* The functionality to complete the purchase of the items will be handled separately and will be written by a different team once this prototype is complete.  
* For the purpose of this exercise, you can assume there’s an existing data storage solution that the API will use, so you can either create stubs for that functionality or simply hold data in memory.
* Create a client library that makes use of the API endpoints.  The purpose of this code to provide authors of client applications a simple framework to use in their applications.

## Projects in the solution
* BasketApp.Api - Prototype Basket REST API
* BasketApp.DAL - EF core code first with Migrations 
* BasketApp.Client - .NET client
* BasketApp.Common - contracts and common utilities
* BasketApp.IntegrationTests - simple integration tests with the use of BasketApp.Client

## 'Extra' features
* Sqlite database with Migrations (run Update-Database on BasketApp.Api)
* IdentityUser & Authentication via JWT Bearer Token
* Seed Test User and Products Data on Initialize WebHost
* Serilog to have log files 
* MemoryCache on Get Products(_Controller_)
* HealthChecks
* Swagger / OpenAPI endpoint documentation

## Basket Api Endpoints
* _post_    : api/account/token     : Generate JWT Token
* _get_     : api/products          : Get All Product Infos From Database
* _get_     : api/basket/           : Get Authenticated User Basket
* _post_    : api/basket/items      : Add item to Auth User Basket
* _put_     : api/basket/items/{id} : Update amount of product in the Auth User Basket
* _delete_  : api/basket/items/{id} : Remove item from the Auth User Basket
* _delete_  : api/basket/clear      : Clears all items from the Auth User basket
* _get_     : healthcheck       : For reporting the health of app infrastructure components
* _get_     : swagger           : Endpoint & models documentation

## ApiClient
* AuthenticateAsync: adds token as a header of internal httpClient
_The follow calls requires previous authentication/authorization_
* GetProductsAsync: returns list of products
* GetUserBasket: return list of items in the user basket
* AddItemToBasket: adds item to the user basket
* UpdateItemAmountInBasket: update amount of a determinated item in the user basket
* RemoveItemFromBasketAsync: remove determinated item (no matter the amount) from the user basket
* ClearBasketAsync: clear all items from the user basket

## Create Sqlite Database
```
dotnet ef database update
```

## Postman [manual example tests]
* https://www.getpostman.com/collections/eeee34352a96e89533ee

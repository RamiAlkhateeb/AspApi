# products-api-dotnet-core

Create user account api:

POST

http://localhost:5000/api/users

and the body should be : 

{

    "Name": "Ammar",

    "Address": "Damascus",

    "Age": 20
    
}

Order a product api:
POST
http://localhost:5000/api/orders
{
    "UserId" : 1,
    "ProductId" : 2,
}

Create a new product api :
POST
http://localhost:5000/api/products
and the body should be : 
{
    "Name": "iphone 5",
    "Category": "Mobiles",
    "Price": 120
}

Modify an existing product api :
PUT
http://localhost:5000/api/products/2
and the body should be : 
{
    "Name": "iphone 5",
    "Category": "Mobiles",
    "Price": 130
}

Get Order infomation api:
GET
http://localhost:5000/api/orders/2
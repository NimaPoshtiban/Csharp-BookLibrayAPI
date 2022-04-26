# Csharp-BookLibray

An api built with ASP .NET 6 with authorization

## API Reference

#### Register 
```http
POST /api/Auth/register
```

#### Login
```http
POST /api/Auth/login
```
(Authorized)
#### Get All Books 

```http
GET /api/Books
```

#### Add a Book 
```http
POST /api/Books
```

####  Get a Book by Id 

```http
GET /api/Books/{id}
```

#### Update a Book 
```http
PUT /api/Books/{id}
```
#### Remove a Book 
```http 
DELETE /api/Books/{id}
```

#### Get All Authors

```http
GET /api/Authors
```

#### Add an Author

```http 
POST /api/Authors
```

#### Get an Author by Id

```http
GET /api/Authors/{id}
```

#### Update an Author
```http
PUT /api/Authors/{id}
```

#### Remove an Author
```http 
DELETE /api/Authors/{id}
```

## Tech Stack


**Server:**  C# ASP.NET Core API 
**Log Server:**  Seq 
**Database:**  MSSQL
**Logger:** Serilog

### How to use
By default you can only register with a user role.
<br/>
any modification requires you to be in Admin Role.
So you should manually add admin role and admin user.


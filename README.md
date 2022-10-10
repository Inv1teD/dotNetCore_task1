# dotNetCore

Firstly you need to setup a PostgerSQL database in config file appsettings.json in "DefaultConnection", where:

"Host={host};Port={port};Database={database_name};Username={psql_username};Password={psql_user_password};"

ENDPOINTS:
1.  GET "/api/users":
    
    Get a list of all users from db;
2.  GET "/api/users/[id]":
    
    Get a data about user with specific id;
3.  POST "/api/users":
    
    Create a new user with data. Pass data in json:
    
    Example: Request Body: 
    POST {
    "name": "Danyil",
    "city": "Kharkiv",
    "is_Angry": true
     }
4.  DELETE "/api/users/[id]":
    
    Delete a user with specific id;

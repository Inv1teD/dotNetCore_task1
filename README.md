# dotNetCore

Firstly you need to setup a PostgerSQL database in config file appsettings.json in "DefaultConnection", where:

"Host={host};Port={port};Database={database_name};Username={psql_username};Password={psql_user_password};"

All data must be passed through request body in json.

ENDPOINTS:
1.  GET "/api/ukrainians":
    
    Get a list of all users from db;
2.  GET "/api/ukrainians/[id]":
    
    Get a data about user with specific id;
3.  POST "/api/ukrainians":
    
    Create a new user with data.
    
    Example: Request Body: 
    POST {
    "name": "Danyil",
    "city": "Kharkiv",
    "isCalm": false
     }
4. PUT "/api/ukrainians/[id]":

    Change data about specific user:
    Example: Request Body: 
    PUT {
    "name": "Danyil",
    "city": "Kharkiv",
    "isCalm": true
     }

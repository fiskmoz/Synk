# Synk

Getting started: 
- Install dependencies (nuget cmd)
- Run Update-Database to make all migrations to the DB.


When deploying: 
- Change the secret that is used to create secure JWT token. 
- docker-compose build
- docker-compose up

Running .NET core 2.2 in docker containers

Includes: 
- Swagger api testing and documentation
- Jwt token authentication, login and registration, refreshing JWT with refresh tokens.
- Individual user accounts

Dependencies: 
- Swagger 
- Entity framework 
- MSSQL DB

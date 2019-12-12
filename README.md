# Synk

Getting started without docker: 
- sdasd
- Install dependencies (nuget cmd)
- Run Update-Database to make all migrations to the DB.

With docker: 
- run docker-compose build in root folder
- run docker-compose up
- navigate to localhost port 7000. DB port is 1400


When deploying: 
- Change the secret that is used to create secure JWT token. 
- docker-compose build
- docker-compose up

Running .NET core 2.2

Includes: 
- Swagger api testing and documentation
- Jwt token authentication, login and registration, refreshing JWT with refresh tokens.
- Individual user accounts

Dependencies: 
- Swagger 
- Entity framework 
- MSSQL DB

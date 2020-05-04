# Synk

Dependencies:   
 .Net core 3.0 sdk   
`npm install -g @angular/cli`   
`npm install nswag -g `   
 MSSQL server   

Getting started without docker: 
- Run BE (will automatically install nugetpackages)
- Run Update-Database in nugetterminal (target DB in appsettings.json)

With docker: 
- run docker-compose build in root folder
- run docker-compose up
- navigate to localhost port 7000. DB port is 1400

Starting FE: 
- navigate to fe root folder.
- `npm install`
- `ng serve`
- fe hosted locally on port 4200

When deploying: 
- Change the secret that is used to create secure JWT token. 
- `ng build --prod`
- docker-compose build
- docker-compose up

Includes: 
- Swagger api testing and documentation
- Jwt token authentication, login and registration, refreshing JWT with refresh tokens.
- Individual user accounts

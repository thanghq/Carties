# Prerequisite 

Some tool/package that will be used in the project
* Nodejs 
* Dotnet core 8.0
* Docker
* Postgres
* MongoDB

For developer, VS Code is suggested with following extensions:
* .Net devkit v1.2.5
* C# v2.14.8
* .NET Install Tool v2.0.0
* Docker v1.28.0
* PostgreSQL v1.4.3
* NuGet Gallery v0.0.24
* MongoDB for VS Code v1.3.1

# Dotnet core project with NextJS

To run the project follow steps below:

* install dotnet core 8.0 SDK
* install docker
* install dotnet-ef tool with ```dotnet tool install --global dotnet-ef```
* run ```docker compose up -d```
* run ```dotnet ef database udpate``` to generate tables into your postgres
* run ```dotnet watch``` if you want to run in watch mode

# Some side command that would be use alot

* ```dotnet tool list -g``` this command list all the tool that had been installed globaly (most of the case is to check if we had the entity framework tool installed or not)
* ```dotnet ef database drop``` this command will drop your database
* ```dotnet ef Migrations add <name-of-migration>``` this command will create a migrations script into the folder ```Migrations``` and name of the migration will be ```<timestamp>_<name-of-migration>.cs```
* ```dotnet ef database udpate``` this command will update your database with the migrations script created.

# Alternative solution for Duende Identity server:
Because Duende Identity server (former is IdentityService4) is not free, hence you may wanna try with this:  ```https://github.com/Aguafrommars/Identity.Redis```
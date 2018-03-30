# Prerequisite:

* Windows 10
* Visual Studio 2017
* .Net Core SDK
* Docker 
* Kitematic: along with docker tools you will get kitematic.It's a UI for docker containers and logs

# Execute Database script "/sql/Coreservice.sql" :
   * It will create coreservices databse with required tables and sps
   * Update connection string with your database connections.

# NewRelic download and installation:
  * Download below from http://download.newrelic.com/dot_net_agent/core_20/current  
	 1. newrelic-netcore20-agent-win_8.0.0.0_x86.zip
	 2. newrelic-netcore20-agent_8.0.0.0_amd64.deb 
  * Extract zip file to "newrelic" folder to root of your app where docker file exists
  * Add below line in .dockerignore file
	   !newrelic
  * Register into Newrelic and get NEW_RELIC_LICENSE_KEY
  * Replace this NEW_RELIC_LICENSE_KEY value in dockerfile 

# Run application using below steps:
* Open command promt and go to ../src/Core.Services
* dotnet build
* dotnet publish -o obj/Docker/publish
* docker build -t coreimage .
* See images using command,it will list your image "coreimage" also
	 docker images
* docker run -d -p 28601:8601 --name core1 --env ASPNETCORE_ENVIRONMENT=QA coreimage
* See conatainers running using command,it will lsit your container "core1" also
	docker ps 

# Access Apis by passing apikey in header : apikey:67cd910a-6a6d-4cb3-b57b-92f7a4dee9c2
 * To GetList of Employees: 
 	 http://localhost:28601/service/v1/
 * To GetList of Employees: 
 	 http://localhost:28601/service/v1/{id}

# Check Kitematic for logs
   
# To see NewRelic logs
 * Run docker image with NewRelic app name
  docker run -d -p 28601:8601 --name core1 --env ASPNETCORE_ENVIRONMENT=QA coreimage --env NEW_RELIC_APP_NAME=CoreServiceApp
 * Go to newrelic and click on Application and select your app "CoreServiceApp"
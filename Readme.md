## Prerequisite:

* Windows 10
* Visual Studio 2017
* .Net Core SDK : Download and install from    
  1. https://www.microsoft.com/net/learn/get-started/windows#windowscmd
* Docker : https://docs.docker.com/docker-for-windows/install/#download-docker-for-windows 
* Docker Toolbox :https://docs.docker.com/toolbox/toolbox_install_windows/
  1. It will provide Kitematic .It's a Docker UI for docker containers and logs.

## Database
If you want to use your local database then Execute Database script "/sql/Coreservice.sql" :
   * It will create coreservices databse with required tables and sps
   * Update connection string with your database connections.

## NewRelic 
* Case1: If you donot want to use NewRelic then comment below three lines in DockerFile :   ../src/Core.Services/Dockerfile :
 1. ARG NewRelic=./newrelic
 2. COPY $NewRelic ./newrelic 
 3. RUN dpkg -i ./newrelic/newrelic-netcore20-agent*.deb

* Case2: In you want to use NewRelic monitoring:
  * Download below from http://download.newrelic.com/dot_net_agent/core_20/current  
	 1. newrelic-netcore20-agent_8.0.0.0_amd64.deb 
  * Create a folder named "newrelic" inside ../src/Core.Services
  * Add below line in .dockerignore file : 
	   !newrelic
  * Register into Newrelic and get NEW_RELIC_LICENSE_KEY
  * Replace this NEW_RELIC_LICENSE_KEY value in dockerfile 
 

## Run application using below steps:
* Open command promt and go to ../src/Core.Services
* dotnet restore
* dotnet build
* dotnet publish -o obj/Docker/publish
* docker build -t coreimage . 
* See images using command "docker images",it will list your image "coreimage" also
* docker run -d -p 28601:8601 --name core1 --env ASPNETCORE_ENVIRONMENT=QA coreimage
     1. d  => detached mode
     2. p => host protocol and here you will acees api by localhost:28601
     3. name => container name
     4. env  => Environmentname like QA/Development/Staging/PROD for running transformation 
* See conatainers running using command "docker ps",it will lsit your container "core1" also

## Access Apis

 * http://localhost:28601/service/version
 * To access below apis ,you need to pass authentication key in header
    apikey:67cd910a-6a6d-4cb3-b57b-92f7a4dee9c2
    1. To GetList of Employees: 
      http://localhost:28601/service/v1/
    2. To GetList of Employees: 
      http://localhost:28601/service/v1/{id}

## Check Kitematic for logs
   
## To see NewRelic logs
 * Run docker image with NewRelic app name
  docker run -d -p 28601:8601 --name core1 --env ASPNETCORE_ENVIRONMENT=QA coreimage --env NEW_RELIC_APP_NAME=CoreServiceApp
 * Go to newrelic and click on Application and select your app "CoreServiceApp"

## Run Unit Tests
  1. Open command prompt and go to unit test project : ../src/Core.Services.Tests.Unit
  2. run tests using command : dotnet test
     
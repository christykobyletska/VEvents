# VEvents
An application for viewing and sharing events.

How to start:
1. Install MongoDB: https://www.mongodb.com/try/download/community
2. Install Docker Desktop: https://hub.docker.com/editions/community/docker-ce-desktop-windows/
3. Install MS SQL server or pull docker image + configure it to allow remote connections and enable SQL authentication
4. Install and run MongoDB image for Docker by running following commands in PowerShell console:
   - docker pull mongo
   - docker run --name my_mongo -p 27017:27017 -d mongo
5. Install and run Redis image for Docker by running following command in PowerShell console (behorehand switch Docker to Linux):
   - docker pull redis
   - docker run --name local-redis -p 6379:6379 -d redis 
6. Update SqlConnection setting in appsettings.json file for VEvents.Web project with current server IP address.
7. Update MongoDbConnection setting in appsettings.json files for VEvents.Poller and VEvents.Api projects with current server IP address.
8. Update RedisConnection setting in appsettings.json file for VEvents.Api project with current server IP address.
9. Open VEvents.sln and run follwoing command in Package Manager Console:
   - Update-Database
10. Run application in following order:
   - VEvents.Api
   - VEvents.Web
   - VEvents.Poller (for getting events from fake source)
# VEvents
A simple service for convenient and fast events sharing.

How to start:
1. Install MongoDB: https://www.mongodb.com/try/download/community
2. Install Docker Desktop: https://hub.docker.com/editions/community/docker-ce-desktop-windows/
3. Install and run MongoDB image for Docker:
   > docker pull mongo
   > docker run --name my_mongo -p 27017:27017 -d mongo
4. Open EventShare.sln and run follwoing command in Package Manager Console:
   > Update-Database
5. Run application in following order:
   - EventShare.Api
   - EventShare.Web
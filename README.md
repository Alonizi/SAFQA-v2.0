# Installation Guide

- with your console navigate to SAFQA v2.0 directory
- run the command `docker-compose up` and wait for the containers to run
- navigate to DockerDBMigrator
- run command `dotnet ef database update` to initialize the database tables
- to test the APIs , add the collections in the Github repo `/Postman Collections` to your postman environment

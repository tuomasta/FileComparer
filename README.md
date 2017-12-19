# FileComparer
Simple web API to store and compare files.

## Run
* Clone
* cd FileComparer
* dotnet restore
* dotnet run --project FileComparer/FileComparer.csproj
* open uri http://localhost:5000/swagger/


or

* start *.sln
* Hit F5 to debug


## Endpoints
*{host}/api/v1/files/{id}*

Supports GET, POST and DELETE to manage files

*{host}/right/{rightId}/left/{leftId}*

Endpoint for comparing the files. *rightId* is the id of the right hand side 'old' file and the *leftId* is the id of the left hand side 'new' file id.

*{host}/swagger/*

Swagger UI for documentation and testing.
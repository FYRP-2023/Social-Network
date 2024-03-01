### Here, it contains docker containers for social network CRUD API and Semantic Search API. Please note that semantic search API might take up to 7 GB of disk space.

#### In Unix-based operating systems, execute the following commands to start the Docker containers.
```
chmod +x start.sh

./start.sh
```

#### In Windows-based operating systems, execute the following command to start the Docker containers.
```
powershell -ExecutionPolicy ByPass -File .\start.ps1
```

#### API URLs
Social Network CRUD API: http://localhost:8080/swagger/index.html (Hosted URL: https://ayurconnect-service-1.azurewebsites.net/swagger/index.html)
<br />
Semantic Search API: http://localhost:8000/docs
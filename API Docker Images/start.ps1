# List of tar files and their download URLs
$tar_files = @{
    "ayurconnect-service-1.tar" = "https://drive.google.com/file/d/1vZO0ySc18DUZkaAe4O5jx_PmVEGubcp5/view?usp=sharing";
    "semantic-search.tar" = "https://drive.google.com/file/d/19qz2vL5kfwnhSxD4qZbUwlLscYM0aF7a/view?usp=sharing"
}

# Download and load each tar file as a Docker image
foreach ($file in $tar_files.GetEnumerator()) {
    Write-Host "Downloading $($file.Name)..."
    Invoke-WebRequest -Uri $file.Value -OutFile $file.Name

    Write-Host "Loading $($file.Name)..."
    docker load -i $file.Name
}

# Start the Docker containers
Write-Host "Starting Docker containers..."

docker run -d --name ayurconnect-service-1-container -p 8080:80 ayurconnect-service-1
docker run -d --name semantic-search-container -p 8000:8000 semantic-search

Write-Host "Docker containers started successfully."

#!/bin/bash

# List of tar files and their download URLs
declare -A tar_files
tar_files=(
    ["ayurconnect-service-1.tar"]="https://drive.google.com/file/d/1vZO0ySc18DUZkaAe4O5jx_PmVEGubcp5/view?usp=sharing"
    ["semantic-search.tar"]="https://drive.google.com/file/d/19qz2vL5kfwnhSxD4qZbUwlLscYM0aF7a/view?usp=sharing"
)

# Download and load each tar file as a Docker image
for file in "${!tar_files[@]}"
do
    echo "Downloading $file..."
    wget ${tar_files[$file]}

    echo "Loading $file..."
    docker load -i "$file"
done

# Start the Docker containers
echo "Starting Docker containers..."

docker run -d --name ayurconnect-service-1-container -p 8080:80 ayurconnect-service-1
docker run -d --name semantic-search-container -p 8000:8000 semantic-search

echo "Docker containers started successfully."

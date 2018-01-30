$imagesToDelete = docker images --filter=reference="HMS/*" -q

If (-Not $imagesToDelete) {Write-Host "Not deleting HMS images as there are no HMS images in the current local Docker repo."} 
Else 
{
    # Delete all containers
    Write-Host "Deleting all containers in local Docker Host"
    docker rm $(docker ps -a -q) -f
    
    # Delete all HMS images
    Write-Host "Deleting hms images in local Docker repo"
    Write-Host $imagesToDelete
    docker rmi $(docker images --filter=reference="HMS/*" -q) -f
}


# DELETE ALL IMAGES AND CONTAINERS

# Delete all containers
# docker rm $(docker ps -a -q) -f

# Delete all images
# docker rmi $(docker images -q)

#Filter by image name (Has to be complete, cannot be a wildcard)
#docker ps -q  --filter=ancestor=HMS/identity.api:dev


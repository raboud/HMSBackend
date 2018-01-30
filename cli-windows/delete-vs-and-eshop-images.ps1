 # Delete all containers
 Write-Host "Deleting all running containers in the local Docker Host"
 docker rm $(docker ps -a -q) -f

$hmsImagesToDelete = docker images --filter=reference="HMS/*" -q
If (-Not $hmsImagesToDelete) {Write-Host "Not deleting HMS images as there are no HMS images in the current local Docker repo."} 
Else 
{    
    # Delete all HMS images
    Write-Host "Deleting HMS images in local Docker repo"
    Write-Host $hmsImagesToDelete
    docker rmi $(docker images --filter=reference="HMS/*" -q) -f
}

$VSImagesToDelete = docker images --filter=reference="catalog.api:dev" -q
If (-Not $VSImagesToDelete) {Write-Host "Not deleting VS images as there are no VS images in the current local Docker repo."} 
Else 
{    
    # Delete all HMS images
    Write-Host "Deleting images created by VS in local Docker repo"
    Write-Host $VSImagesToDelete
    docker rmi $(docker images --filter=reference="*:dev" -q) -f
    
    #docker rmi $(docker images --filter=reference="HMS/payment.api:dev" -q) -f
    #docker rmi $(docker images --filter=reference="HMS/webspa:dev" -q) -f
    #docker rmi $(docker images --filter=reference="HMS/webmvc:dev" -q) -f
    #docker rmi $(docker images --filter=reference="HMS/catalog.api:dev" -q) -f
    #docker rmi $(docker images --filter=reference="HMS/marketing.api:dev" -q) -f
    #docker rmi $(docker images --filter=reference="HMS/ordering.api:dev" -q) -f
    #docker rmi $(docker images --filter=reference="HMS/basket.api:dev" -q) -f
    #docker rmi $(docker images --filter=reference="HMS/identity.api:dev" -q) -f
    #docker rmi $(docker images --filter=reference="HMS/locations.api:dev" -q) -f
    #docker rmi $(docker images --filter=reference="HMS/webstatus:dev" -q) -f
}

# DELETE ALL IMAGES AND CONTAINERS

# Delete all containers
# docker rm $(docker ps -a -q) -f

# Delete all images
# docker rmi $(docker images -q)

#Filter by image name (Has to be complete, cannot be a wildcard)
#docker ps -q  --filter=ancestor=HMS/identity.api:dev


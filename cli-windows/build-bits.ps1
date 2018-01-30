Param([string] $rootPath)
$scriptPath = Split-Path $script:MyInvocation.MyCommand.Path

Write-Host "Current script directory is $scriptPath" -ForegroundColor Yellow

if ([string]::IsNullOrEmpty($rootPath)) {
    $rootPath = "$scriptPath\.."
}
Write-Host "Root path used is $rootPath" -ForegroundColor Yellow

workflow BuildAndPublish {
    param ([string] $rootPath
    )
$projectPaths = 
    @{Path="$rootPath\src\Web\WebMVC";Prj="WebMVC.csproj"},
    @{Path="$rootPath\src\Web\WebSPA";Prj="WebSPA.csproj"},
    @{Path="$rootPath\src\Services\Identity\Identity.API";Prj="Identity.API.csproj"},
    @{Path="$rootPath\src\Services\Catalog\Catalog.API";Prj="Catalog.API.csproj"},
    @{Path="$rootPath\src\Services\Ordering\Ordering.API";Prj="Ordering.API.csproj"},
    @{Path="$rootPath\src\Services\Basket\Basket.API";Prj="Basket.API.csproj"},
    @{Path="$rootPath\src\Services\Location\Locations.API";Prj="Locations.API.csproj"},
    @{Path="$rootPath\src\Services\Marketing\Marketing.API";Prj="Marketing.API.csproj"},
    @{Path="$rootPath\src\Services\Payment\Payment.API";Prj="Payment.API.csproj"},
    @{Path="$rootPath\src\Web\WebStatus";Prj="WebStatus.csproj"}

    foreach -parallel ($item in $projectPaths) {
        $projectPath = $item.Path
        $projectFile = $item.Prj
        $outPath = $item.Path + "\obj\Docker\publish"
        $projectPathAndFile = "$projectPath\$projectFile"
        #Write-Host "Deleting old publish files in $outPath" -ForegroundColor Yellow
        remove-item -path $outPath -Force -Recurse -ErrorAction SilentlyContinue
        #Write-Host "Publishing $projectPathAndFile to $outPath" -ForegroundColor Yellow
        dotnet publish $projectPathAndFile -o $outPath -c Release
    }
}

BuildAndPublish $rootPath

########################################################################################
# Delete old HMS Docker images
########################################################################################

$imagesToDelete = docker images --filter=reference="HMS/*" -q

If (-Not $imagesToDelete) {Write-Host "Not deleting HMS images as there are no HMS images in the current local Docker repo."} 
Else 
{
    # Delete all containers
    Write-Host "Deleting all containers in local Docker Host"
    docker rm $(docker ps -a -q) -f
    
    # Delete all HMS images
    Write-Host "Deleting HMS images in local Docker repo"
    Write-Host $imagesToDelete
    docker rmi $(docker images --filter=reference="HMS/*" -q) -f
}

# WE DON'T NEED DOCKER BUILD AS WE CAN RUN "DOCKER-COMPOSE BUILD" OR "DOCKER-COMPOSE UP" AND IT WILL BUILD ALL THE IMAGES IN THE .YML FOR US

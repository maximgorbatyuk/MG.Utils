param (    
    [Parameter(Mandatory=$true)][string]$apiKey
)
dotnet nuget push MG.Utils.1.0.0.nupkg --api-key $apiKey --source https://api.nuget.org/v3/index.json


#Get-ChildItem -Path ./MG*/*.nuspec


dotnet build ./../src/MG.Utils/
nuget pack ./../src/MG.Utils/

dotnet build ./../src/MG.Utils.Abstract/
nuget pack ./../src/MG.Utils.Abstract/

dotnet build ./../src/MG.Utils.AspNetCore/
nuget pack ./../src/MG.Utils.AspNetCore/

dotnet build ./../src/MG.Utils.AspNetCore.I18N/
nuget pack ./../src/MG.Utils.AspNetCore.I18N/

dotnet build ./../src/MG.Utils.AspNetCore.Middlewares/
nuget pack ./../src/MG.Utils.AspNetCore.Middlewares/

dotnet build ./../src/MG.Utils.AspNetCore.Redis/
nuget pack ./../src/MG.Utils.AspNetCore.Redis/

dotnet build ./../src/MG.Utils.AspNetCore.Swagger/
nuget pack ./../src/MG.Utils.AspNetCore.Swagger/

dotnet build ./../src/MG.Utils.Authentication/
nuget pack ./../src/MG.Utils.Authentication/

dotnet build ./../src/MG.Utils.Azure.Authentication/
nuget pack ./../src/MG.Utils.Azure.Authentication/

dotnet build ./../src/MG.Utils.Azure.Logging/
nuget pack ./../src/MG.Utils.Azure.Logging/

dotnet build ./../src/MG.Utils.Azure.ServiceBus/
nuget pack ./../src/MG.Utils.Azure.ServiceBus/

dotnet build ./../src/MG.Utils.Export/
nuget pack ./../src/MG.Utils.Export/

dotnet build ./../src/MG.Utils.Smtp/
nuget pack ./../src/MG.Utils.Smtp/

dotnet build ./../src/MG.Utils.Validation/
nuget pack ./../src/MG.Utils.Validation/

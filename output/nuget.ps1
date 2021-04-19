#Get-ChildItem -Path ./MG*/*.nuspec


dotnet build ./../MG.Utils/MG.Utils.csproj
nuget pack ./../MG.Utils/MG.Utils.nuspec

dotnet build ./../MG.Utils/MG.Utils.Abstract.csproj
nuget pack ./../MG.Utils.Abstract/MG.Utils.Abstract.nuspec

nuget pack ./../MG.Utils.AspNetCore/MG.Utils.AspNetCore.nuspec

nuget pack ./../MG.Utils.AspNetCore.I18N/MG.Utils.AspNetCore.I18N.nuspec

nuget pack ./../MG.Utils.AspNetCore.Middlewares/MG.Utils.AspNetCore.Middlewares.nuspec

nuget pack ./../MG.Utils.AspNetCore.Redis/MG.Utils.AspNetCore.Redis.nuspec

nuget pack ./../MG.Utils.AspNetCore.Swagger/MG.Utils.AspNetCore.Swagger.nuspec

nuget pack ./../MG.Utils.Authentication/MG.Utils.Authentication.nuspec

nuget pack ./../MG.Utils.Azure.Authentication/MG.Utils.Azure.Authentication.nuspec

nuget pack ./../MG.Utils.Azure.Logging/MG.Utils.Azure.Logging.nuspec

nuget pack ./../MG.Utils.Azure.ServiceBus/MG.Utils.Azure.ServiceBus.nuspec

nuget pack ./../MG.Utils.Export/MG.Utils.Export.nuspec

nuget pack ./../MG.Utils.Smtp/MG.Utils.Smtp.nuspec

nuget pack ./../MG.Utils.Validation/MG.Utils.Validation.nuspec

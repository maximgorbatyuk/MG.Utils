$data = @(
   "./../src/MG.Utils/",
   "./../src/MG.Utils.Abstract/" ,
   "./../src/MG.Utils.AspNetCore/" ,
   "./../src/MG.Utils.AspNetCore.I18N/" ,
   "./../src/MG.Utils.AspNetCore.Middlewares/" ,
   "./../src/MG.Utils.AspNetCore.Redis/" ,
   "./../src/MG.Utils.AspNetCore.Swagger/" ,
   "./../src/MG.Utils.AspNetCore.DatabaseView/" ,
   "./../src/MG.Utils.Authentication/" ,
   "./../src/MG.Utils.Azure.Authentication/" ,
   "./../src/MG.Utils.Azure.Logging/" ,
   "./../src/MG.Utils.Azure.ServiceBus/" ,
   "./../src/MG.Utils.Export/" ,
   "./../src/MG.Utils.Smtp/" ,
   "./../src/MG.Utils.Validation/" 
);

$data | ForEach-Object {
    dotnet build $_
    nuget pack $_
}

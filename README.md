# TaskWebApi

1. Add the Connection String for your SSMS in appsetting.json
2. Add the Following Packages if not present 
   microsoft.entityframeworkcore.design\10.0.3\
   microsoft.entityframeworkcore.sqlserver\10.0.3\
   microsoft.entityframeworkcore.tools\10.0.3\
   swashbuckle.aspnetcore\10.1.3\

3. dotnet ef --version  ( Check version )
4. dotnet tool update --global dotnet-ef ( Update version )
5. dotnet ef migrations add InitialCreate ( then run this commond )
6. dotnet ef database update ( then run this commond for Database Changes )
7. then run the project and https://localhost:xxxx/swagger ( go this url and check the APIs )

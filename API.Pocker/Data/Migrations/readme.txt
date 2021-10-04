EF .NET Core
********************

---Directly from VS Package Manager Console---
Add migration:
Add-Migration [NAME_OF_MIGRATION]  -OutputDir "Data\Migrations" -Context "ApplicationDbContext" -Verbose
Add-Migration InitialCreate  -OutputDir "Data\Migrations" -Context "ApplicationDbContext" -Verbose

Remove migration:
Remove-Migration -Context "ApplicationDbContext" -Verbose



****Command line:
dotnet ef database drop --context=ApplicationDbContext --verbose

dotnet ef migrations add InitialCreate -o "Data\Migrations" --context=ApplicationDbContext --verbose

dotnet ef migrations remove --context=ApplicationDbContext --verbose

dotnet ef database update --context=ApplicationDbContext --verbose
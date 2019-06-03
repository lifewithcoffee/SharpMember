:: Usage:
:: add-migration Init
:: add-migration Init --output-dir EfGenerated
dotnet ef migrations add %* --context ApplicationDbContext --startup-project ..\SharpMember
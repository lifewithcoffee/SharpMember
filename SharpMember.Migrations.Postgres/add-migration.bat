:: Old Usage for "dotnet ef migrations add %* --context %context% --startup-project ..\SharpMember":
:: add-migration Init
:: add-migration Init --output-dir EfGenerated

:: Current Usage:
:: add-migration.bat <migration-name> <dbcontext-name>

dotnet ef migrations add %1 --context %2 --startup-project ..\SharpMember
:: Usage:
:: add-migration Init
:: add-migration Init --output-dir EfGenerated

set context=GlobalContext

dotnet ef migrations add %* --context %context% --startup-project ..\SharpMember
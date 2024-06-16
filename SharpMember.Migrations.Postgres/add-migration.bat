:: Usage:
:: add-migration Init
:: add-migration Init --output-dir EfGenerated

::set context=GlobalContext
::set context=MemberContext

::dotnet ef migrations add %* --context %context% --startup-project ..\SharpMember
dotnet ef migrations add %1 --context %2 --startup-project ..\SharpMember
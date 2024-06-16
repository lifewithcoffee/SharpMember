set context=GlobalContext

dotnet ef database update %* --context %context% --startup-project ..\SharpMember
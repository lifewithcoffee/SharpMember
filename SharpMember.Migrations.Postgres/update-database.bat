::set context=GlobalContext
set context=MemberContext

dotnet ef database update %* --context %context% --startup-project ..\SharpMember
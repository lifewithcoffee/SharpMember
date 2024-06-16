::set context=GlobalContext
set context=MemberContext

dotnet ef migrations remove --context %context% --startup-project ..\SharpMember
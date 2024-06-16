::set context=GlobalContext
set context=MemberContext

dotnet ef database update 0 --context %context% --startup-project ..\SharpMember
dotnet ef migrations remove --context %context% --startup-project ..\SharpMember

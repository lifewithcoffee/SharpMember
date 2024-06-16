::set context=GlobalContext
::set context=MemberContext

dotnet ef migrations remove --context %1 --startup-project ..\SharpMember
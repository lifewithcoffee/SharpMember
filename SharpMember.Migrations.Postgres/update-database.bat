::set context=GlobalContext
::set context=MemberContext

dotnet ef database update --context %1 --startup-project ..\SharpMember
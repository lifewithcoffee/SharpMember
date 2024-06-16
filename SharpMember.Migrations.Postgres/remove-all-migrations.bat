set context=ApplicationDbContext

dotnet ef database update Init --context %context% --startup-project ..\SharpMember
dotnet ef migrations remove --context %context% --startup-project ..\SharpMember
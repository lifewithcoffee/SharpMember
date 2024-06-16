# TODOs

- [ ] Rename MemberSystem to CommunitySystem, TaskSystem to ProjectSystem, then
      keep the aggregate root name,system name, DbContext name and service name
      the same, i.e. CommunitySystem's service is CommunityService, ProjectSystem's
      service is ProjectService.

- [ ] Rename TaskDbContext to ProjectContext, see if the migration folder will
      get rid of the postfix "Db"

---

- [ ] Apply global package & runtime configs
- [ ] Add shouldly to xunit (purpose: improve code and message readability)
- [ ] GitHub action pipeline to deploy to AWS
- [ ] Build REST interface and JS sdk (using openapi sdk generator)
- [ ] Build Angular client app
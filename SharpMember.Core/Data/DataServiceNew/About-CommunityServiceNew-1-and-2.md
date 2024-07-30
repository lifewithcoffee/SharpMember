# About CommunityServiceNew1/2

## 2 possible ways to implement DDD aggregate

CommunityServiceNew1 and CommunityServiceNew2 are 2 experimental approaches of
implementing DDD aggregate root:

- CommunityServiceNew1 is based on the `IRepository` of package `NetCoreUtils.Database`
- CommunityServiceNew2 is based on a dedicated DbContext

## DbContext per aggregate root (for CommunityServiceNew2)

Thoughts 2024-05-25:

- Every repository (named with the aggregate root object) is a DbContext with a subset
  of DbSet
  
- May need to design a dedicated BaseDbContext for ChangeTracker.AutoDetectChangesEnabled
  handling, logging etc.
  
- The repository DbContext will never be used for migration (migration will only be done
  via a 'overall' DbContext containing all DbSets
  
- The repository may have multiple DB schema, i.e. the repository is not designed by DB
  schema
  
- The repository will be rejected into a relevant service

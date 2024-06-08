using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.DataServiceNew
{
    public class CommunityRepository : DbContext
    {
        public DbSet<Community> Communities { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberProfileItemTemplate> memberProfileItemTemplates { get; set; }
    }

    public class CommunityServiceNew2 : DbContext, ICommunityServiceNew
    {
        CommunityRepository _repo { get; set; }

        public CommunityServiceNew2(CommunityRepository communityRepository)
        {
            _repo = communityRepository;
        }

        public Task<Member> AddMemberAsync(string appUserId, string name, string email, string role)
        {
            throw new NotImplementedException();
        }

        public Task AddMemberProfileTemplateAsync(string itemName, bool required)
        {
            throw new NotImplementedException();
        }

        public ICommunityServiceNew Bind(int id)
        {
            throw new NotImplementedException();
        }

        public Task CreateCommunityAsync(string appUserId, string communityName)
        {
            throw new NotImplementedException();
        }

        public Task<Member> CreateMemberAsync(string appUserId)
        {
            throw new NotImplementedException();
        }

        public int GetNextUnassignedMemberNumber()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Member> QueryMemberByNumber(int memberNumber)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Member> QueryMembers()
        {
            throw new NotImplementedException();
        }
    }
}

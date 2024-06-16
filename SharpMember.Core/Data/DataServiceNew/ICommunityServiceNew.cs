using SharpMember.Core.Data.Models.Community;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.DataServiceNew
{
    /// <summary>
    /// ICommunityServiceNew merges the old IMemberService and ICommunityService, as
    /// the "Community" object should be used as the aggregate root of the member system
    /// </summary>
    public interface ICommunityServiceNew : IBindable<ICommunityServiceNew>
    {
        // from old IMemberService
        int GetNextUnassignedMemberNumber();
        IQueryable<Member> QueryMemberByNumber(int memberNumber);   // renamed from GetByMemberNumber()
        IQueryable<Member> QueryMembers();    // renamed from GetByCommunity()
        Task<Member> CreateMemberAsync(string appUserId);  // renamed from GenerateNewMemberWithProfileItemsAsync()

        // from old ICommunityService
        Task<Member> AddMemberAsync(string appUserId, string name, string email, string role);
        Task AddMemberProfileTemplateAsync(string itemName, bool required);
        Task CreateCommunityAsync(string appUserId, string communityName);
    }
}

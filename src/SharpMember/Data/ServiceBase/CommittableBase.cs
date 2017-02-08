using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharpMember.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Data.ServiceBase
{
    public interface ICommittable<TDbContext> : IDisposable where TDbContext : DbContext
    {
        bool Commit();
        Task<bool> CommitAsync();
    }

    public class CommittableBase<TDbContext> : ICommittable<TDbContext> where TDbContext : DbContext
    {
        private TDbContext _context;
        private readonly ILogger _logger;

        public CommittableBase(TDbContext context, ILogger<ICommittable<TDbContext>> logger)
        {
            _context = context;
            _logger = logger;

            _logger.LogTrace("Initializing UnitOfWork with system default setting");
        }

        public async Task<bool> CommitAsync()
        {
            bool result = false;
            try
            {
                await _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                _logger.WriteException(ex);
            }

            return result;
        }

        public bool Commit()
        {
            bool result = false;
            try
            {
                _context.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                _logger.WriteException(ex);
            }

            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

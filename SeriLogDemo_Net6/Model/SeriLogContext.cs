using Microsoft.EntityFrameworkCore;

namespace SeriLogDemo_Net6.Model
{
    public class SeriLogContext: DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public SeriLogContext(DbContextOptions<SeriLogContext> options) : base(options)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Work> Work { get; set; }

        public DbSet<t_agent> t_agent { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SecurePosts.Context.Shared;
using SecurePosts.Entity.Shared.Database;

namespace SecurePosts.Context
{
    public partial class SecurePostsDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public SecurePostsDbContext() 
        {
        }

        public SecurePostsDbContext(DbContextOptions options, IConfiguration config) : base(options) 
        {
            this._config = config;
        }

        public virtual DbSet<UserDetail> UserDetail { get; set; }
        public virtual DbSet<UserSession> UserSession { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "SecurePosts");

            modelBuilder.ConfigureEntity();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

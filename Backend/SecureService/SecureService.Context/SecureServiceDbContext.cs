using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SecureService.Context.Shared;
using SecureService.Entity.Shared.Database;

namespace SecureService.Context
{
    public partial class SecureServiceDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public SecureServiceDbContext() 
        {
        }

        public SecureServiceDbContext(DbContextOptions options, IConfiguration config) : base(options) 
        {
            this._config = config;
        }

        public virtual DbSet<UserDetail> UserDetail { get; set; }
        public virtual DbSet<UserSession> UserSession { get; set; }
        public virtual DbSet<UserRefreshToken> UserRefreshToken { get; set; }
        public virtual DbSet<UserMatch> UserMatch { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "SecureService");

            modelBuilder.ConfigureEntity();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

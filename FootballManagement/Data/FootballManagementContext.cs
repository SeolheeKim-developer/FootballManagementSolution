using FootballManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace FootballManagement.Data
{
    public class FootballManagementContext : DbContext
    {
        //To give access to IHttpContextAccessor for Audit Data with IAuditable
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Property to hold the UserName value
        public string UserName
        {
            get; private set;
        }
        public FootballManagementContext(DbContextOptions<FootballManagementContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            if (_httpContextAccessor.HttpContext != null)
            {
                //We have a HttpContext, but there might not be anyone Authenticated
                UserName = _httpContextAccessor.HttpContext?.User.Identity.Name;
                UserName ??= "Unknown";
            }
            else
            {
                //No HttpContext so seeding data
                UserName = "Seed Data";
            }
        }
        public DbSet<Player> Players { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }

        public DbSet<PlayerTeam> PlayerTeams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add a unique index to the Code
            modelBuilder.Entity<League>()
                .HasKey(l => l.Code);

            //Prevent Cascade Delete from League to Team
            //so we are prevented from deleting a League with
            //Teams assigned
            modelBuilder.Entity<League>()
                .HasMany(t => t.Teams)
                .WithOne(l => l.League)
                .HasForeignKey(t => t.LeagueCode)
                .OnDelete(DeleteBehavior.Restrict);
                

            modelBuilder.Entity<Team>()
                .HasMany(t => t.PlayerTeams)
                .WithOne(t=>t.Team)
                .HasForeignKey(pt => pt.TeamID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
                .HasIndex(l => l.EMail)
                .IsUnique();

            modelBuilder.Entity<Player>()
                .HasMany(t => t.PlayerTeams)
                .WithOne(t => t.Player)
                .HasForeignKey(pt => pt.PlayerID)
                .OnDelete(DeleteBehavior.Cascade);

            //Many to Many Intersection
            modelBuilder.Entity<PlayerTeam>()
            .HasKey(t => new { t.PlayerID, t.TeamID });

            //modelBuilder.Entity<PlayerTeam>()
            //    .HasOne(t => t.Team)
            //    .WithMany(pt => pt.PlayerTeams)
            //    .HasForeignKey(t => t.TeamID)
            //    .OnDelete(DeleteBehavior.Restrict);

        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;

                        case EntityState.Added:
                            trackable.CreatedOn = now;
                            trackable.CreatedBy = UserName;
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;
                    }
                }
            }
        }
    }

}

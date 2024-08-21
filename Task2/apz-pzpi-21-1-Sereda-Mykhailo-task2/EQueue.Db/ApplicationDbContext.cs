using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EQueue.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace EQueue.Db
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<TraumaPlace> TraumaPlaces { get; set; }
        public virtual DbSet<TraumaType> TraumaTypes { get; set; }
        public virtual DbSet<Trauma> Traumas { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<CasePriority> CasePriorities { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
    }
}

using Microsoft.EntityFrameworkCore;

namespace Portal.Models.Admin
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Menu> Menus { get; set; }
      
        // public DbSet<Personel> Personeller { get; set; }
        public DbSet<Birim> Birimler { get; set; }
        public DbSet<Mudurluk> Mudurlukler { get; set; }
        public DbSet<Sube> Subeler { get; set; }
        public DbSet<KanGrubu> KanGrubus { get; set; }


         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

           

 // User - Sube Relationship
    modelBuilder.Entity<User>()
        .HasOne(u => u.Sube)
        .WithMany(s => s.Users)
        .HasForeignKey(u => u.SubeId)
        .OnDelete(DeleteBehavior.SetNull);

    // Birim - Mudurluk Relationship
    modelBuilder.Entity<Birim>()
        .HasMany(b => b.Mudurlukler)
        .WithOne(m => m.Birim)
        .HasForeignKey(m => m.BirimId)
        .OnDelete(DeleteBehavior.Cascade);

    // Mudurluk - Sube Relationship
    modelBuilder.Entity<Mudurluk>()
        .HasMany(m => m.Subeler)
        .WithOne(s => s.Mudurluk)
        .HasForeignKey(s => s.MudurlukId)
        .OnDelete(DeleteBehavior.Cascade);
       // User ve KanGrubu arasındaki Foreign Key ilişkisi
        modelBuilder.Entity<User>()
            .HasOne(u => u.KanGrubu)
            .WithMany(k => k.Users)
            .HasForeignKey(u => u.KanGrubuId)
            .OnDelete(DeleteBehavior.SetNull); // İsteğe bağlı: Eğer KanGrubu silinirse, User kaydındaki KanGrubuId null olur.

        // KanGrubu tablosundaki Ad alanı için zorunluluk tanımlama
        modelBuilder.Entity<KanGrubu>()
            .Property(k => k.Ad)
            .IsRequired()
            .HasMaxLength(50); // İsteğe bağlı: Ad alanının uzunluğunu sınırlayabilirsin.
        }
    }
}
// protected override void OnModelCreating(ModelBuilder modelBuilder)
// {
//     base.OnModelCreating(modelBuilder);

//     // RolePermission
//     modelBuilder.Entity<RolePermission>()
//         .HasKey(rp => new { rp.RoleId, rp.PermissionId });
//     modelBuilder.Entity<RolePermission>()
//         .HasOne(rp => rp.Role)
//         .WithMany(r => r.RolePermissions)
//         .HasForeignKey(rp => rp.RoleId)
//         .OnDelete(DeleteBehavior.Restrict);
//     modelBuilder.Entity<RolePermission>()
//         .HasOne(rp => rp.Permission)
//         .WithMany(p => p.RolePermissions)
//         .HasForeignKey(rp => rp.PermissionId)
//         .OnDelete(DeleteBehavior.Restrict);

//     // UserRole
//     modelBuilder.Entity<UserRole>()
//         .HasKey(ur => new { ur.UserId, ur.RoleId });
//     modelBuilder.Entity<UserRole>()
//         .HasOne(ur => ur.User)
//         .WithMany(u => u.UserRoles)
//         .HasForeignKey(ur => ur.UserId)
//         .OnDelete(DeleteBehavior.Restrict);
//     modelBuilder.Entity<UserRole>()
//         .HasOne(ur => ur.Role)
//         .WithMany(r => r.UserRoles)
//         .HasForeignKey(ur => ur.RoleId)
//         .OnDelete(DeleteBehavior.Restrict);


// modelBuilder.Entity<Event>()
//         .HasOne(e => e.Creator)
//         .WithMany(u => u.EventsCreated)
//         .HasForeignKey(e => e.CreatedBy)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete for User

//     // EventParticipant ile Event arasındaki ilişki
//     modelBuilder.Entity<EventParticipant>()
//         .HasOne(ep => ep.Event)
//         .WithMany(e => e.EventParticipants)
//         .HasForeignKey(ep => ep.EventId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete for Event

//     // EventParticipant ile User arasındaki ilişki
//     modelBuilder.Entity<EventParticipant>()
//         .HasOne(ep => ep.User)
//         .WithMany(u => u.EventParticipants)
//         .HasForeignKey(ep => ep.UserId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete for User

   
//     // FormSubmission
//     modelBuilder.Entity<FormSubmission>()
//         .HasKey(fs => fs.SubmissionId);
//     modelBuilder.Entity<FormSubmission>()
//         .HasOne(fs => fs.Form)
//         .WithMany(f => f.FormSubmissions)
//         .HasForeignKey(fs => fs.FormId)
//         .OnDelete(DeleteBehavior.Restrict);
//     modelBuilder.Entity<FormSubmission>()
//         .HasOne(fs => fs.User)
//         .WithMany(u => u.FormSubmissions)
//         .HasForeignKey(fs => fs.UserId)
//         .OnDelete(DeleteBehavior.Restrict);
//     modelBuilder.Entity<Form>()
//         .HasMany(f => f.FormSubmissions)
//         .WithOne(fs => fs.Form)
//         .HasForeignKey(fs => fs.FormId)
//         .OnDelete(DeleteBehavior.Cascade);

//     // TripParticipant
//     modelBuilder.Entity<TripParticipant>()
//         .HasKey(tp => tp.ParticipantId);
//     modelBuilder.Entity<TripParticipant>()
//         .HasOne(tp => tp.Trip)
//         .WithMany(t => t.TripParticipants)
//         .HasForeignKey(tp => tp.TripId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete
//     modelBuilder.Entity<TripParticipant>()
//         .HasOne(tp => tp.User)
//         .WithMany(u => u.TripParticipants)
//         .HasForeignKey(tp => tp.UserId)
//         .OnDelete(DeleteBehavior.Restrict);
//     modelBuilder.Entity<Trip>()
//         .HasMany(f => f.TripParticipants)
//         .WithOne(fs => fs.Trip)
//         .HasForeignKey(fs => fs.TripId)
//         .OnDelete(DeleteBehavior.Cascade);

//     // Blog
//     modelBuilder.Entity<Blog>()
//         .HasOne(b => b.Category)
//         .WithMany(c => c.Blogs)
//         .HasForeignKey(b => b.CategoryId)
//         .OnDelete(DeleteBehavior.Restrict);

//     // PointsRedemption
//     modelBuilder.Entity<PointsRedemption>()
//         .HasKey(pr => pr.RedemptionId);
//     modelBuilder.Entity<PointsRedemption>()
//         .HasOne(pr => pr.User)
//         .WithMany(u => u.PointsRedemptions)
//         .HasForeignKey(pr => pr.UserId)
//         .OnDelete(DeleteBehavior.Restrict);

//     // PointsLog
//     modelBuilder.Entity<PointsLog>()
//         .HasKey(pl => pl.LogId);
//     modelBuilder.Entity<PointsLog>()
//         .HasOne(pl => pl.User)
//         .WithMany(u => u.PointsLogs)
//         .HasForeignKey(pl => pl.UserId)
//         .OnDelete(DeleteBehavior.Restrict);

//     // QrCodeScan
//     modelBuilder.Entity<QrCodeScan>()
//         .HasKey(qs => qs.ScanId);
//     modelBuilder.Entity<QrCodeScan>()
//         .HasOne(qs => qs.Event)
//         .WithMany(e => e.QrCodeScans)
//         .HasForeignKey(qs => qs.EventId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete
//     modelBuilder.Entity<QrCodeScan>()
//         .HasOne(qs => qs.User)
//         .WithMany(u => u.QrCodeScans)
//         .HasForeignKey(qs => qs.UserId)
//         .OnDelete(DeleteBehavior.Restrict);

//     // FacilityQrCode
//     modelBuilder.Entity<FacilityQrCode>()
//         .HasKey(fqc => fqc.FacilityQrCodeId);
//     modelBuilder.Entity<FacilityQrCode>()
//         .HasOne(fqc => fqc.Facility)
//         .WithMany(f => f.FacilityQrCodes)
//         .HasForeignKey(fqc => fqc.FacilityId)
//         .OnDelete(DeleteBehavior.Restrict);
// }

//     }}


// protected override void OnModelCreating(ModelBuilder modelBuilder)
// {
//     base.OnModelCreating(modelBuilder);

//     // RolePermission
//     modelBuilder.Entity<RolePermission>()
//         .HasKey(rp => new { rp.RoleId, rp.PermissionId });
//     modelBuilder.Entity<RolePermission>()
//         .HasOne(rp => rp.Role)
//         .WithMany(r => r.RolePermissions)
//         .HasForeignKey(rp => rp.RoleId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete
//     modelBuilder.Entity<RolePermission>()
//         .HasOne(rp => rp.Permission)
//         .WithMany(p => p.RolePermissions)
//         .HasForeignKey(rp => rp.PermissionId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete

//     // UserRole
//     modelBuilder.Entity<UserRole>()
//         .HasKey(ur => new { ur.UserId, ur.RoleId });
//     modelBuilder.Entity<UserRole>()
//         .HasOne(ur => ur.User)
//         .WithMany(u => u.UserRoles)
//         .HasForeignKey(ur => ur.UserId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete
//     modelBuilder.Entity<UserRole>()
//         .HasOne(ur => ur.Role)
//         .WithMany(r => r.UserRoles)
//         .HasForeignKey(ur => ur.RoleId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete

//     // Event
//     modelBuilder.Entity<Event>()
//         .HasOne(e => e.Creator)
//         .WithMany(u => u.EventsCreated)
//         .HasForeignKey(e => e.CreatedBy)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete

//     // EventParticipant
//     modelBuilder.Entity<EventParticipant>()
//         .HasOne(ep => ep.Event)
//         .WithMany(e => e.EventParticipants)
//         .HasForeignKey(ep => ep.EventId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete
//     modelBuilder.Entity<EventParticipant>()
//         .HasOne(ep => ep.User)
//         .WithMany(u => u.EventParticipants)
//         .HasForeignKey(ep => ep.UserId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete

//     // Form
//     modelBuilder.Entity<Form>()
//         .HasOne(f => f.Creator)
//         .WithMany(u => u.FormsCreated)
//         .HasForeignKey(f => f.CreatedBy)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete

//     modelBuilder.Entity<FormSubmission>()
//         .HasOne(fs => fs.Form)
//         .WithMany(f => f.FormSubmissions)
//         .HasForeignKey(fs => fs.FormId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete
//     modelBuilder.Entity<FormSubmission>()
//         .HasOne(fs => fs.User)
//         .WithMany(u => u.FormSubmissions)
//         .HasForeignKey(fs => fs.UserId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete

//     // Trip
//     modelBuilder.Entity<Trip>()
//         .HasOne(t => t.Creator)
//         .WithMany(u => u.TripsCreated)
//         .HasForeignKey(t => t.CreatedBy)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete

//     modelBuilder.Entity<TripParticipant>()
//         .HasOne(tp => tp.Trip)
//         .WithMany(t => t.TripParticipants)
//         .HasForeignKey(tp => tp.TripId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete
//     modelBuilder.Entity<TripParticipant>()
//         .HasOne(tp => tp.User)
//         .WithMany(u => u.TripParticipants)
//         .HasForeignKey(tp => tp.UserId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete

//     // Blog
//     modelBuilder.Entity<Blog>()
//         .HasOne(b => b.Creator)
//         .WithMany(u => u.BlogsCreated)
//         .HasForeignKey(b => b.CreatedBy)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete
//     modelBuilder.Entity<Blog>()
//         .HasOne(b => b.Category)
//         .WithMany(c => c.Blogs)
//         .HasForeignKey(b => b.CategoryId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete

//     // PointsRedemption
//     modelBuilder.Entity<PointsRedemption>()
//         .HasOne(pr => pr.User)
//         .WithMany(u => u.PointsRedemptions)
//         .HasForeignKey(pr => pr.UserId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete

//     // PointsLog
//     modelBuilder.Entity<PointsLog>()
//         .HasOne(pl => pl.User)
//         .WithMany(u => u.PointsLogs)
//         .HasForeignKey(pl => pl.UserId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete

//     // QrCodeScan
//     modelBuilder.Entity<QrCodeScan>()
//         .HasOne(qs => qs.Event)
//         .WithMany(e => e.QrCodeScans)
//         .HasForeignKey(qs => qs.EventId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete
//     modelBuilder.Entity<QrCodeScan>()
//         .HasOne(qs => qs.User)
//         .WithMany(u => u.QrCodeScans)
//         .HasForeignKey(qs => qs.UserId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete

//     // FacilityQrCode
//     modelBuilder.Entity<FacilityQrCode>()
//         .HasOne(fqc => fqc.Facility)
//         .WithMany(f => f.FacilityQrCodes)
//         .HasForeignKey(fqc => fqc.FacilityId)
//         .OnDelete(DeleteBehavior.Cascade); // Cascade delete
// }

   
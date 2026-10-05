using Microsoft.EntityFrameworkCore;
using SaaSProje.Models;
using SaaSProje.Services;

namespace SaaSProje.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ICurrentTenantService _currentTenantService;

        // Dependency Injection ile Tenant servisimizi veritabanına enjekte ediyoruz
        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentTenantService currentTenantService)
            : base(options)
        {
            _currentTenantService = currentTenantService;
        }

        // Tablolarımız
        public DbSet<Firm> Firms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // GLOBAL QUERY FILTER: Yazılımcı Where yazmasa bile otomatik olarak veriyi FirmId'ye göre filtrele
            modelBuilder.Entity<User>().HasQueryFilter(u => u.FirmId == _currentTenantService.FirmId);

            // DİKKAT: Ürünler tablosu için de aynı filtreleme kuralını ekledik!
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.FirmId == _currentTenantService.FirmId);
        }

        // Veritabanına yeni kayıt eklenirken FirmId'yi otomatik doldurma
        public override int SaveChanges()
        {
            SetTenantId();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetTenantId();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetTenantId()
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e => e.State == EntityState.Added))
            {
                if (_currentTenantService.FirmId.HasValue)
                {
                    entry.Entity.FirmId = _currentTenantService.FirmId.Value;
                }
            }
        }
    }
}
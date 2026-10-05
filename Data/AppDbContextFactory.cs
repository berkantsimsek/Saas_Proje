using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SaaSProje.Services;

namespace SaaSProje.Data
{
    // EF Core'a "Migration yaparken AppDbContext'i bu ayarlarla oluştur" diyoruz
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // appsettings.json dosyamızdaki bağlantı adresini buraya veriyoruz
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SaasDb;Trusted_Connection=True;MultipleActiveResultSets=true");

            // Migration işlemi sırasında gerçek bir kullanıcı girişi olmadığı için 
            // Context'e hata vermemesi adına boş/sahte bir Tenant (Firma) servisi gönderiyoruz.
            return new AppDbContext(optionsBuilder.Options, new DummyTenantService());
        }
    }

    // Sadece Migration sırasında kullanılacak geçici (Dummy) servis
    public class DummyTenantService : ICurrentTenantService
    {
        public int? FirmId => null;
        public void SetFirmId(int firmId) { }
    }
}
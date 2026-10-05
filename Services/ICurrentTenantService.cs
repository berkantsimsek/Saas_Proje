
namespace SaaSProje.Services
{
    public interface ICurrentTenantService
    {
        int? FirmId { get; }
        void SetFirmId(int firmId);
    }
}
using Microsoft.AspNetCore.Http;

namespace SaaSProje.Services
{
    public class CurrentTenantService : ICurrentTenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int? _firmId;

        public CurrentTenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? FirmId
        {
            get
            {
                // Daha önce set edildiyse direkt döndür
                if (_firmId.HasValue) return _firmId;

                // Edilmediyse, HTTP isteği ile gelen Token içindeki "FirmId" claim'ini oku
                var firmIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("FirmId")?.Value;
                if (int.TryParse(firmIdClaim, out int id))
                {
                    _firmId = id;
                }
                return _firmId;
            }
        }

        public void SetFirmId(int firmId) => _firmId = firmId;
    }
}
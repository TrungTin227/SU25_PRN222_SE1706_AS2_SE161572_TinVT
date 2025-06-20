namespace SMMS.Services.TinVT
{
    public class ServiceProviders : IServiceProviders
    {
        private readonly UserAccountService _userAccountService;
        private readonly IHealthCheckSessionTinVTService _healthCheckSessionTinVTService;
        private readonly HealthCheckStudentTinVtService _healthCheckStudentTinVtService;

        public ServiceProviders(
            UserAccountService userAccountService,
            IHealthCheckSessionTinVTService healthCheckSessionTinVTService,
            HealthCheckStudentTinVtService healthCheckStudentTinVtService)
        {
            _userAccountService = userAccountService;
            _healthCheckSessionTinVTService = healthCheckSessionTinVTService;
            _healthCheckStudentTinVtService = healthCheckStudentTinVtService;
        }

        public UserAccountService UserAccountService => _userAccountService;
        public IHealthCheckSessionTinVTService HealthCheckSessionTinVTService => _healthCheckSessionTinVTService;
        public HealthCheckStudentTinVtService HealthCheckStudentTinVtService => _healthCheckStudentTinVtService;
    }

    public interface IServiceProviders
    {
        UserAccountService UserAccountService { get; }
        IHealthCheckSessionTinVTService HealthCheckSessionTinVTService { get; }
        HealthCheckStudentTinVtService HealthCheckStudentTinVtService { get; }
    }
}
namespace SMMS.Services.TinVT
{
    public class ServiceProviders : IServiceProviders
    {
        private readonly UserAccountService _userAccountService;
        private readonly IHealthCheckSessionTinVTService _healthCheckSessionTinVTService;
        private readonly HealthCheckStudentTinVtService _healthCheckStudentTinVtService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISessionService _sessionService;

        public ServiceProviders(
            UserAccountService userAccountService,
            IHealthCheckSessionTinVTService healthCheckSessionTinVTService,
            HealthCheckStudentTinVtService healthCheckStudentTinVtService,
            IAuthenticationService authenticationService,
            ISessionService sessionService)
        {
            _userAccountService = userAccountService;
            _healthCheckSessionTinVTService = healthCheckSessionTinVTService;
            _healthCheckStudentTinVtService = healthCheckStudentTinVtService;
            _authenticationService = authenticationService;
            _sessionService = sessionService;
        }

        public UserAccountService UserAccountService => _userAccountService;
        public IHealthCheckSessionTinVTService HealthCheckSessionTinVTService => _healthCheckSessionTinVTService;
        public HealthCheckStudentTinVtService HealthCheckStudentTinVtService => _healthCheckStudentTinVtService;
        public IAuthenticationService AuthenticationService => _authenticationService;
        public ISessionService SessionService => _sessionService;
    }

    public interface IServiceProviders
    {
        UserAccountService UserAccountService { get; }
        IHealthCheckSessionTinVTService HealthCheckSessionTinVTService { get; }
        HealthCheckStudentTinVtService HealthCheckStudentTinVtService { get; }
        IAuthenticationService AuthenticationService { get; }
        ISessionService SessionService { get; }
    }
}
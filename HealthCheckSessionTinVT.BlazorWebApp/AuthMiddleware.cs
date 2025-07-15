//using SMMS.Services.TinVT;

//namespace HealthCheckSessionTinVT.BlazorWebApp.ASM2
//{
//    public class AuthMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public AuthMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext context, ISessionService sessionService)
//        {
//            if (!sessionService.IsLoggedIn() && !context.Request.Path.Value.Contains("/account/login"))
//            {
//                context.Response.Redirect("/account/login");
//                return;
//            }

//            await _next(context);
//        }
//    }
//}

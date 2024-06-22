namespace FinanceManager.Routes
{
    public static class AuthRouter
    {
        public static void MapAtuhRoutes(WebApplication app)
        {
            app.MapPost("/Login", async (IAuthService authService, LoginModel loginModel) =>
            {
                var res = await authService.LoginAsync(loginModel);
                return res;
            });
            app.MapPost("/register", async (IAuthService authService , RegisterModel registerModel) =>
            {
                await authService.RegisterAsync(registerModel);
                return StatusCodes.Status201Created;
            });
            app.MapGet("/forgot-password", () => "Forgot Password Page");
        }
    }
}

namespace FinanceManager.Routes
{
    public static class AuthRouter
    {
        public static void MapAtuhRoutes(WebApplication app)
        {
            app.MapGet("/login", () => "Login Page");
            app.MapGet("/register", () => "Register Page");
            app.MapGet("/forgot-password", () => "Forgot Password Page");
        }
    }
}

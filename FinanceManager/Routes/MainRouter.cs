namespace FinanceManager.Routes
{
    public static class MainRouter
    {
        public static void MapRoutes(this WebApplication app)
        {
            AuthRouter.MapAtuhRoutes(app);
        }
    }
}

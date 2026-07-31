namespace MultiShop.Order.WebApi.Authorization
{
    public static class OrderAuthorizationConstants
    {
        public const string Audience = "order_api";
        public const string Scope = "order_api";

        public const string AccessPolicy = "OrderApi";
        public const string ManagementPolicy = "OrderManagement";

        public const string AdminRole = "Admin";
        public const string ManagerRole = "Manager";
    }
}

namespace OMSBlazor.Client.Constants
{
    public static class BackEndEnpointURLs
    {
        public const string Base = "api/app";

        public static class CustomersEndpoints
        {
            public const string CustomerBase = "customer";

            public const string GetCustomers = $"{Base}/{CustomerBase}/customers";

            public const string PurchasesByCustomers = $"{Base}/{CustomerBase}/purchases-by-customer";

            public const string CustomersByCountries = $"{Base}/{CustomerBase}/customers-by-country";
        }

        public static class EmployeeEndpoints
        {
            public const string EmployeeBase = "employee";

            public const string GetEmployees = $"{Base}/{EmployeeBase}/employees";

            public const string SalesByEmployees = $"{Base}/{EmployeeBase}/sales-by-employees";
        }

        public static class OrderEndpoints
        {
            public const string OrderBase = "order";

            public const string GetOrders = $"{Base}/{OrderBase}/orders";

            public const string SaveOrder = $"{Base}/{OrderBase}/save-order";

            public const string OrdersByCountries = $"{Base}/{OrderBase}/orders-by-countries";

            public const string SalesByCategories = $"{Base}/{OrderBase}/sales-by-categories";

            public const string SalesByCountries = $"{Base}/{OrderBase}/sales-by-countries";

            public const string Summaries = $"{Base}/{OrderBase}/summaries";

            public static string GetUrlForInvoice(int orderId)
            {
                return $"{BackEndEnpointURLs.Base}/{BackEndEnpointURLs.OrderEndpoints.OrderBase}/{orderId}/invoice";
            }
        }

        public static class ProductEndpoints
        {
            public const string ProductBase = "product";

            public const string GetProducts = $"{Base}/{ProductBase}/products";

            public const string ProductByCategories = $"{Base}/{ProductBase}/products-by-category";
        }

        public static class StasticsRecalculator
        {
            private const string StasticsBase = "stastics-recalculator";

            public const string RecalculateStatistics = $"{Base}/{StasticsBase}/recalculate-Stastics";
        }
    }
}

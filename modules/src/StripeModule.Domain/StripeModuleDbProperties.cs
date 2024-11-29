namespace StripeModule;

public static class StripeModuleDbProperties
{
    public static string DbTablePrefix { get; set; } = "StripeModule";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "StripeModule";
}

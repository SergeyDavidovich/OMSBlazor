using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripeModule.Pages
{
    public static class BackEndEndpointURLs
    {
        public const string Base = "api/app";

        public const string CreatePayment = $"{Base}/payment";

        public const string GetCheckoutUrl = $"{Base}/payment/checkout-url";
    }
}

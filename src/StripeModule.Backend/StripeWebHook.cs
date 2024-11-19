using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;
using Stripe.Checkout;

namespace StripeModule.Backend
{
    [Route("api/stripehooks")]
    public class StripeWebHook : Controller
    {
        private readonly IConfiguration _configuration;

        public StripeWebHook(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            Console.WriteLine("Hello from webhook");
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            //var secret = _configuration.GetValue<string>("StripeWebHookSecret");
            var secret = "whsec_0c365d9db221ef7e28da5af75d1d7f1103d146e0edea0ed3be9d6888946a71b5";

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    secret
                );

                // Handle the checkout.session.completed event
                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;
                    var options = new SessionGetOptions();
                    options.AddExpand("line_items");

                    var service = new SessionService();
                    // Retrieve the session. If you require line items in the response, you may include them by expanding line_items.
                    var sessionWithLineItems = await service.GetAsync(session.Id, options);
                    StripeList<LineItem> lineItems = sessionWithLineItems.LineItems;

                    // do something here based on the order's line items!
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}

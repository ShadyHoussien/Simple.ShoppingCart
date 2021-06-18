using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Simple.ShoppingCart.Models;
using Simple.ShoppingCart.Models.PaymentModels.PaymentCallback;
using Simple.ShoppingCart.Models.PaymentModels.StepOne;
using Simple.ShoppingCart.Models.PaymentModels.StepThree;
using Simple.ShoppingCart.Models.PaymentModels.StepTwo;

namespace Simple.ShoppingCart.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public CheckoutController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.Cart = HomeController.ShoppingCart;
            return View();
        }

        public IActionResult Purchase()
        {
            ViewBag.Cart = HomeController.ShoppingCart;

            decimal amountInCents = HomeController.ShoppingCart.Price * 100;
            //Step 1
            string token = GetToken();
            //Step 2
            int orderId = GetOrderId(token , amountInCents);
            //Step 3
            string paymentKey = GetPaymentKey(token, amountInCents, orderId);

            int iFrameId = _configuration.GetValue<int>("IFrameId");

            string iframeUrl = $"https://accept.paymobsolutions.com/api/acceptance/iframes/{iFrameId}?payment_token={paymentKey}";

            return View("Purchase", iframeUrl);
        }

        [HttpGet]
        public IActionResult PaymentCallback(PaymentCallBackRequest obj)
        {
            ViewBag.Cart = new ShoppingCartDto();
            return View();
        }

        private string GetToken()
        {
            string apiKey = _configuration.GetValue<string>("ApiKey");
            string acceptAuthUrl = "https://accept.paymob.com/api/auth/tokens";

            using (var response = Post(acceptAuthUrl, new { api_key = apiKey }))
            {
                if (response.IsSuccessStatusCode)
                {
                    using (var responseContent = response.Content)
                    {
                        var data = responseContent.ReadAsStringAsync();

                        var res = JsonConvert.DeserializeObject<AuthTokenResponse>(data.Result);

                        return res.token;
                    }
                }
            }
            return "";
        }

        private int GetOrderId(string token,decimal amountInCents)
        {
            string getOrderUrl = "https://accept.paymob.com/api/ecommerce/orders";

            var request = new OrderRegistrationRequest
            {
                amount_cents = amountInCents,
                auth_token = token,
                delivery_needed = false,
                items = new List<OrderItem>()
            };

            using (var response = Post(getOrderUrl, request))
            {
                if (response.IsSuccessStatusCode)
                {
                    using (var responseContent = response.Content)
                    {
                        var data = responseContent.ReadAsStringAsync();

                        var res = JsonConvert.DeserializeObject<OrderRegestirationResponse>(data.Result);

                        return res.id;
                    }
                }
            }
            return 0;
        }

        private string GetPaymentKey(string token, decimal amountInCents , int orderId)
        {
            string getPaymentKeyUrl = "https://accept.paymob.com/api/acceptance/payment_keys";
            int integrationid = _configuration.GetValue<int>("IntegrationId");
            var request = new PaymentKeyRequest
            {
                auth_token = token,
                amount_cents = amountInCents,
                expiration = 3600,
                order_id = orderId,
                billing_data = new BillingData 
                {
                    first_name = "Ahmed",
                    last_name = "Saleh",
                    email = "Ahmed.Saleh@gmail.com",
                    phone_number = "+20145778945",
                    apartment = "NA",
                    building = "NA",
                    city = "NA",
                    country = "NA",
                    floor = "NA",
                    postal_code = "NA",
                    shipping_method = "NA",
                    state = "NA",
                    street = "NA"
                },
                currency = "EGP",
                integration_id = integrationid
            };

            using (var response = Post(getPaymentKeyUrl, request))
            {
                if (response.IsSuccessStatusCode)
                {
                    using (var responseContent = response.Content)
                    {
                        var data = responseContent.ReadAsStringAsync();

                        var res = JsonConvert.DeserializeObject<PaymentKeyResponse>(data.Result);

                        return res.token;
                    }
                }
            }
            return "";
        }

        public HttpResponseMessage Post<T>(string url, T genericType, string token = null)
        {
            var client = _clientFactory.CreateClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var jText = JsonConvert.SerializeObject(genericType);

            HttpContent httpContent = new StringContent(jText);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return client.PostAsync(url, httpContent).Result;
        }
    }
}
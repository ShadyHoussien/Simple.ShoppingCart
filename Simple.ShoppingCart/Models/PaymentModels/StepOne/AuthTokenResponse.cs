using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.ShoppingCart.Models.PaymentModels.StepOne
{
    public class AuthTokenResponse
    {
        public object Profile { get; set; }
        public string token { get; set; }
    }
}

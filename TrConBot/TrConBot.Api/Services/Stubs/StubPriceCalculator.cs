using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrConBot.Api.Services.Stubs
{
    public class StubPriceCalculator : IPriceCalculator
    {
        public double GetPrice(string routeFrom, string routeTo, DateTime loadingDate, string cargoTypeCode, int containerCount)
        {
            return 123700.00;
        }
    }
}
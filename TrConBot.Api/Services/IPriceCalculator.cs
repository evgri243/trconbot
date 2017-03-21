using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrConBot.Api.Services
{
    public interface IPriceCalculator
    {
        double GetPrice(string routeFrom, string routeTo, DateTime loadingDate, string cargoTypeCode, int containerCount);
    }
}
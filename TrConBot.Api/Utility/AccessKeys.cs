using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace TrConBot.Api.Utility
{
    public static class AccessKeys
    {
        public const string LuisModelId = "0007f65b-61ff-4587-a8c4-418f25eaff9b";
        public const string LuisSubscriptionKey = "f54af3c6ba1f46dd91b44ebc5a12da5b";

        public static readonly string BingTranslatorSubscriptionKey = WebConfigurationManager.AppSettings["BingTranslatorSubscriptionKey"];
    }
}
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TrConBot.Api.Utility
{
    public static class ActivityExtensions
    {
        public static Activity CreateTyping(this Activity activity)
        {
            var typing = activity.CreateReply();
            typing.Type = ActivityTypes.Typing;

            return typing;
        }
    }
}
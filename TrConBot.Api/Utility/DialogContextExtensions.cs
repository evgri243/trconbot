using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TrConBot.Api.Utility
{
    public static class DialogContextExtensions
    {
        public async static Task Delay(this IDialogContext context, int millisecondsDelay)
        {
            if (context == null) throw new NullReferenceException();
            if (millisecondsDelay < 0) throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            var typing = context.MakeMessage();
            typing.Type = ActivityTypes.Typing;
            await context.PostAsync(typing);

            await Task.Delay(millisecondsDelay);
        }
    }
}
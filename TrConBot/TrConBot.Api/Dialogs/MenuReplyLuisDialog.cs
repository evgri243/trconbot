using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using TrConBot.Api.Services;
using TrConBot.Api.Services.Stubs;
using TrConBot.Api.Utility;
using Microsoft.Bot.Builder.FormFlow;

namespace TrConBot.Api.Dialogs
{
    [Serializable]
    [LuisModel(AccessKeys.LuisModelId, AccessKeys.LuisSubscriptionKey)]
    public class MenuReplyLuisDialog: LuisDialog<bool>
    {
        [LuisIntent("")]
        public async Task ProcessNone(IDialogContext context, LuisResult result)
        {
            var searcher = new StubQnaSeracher();
            var answer = searcher.Search(context?.Activity?.AsMessageActivity()?.Text);

            if (answer == null)
            {
                var text = "К сожалению, мне не удалось вас понять. Повторите запрос или позвоните нам по телефону 8(800)100-22-20";
                await context.PostAsync(text, "ru-RU");

                context.Done(false);
            }
            else
            {
                await context.PostAsync(answer.Answer);

                await context.Delay(1000);
                var otherLinksMessage = context.MakeMessage();
                var otherLinksText = "Другие ссылки, которые могут быть полезны:\n\n";
                otherLinksText += String.Join("\n", answer.Links.Select(l => "* [" + l.Title + "](" + l.Uri.ToString() + ")")) + "\n\n";
                await context.PostAsync(otherLinksText);

                await context.Delay(1000);
                var otherLinksCard = new ThumbnailCard(text: "Не нашли того, что хотели? Позвоните нам!", buttons: new List<CardAction>
                                                                            {
                                                                                new CardAction {Type = ActionTypes.OpenUrl, Title = "8(800)100-22-20", Value = "tel:+78001002220"}
                                                                            });

                otherLinksMessage.Attachments.Add(otherLinksCard.ToAttachment());
                await context.PostAsync(otherLinksMessage);

                context.Done(true);
            }

        }

        [LuisIntent("Hello")]
        public async Task ProcessHello(IDialogContext context, LuisResult result)
        {
            await context.Delay(1000);
            await context.PostAsync("Добрый день еще раз! Чем могу помочь?", MessageLocale.RuRu);

            context.Done(false);
        }

        [LuisIntent("ThankYou")]
        public async Task ProcessThankYou(IDialogContext context, LuisResult result)
        {
            await context.Delay(1000);
            await context.PostAsync("Не за что, обращайтесь", MessageLocale.RuRu);

            context.Done(false);
        }

        [LuisIntent("Goodbye")]
        public async Task ProcessGoodbye(IDialogContext context, LuisResult result)
        {
            await context.Delay(1000);
            await context.PostAsync("Всего доброго, обращайтесь еще", MessageLocale.RuRu);

            context.Done(false);
        }

        [LuisIntent("CalculateCosts")]
        public async Task ProcessCalculateCost(IDialogContext context, LuisResult result)
        {
            var form = new FormDialog<PriceCalculationForm>(new PriceCalculationForm(), PriceCalculationForm.BuildForm, FormOptions.PromptInStart);
            context.Call(form, FormEnded);
        }

        private async Task FormEnded(IDialogContext context, IAwaitable<PriceCalculationForm> result)
        {
            context.Done(true);

            await Task.FromResult(true);
        }

        protected override async Task<string> GetLuisQueryTextAsync(IDialogContext context, IMessageActivity message)
        {
            var baseLuisText = await base.GetLuisQueryTextAsync(context, message);

            if (message.Locale != null && message.Locale != "en-US")
            {
                try
                {
                    var bingTranslatorClient = new BingTranslatorClient(AccessKeys.BingTranslatorSubscriptionKey);
                    return await bingTranslatorClient.Translate(baseLuisText, "en-US", message.Locale);

                }
                catch (Exception e)
                {
                    throw;
                }
            }

            return baseLuisText;
        }
    }
}
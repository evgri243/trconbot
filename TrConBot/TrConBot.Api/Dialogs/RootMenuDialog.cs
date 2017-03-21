using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using TrConBot.Api.Utility;
using Microsoft.Bot.Builder.FormFlow;

namespace TrConBot.Api.Dialogs
{
    [Serializable]
    public class RootMenuDialog : IDialog
    {
        private const string _priceDialogCommand = "Рассчитать стоимость";
        private const string _failDialogCommand = "Сломать все";
        private const string _cancelDialogCommand = "Завершить";

        public RootMenuDialog(bool sendWelcomeMessage = false)
        {
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(InitialMessageReceived);
        }

        private async Task InitialMessageReceived(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Добрый день!", MessageLocale.RuRu);

            await context.Delay(1000);
            var message = PopulateMenuMessage(context.MakeMessage(), title: "Чем я могу вам помочь?");
            await context.PostAsync(message, context.CancellationToken);

            context.Wait(MenuReplyReceived);
        }

        private async Task MenuReplyReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var response = await result;
            if (response.Text == _priceDialogCommand)
            {
                var form = new FormDialog<PriceCalculationForm>(new PriceCalculationForm(), PriceCalculationForm.BuildForm, FormOptions.PromptInStart);
                context.Call<PriceCalculationForm>(form, FormEnded);

            }
            else if (response.Text == _failDialogCommand)
            {
                await context.Delay(2000);
                throw new Exception();
            }
            else if (response.Text == _cancelDialogCommand)
            {
                await context.Delay(1500);
                await context.PostAsync("До свидания, обращайтесь еще", MessageLocale.RuRu);

                context.Done<object>(null);
            }
            else
            {
                await context.Forward(new MenuReplyLuisDialog(), DialogEnded, response, context.CancellationToken);
            }

        }

        private async Task FormEnded(IDialogContext context, IAwaitable<PriceCalculationForm> result)
        {
            await context.Delay(3000);

            var message = PopulateMenuMessage(context.MakeMessage(), title: "Чем еще я могу вам помочь?");
            await context.PostAsync(message, context.CancellationToken);

            context.Wait(MenuReplyReceived);
        }

        private async Task DialogEnded(IDialogContext context, IAwaitable<bool> result)
        {
            var dialogDone = await result;

            if (dialogDone)
            {
                await context.Delay(2000);

                var message = PopulateMenuMessage(context.MakeMessage(), title: "Чем еще я могу вам помочь?");
                await context.PostAsync(message, context.CancellationToken);
            }

            context.Wait(MenuReplyReceived);

            // Оценка опыта использования сервиса
        }

        private IMessageActivity PopulateMenuMessage(IMessageActivity message, string title = null, string subtitle = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var attachment = new ThumbnailCard(title: title, subtitle: subtitle, text: "Выберите одну из опций или напишите свой вопрос",
                                        buttons: new List<CardAction> {
                                            new CardAction(type: ActionTypes.PostBack, title: "Расчитать стоимость", value: _priceDialogCommand),
                                            new CardAction(type: ActionTypes.PostBack, title: "Сломать все", value: _failDialogCommand),
                                            new CardAction(type: ActionTypes.PostBack, title: "Завершить", value: _cancelDialogCommand)
                                        }
                );
            message.Attachments.Add(attachment.ToAttachment());

            return message;
        }
    }
}
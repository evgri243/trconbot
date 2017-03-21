using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using TrConBot.Api.Services.Stubs;

namespace TrConBot.Api.Dialogs
{
    [Serializable]
    public class PriceCalculationForm
    {
        [Prompt("Откуда везти?")]
        public string RouteFrom;

        [Prompt("Куда везти?")]
        public string RouteTo;

        [Numeric(1, 20)]
        [Prompt("Какое число контейнеров нужно?")]
        public int ContainerCount;

        [Prompt("Укажите дату отправки?")]
        public DateTime LoadingDate;

        [Prompt("Какой груз вы хотите перевести?")]
        public string CargoTypeCode;

        public static IForm<PriceCalculationForm> BuildForm()
        {
            return new FormBuilder<PriceCalculationForm>()
                .Field(nameof(ContainerCount))
                .Field(nameof(RouteFrom))
                .Field(nameof(RouteTo))
                .Field(nameof(LoadingDate))
                .Field(nameof(CargoTypeCode))
                .Confirm("Вы хотите рассчитать стоимость отправки {LoadingDate:d MMMM yyyy} г. груза с кодом ЕТСНГ {CargoTypeCode} в {ContainerCount} конт. по пути следования {RouteFrom} - {RouteTo}?")
                .OnCompletion(FormCompleted)
                .Build();
        }

        private async static Task FormCompleted(IDialogContext context, PriceCalculationForm state)
        {
            var priceCalculator = new StubPriceCalculator();
            var price = priceCalculator.GetPrice(state.RouteFrom, state.RouteTo, state.LoadingDate, state.CargoTypeCode, state.ContainerCount);

            var receipt = new ReceiptCard(title: "Title",
                                       facts: new List<Fact>
                                              {
                                                 new Fact("Место отправки", state.RouteFrom),
                                                 new Fact("Место доставки", state.RouteTo),
                                                 new Fact("Дата отправки", state.LoadingDate.ToString("dd MMM yyyy") + "г."),
                                                 new Fact("Контейнеры", state.ContainerCount + " x 20 футов (30 тонн)")
                                              },
                                       total: price.ToString() + " р.",
                                       buttons: new List<CardAction>
                                                {
                                                    new CardAction(ActionTypes.OpenUrl, title: "Перейти к оформлению", value: "https://isales.trcont.ru/"),
                                                    new CardAction(ActionTypes.PostBack, title: "Сохранить расчет", value: "Save")
                                                }
                                       );

            var message = context.MakeMessage();
            message.Attachments.Add(receipt.ToAttachment());
            await context.PostAsync(message);

            context.Done(state);
        }
    }

    public enum ContainerTypes: int
    {
        Feet20Tons24 = 1,

        Feet20Tons30 = 2,

        Feet40Tons30 = 3
    }
}
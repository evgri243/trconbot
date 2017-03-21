using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TrConBot.Api.Services.Stubs
{
    public class StubQnaSeracher : IQnaSearcher
    {
        private string _stubPattern = @"включ.*стоим";
        private QnaAnswer _stubAnswer = new QnaAnswer
        {
            Answer = "В стоимость услуги при отправке от терминала отправления до терминала назначения включены:\n\n" +
                     "* контейнер\n" +
                     "* запорно-пломбировочное устройство\n" +
                     "* вагона для перевозки\n" +
                     "* услуги по отправлению груженого Контейнера на железнодорожной станции\n" +
                     "* железнодорожный тариф\n" +
                     "* сбор за охрану и сопровождение груза в пути следования\n",
            Links = new List<QnaLinkEntry>
            {
                new QnaLinkEntry {Title = "От склада отправления – до станции назначения", Uri = new Uri(@"https://isales.trcont.ru/contacts/agreements/Коммерческие%20условия/ОТ%20СКЛАДА%20ОТПРАВЛЕНИЯ%20–%20ДО%20СТАНЦИИ%20НАЗНАЧЕНИЯ.htm")},
                new QnaLinkEntry {Title = "От склада отправления – до склада назначения", Uri = new Uri(@"https://isales.trcont.ru/contacts/agreements/Коммерческие%20условия/ОТ%20СКЛАДА%20ОТПРАВЛЕНИЯ%20–%20ДО%20СКЛАДА%20НАЗНАЧЕНИЯ.htm")},
                new QnaLinkEntry {Title = "От терминала отправления – до порта назначения с участием водного транспорта", Uri = new Uri(@"https://isales.trcont.ru/contacts/agreements/Коммерческие%20условия/ОТ%20СТАНЦИИ%20ОТПРАВЛЕНИЯ%20–%20ДО%20ПОРТА%20НАЗНАЧЕНИЯ%20В%20ПСЖВС.htm")},
                new QnaLinkEntry {Title = "Другие варианты", Uri = new Uri(@"https://isales.trcont.ru/Home/Info/3")}
            }
        };

        public QnaAnswer Search(string text)
        {
            if (!String.IsNullOrWhiteSpace(text) && Regex.IsMatch(text, _stubPattern))
                return _stubAnswer;
            else
                return null;
        }
    }
}
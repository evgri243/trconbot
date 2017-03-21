using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrConBot.Api.Services
{
    public class QnaAnswer
    {
        public string Answer { get; set; }
        public IList<QnaLinkEntry> Links { get; set; } = new List<QnaLinkEntry>();
    }

    public class QnaLinkEntry
    {
        public string Title { get; set; }
        public Uri Uri { get; set; }
    }
}
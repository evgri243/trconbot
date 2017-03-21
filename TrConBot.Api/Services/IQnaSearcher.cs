using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrConBot.Api.Services
{
    public interface IQnaSearcher
    {
        QnaAnswer Search(string text);
    }
}
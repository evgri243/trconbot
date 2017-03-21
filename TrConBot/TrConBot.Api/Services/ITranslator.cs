using System.Threading.Tasks;

namespace TrConBot.Api.Services
{
    public interface ITranslator
    {
        Task<string> Translate(string text, string toLocale, string fromLocale = "Auto-Detect");
    }
}
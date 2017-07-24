using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace TrConBot.Api.Services
{

    public class BingTranslatorClient : ITranslator
    {
        private string _subscriptionKey;
        private string _secret;
        private CognitiveServicesAuthenticator _authenticator;

        public BingTranslatorClient(string subscriptionKey)
        {
            if (subscriptionKey == null) throw new ArgumentNullException(nameof(subscriptionKey));

            _subscriptionKey = subscriptionKey;
            _authenticator = new CognitiveServicesAuthenticator(subscriptionKey);
        }

        public async Task<string> Translate(string text, string toLocale, string fromLocale = "Auto-Detect")
        {
            if (fromLocale == null) throw new ArgumentNullException(nameof(fromLocale));
            if (toLocale == null) throw new ArgumentNullException(nameof(toLocale));
            if (String.IsNullOrWhiteSpace(text)) return text;
            if (toLocale == fromLocale) return text;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _authenticator.GetAccessTokenAsync());

                try
                {
                    string uri = Uri.EscapeUriString($"http://api.microsofttranslator.com/v2/Http.svc/Translate?text={text}&to={toLocale}&from={fromLocale}");
                    string response = await client.GetStringAsync(uri);
                    return ExtractTranslation(response);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        private string ExtractTranslation(string response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            var xTranslation = new System.Xml.XmlDocument();
            xTranslation.LoadXml(response);
            return xTranslation.InnerText;
        }
    }

}
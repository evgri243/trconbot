using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TrConBot.Api.Services
{
    public class CognitiveServicesAuthenticator : IDisposable
    {
        private string _accessUri => "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";
        private string _apiKey;
        public string _token;
        private Timer _tokenRenewTimer;

        private int RefreshTokenDuration { get; set; } = 9 * 60 * 1000;

        public CognitiveServicesAuthenticator(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (_token == null)
                _token = await RequestTokenAsync(_accessUri, _apiKey);

            if (_tokenRenewTimer == null)
                _tokenRenewTimer = new Timer(OnTokenExpiredCallbackAsync, null, RefreshTokenDuration, RefreshTokenDuration);

            return _token;
        }

        private async Task RenewAccessTokenAsync()
        {
            try
            {
                string newAccessToken = await RequestTokenAsync(_accessUri, _apiKey);
                _token = newAccessToken;
            }
            catch (Exception)
            {
                _token = null;
                throw;
            }
        }

        private async void OnTokenExpiredCallbackAsync(object stateInfo)
        {
            await RenewAccessTokenAsync();
        }

        private async Task<string> RequestTokenAsync(string accessUri, string apiKey)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
                var response = await client.PostAsync(accessUri, null);

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        #region IDisposable Implementation
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _tokenRenewTimer?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

}
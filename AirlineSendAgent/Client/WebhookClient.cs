using System.Net.Http.Headers;
using System.Text.Json;
using AirlineSendAgent.Dtos;

namespace AirlineSendAgent.Client
{
    public class WebhookClient : IWebhookClient
    {
        private readonly IHttpClientFactory _httpClient;

        public WebhookClient(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task SendWebhookNotification(FlightDetailChangePayloadDto flightDetail)
        {
            var serializedPayload = JsonSerializer.Serialize(flightDetail);
            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, flightDetail.WebhookURI);

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(serializedPayload);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                using(var response = await client.SendAsync(request))
                {
                    Console.WriteLine("Success");
                    response.EnsureSuccessStatusCode();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Unsuccessful {ex.Message}");
            }
        }
    }
}
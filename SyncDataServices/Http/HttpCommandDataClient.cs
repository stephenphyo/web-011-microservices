using System.Text;
using System.Text.Json;
using PlatformService.DTOs;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        /* Properties */
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        /* Constructor */
        public HttpCommandDataClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        /* Methods */
        public async Task SendPlatformToCommand(PlatformReadDTO platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_config["CommandServiceAPI"]}/api/Platform", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Sync OK");
            }
            else
            {
                Console.WriteLine("Sync Failed");
            }
        }
    }
}
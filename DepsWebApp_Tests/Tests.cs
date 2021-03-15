using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DepsWebApp_Tests
{
    class Tests
    {
        static void Main(string[] args)
        {
            Setup();
            ClientAuthorizationController.Should_Return_StatusCode_2020_When_Parameters_Are_Valid(_httpClient);
            ClientAuthorizationController.Should_Return_StatusCode_400_When_Parameters_Are_Invalid(_httpClient);
            ClientAuthorizationController.Should_Return_StatusCode_409_When_User_Already_Exists(_httpClient);
            ClientRatesController.Should_Return_StatusCode_401_When_Unauthorized(_httpClient);
        }
        private static HttpClient _httpClient;

        public static void Setup()
        {

            var connectionSettings = JsonSerializer.Deserialize<ConnectionSettings>(File.ReadAllText("connectionSettings.json"));

            if (!File.Exists("connectionSettings.json"))
            {
                Console.WriteLine("File with connection settings was not located!");
                throw new FileNotFoundException("connectionSettings.json");
            }

            Uri.TryCreate(connectionSettings.BaseAddress, UriKind.Absolute, out var uri);

            _httpClient = new HttpClient()
            {
                BaseAddress = uri
            };
        }
       
        
        public static void SeparatorLine()
        {
            Console.WriteLine("=======================================================");
        }
    }
}

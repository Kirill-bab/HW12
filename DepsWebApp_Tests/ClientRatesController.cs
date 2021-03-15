using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DepsWebApp_Tests
{
    class ClientRatesController
    {
        public static void Should_Return_StatusCode_401_When_Unauthorized(HttpClient httpClient)
        {
            Tests.SeparatorLine();
            var response = Task.Run(async () =>
            {
                return await httpClient.GetAsync("/rates/UAH/USD?amount=2000");
            }).Result;

            Console.WriteLine("Test Get request /rates/UAH/USD?amount=2000 performed!");
            var responseBody = Task.Run(async () =>
            {
                return await response.Content.ReadAsStringAsync();
            }).Result;
            Console.WriteLine($"Get request /rates/UAH/USD?amount=2000 returned: {responseBody}\n with Status code: {response.StatusCode}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Get request /rates/UAH/USD?amount=2000 was successfull!");
            }
            else
            {
                Console.WriteLine("Get request /rates/UAH/USD?amount=2000 has failed!");
            }
        }

        public static void Should_Return_StatusCode_200_When_Authorized(HttpClient httpClient)
        {
            Tests.SeparatorLine();
            var encodedData = System.Convert
                .ToBase64String(System.Text.Encoding.UTF8
                .GetBytes("Some_not_empty_login" + ":" + "Some_not_empty_password"));
            var message = new HttpRequestMessage(HttpMethod.Get, "/rates/UAH/USD?amount=2000");
            Console.WriteLine(encodedData);
            message.Headers.Authorization = new System.Net.Http.Headers
                .AuthenticationHeaderValue("custom", encodedData);
            var response = Task.Run(async () =>
            {
                return await httpClient.SendAsync(message);
            }).Result;

            Console.WriteLine("Test Get request /rates/UAH/USD?amount=2000 performed!");
            var responseBody = Task.Run(async () =>
            {
                return await response.Content.ReadAsStringAsync();
            }).Result;
            Console.WriteLine($"Get request /rates/UAH/USD?amount=2000 returned: {responseBody}\n with Status code: {response.StatusCode}");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Get request /rates/UAH/USD?amount=2000 was successfull!");
            }
            else
            {
                Console.WriteLine("Get request /rates/UAH/USD?amount=2000 has failed!");
            }
        }
    }
}

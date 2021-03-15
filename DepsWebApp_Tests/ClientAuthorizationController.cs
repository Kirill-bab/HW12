using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DepsWebApp_Tests
{
    class ClientAuthorizationController
    {
        public static void Should_Return_StatusCode_200_When_Parameters_Are_Valid(HttpClient httpClient)
        {
            Tests.SeparatorLine();
            var response = Task.Run(async () =>
            {
                return await httpClient.PostAsJsonAsync("/authorization/register", new
                {
                    login = "Some_not_empty_login",
                    password = "Some_not_empty_password"
                });
            }).Result;

            Console.WriteLine("Test Post request /authorization/register performed!");
            var responseBody = Task.Run(async () =>
            {
                return await response.Content.ReadAsStringAsync();
            }).Result;
            Console.WriteLine($"Post request /authorization/register returned: {responseBody}\n with Status code: {response.StatusCode}");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Post request /authorization/register was successfull!");
            }
            else
            {
                Console.WriteLine("Post request /authorization/register has failed!");
            }
        }

        public static void Should_Return_StatusCode_400_When_Parameters_Are_Invalid(HttpClient httpClient)
        {
            Tests.SeparatorLine();
            var response = Task.Run(async () =>
            {
                return await httpClient.PostAsJsonAsync("/authorization/register", new
                {
                    login = 230,
                });
            }).Result;

            Console.WriteLine("Test Post request /authorization/register performed!");
            var responseBody = Task.Run(async () =>
            {
                return await response.Content.ReadAsStringAsync();
            }).Result;
            Console.WriteLine($"Post request /authorization/register returned: {responseBody}\n with Status code: {response.StatusCode}");

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Console.WriteLine("Post request /authorization/register was successfull!");
            }
            else
            {
                Console.WriteLine("Post request /authorization/register has failed!");
            }
        }

        public static void Should_Return_StatusCode_409_When_User_Already_Exists(HttpClient httpClient)
        {
            Tests.SeparatorLine();
            var response = Task.Run(async () =>
            {
                return await httpClient.PostAsJsonAsync("/authorization/register", new
                {
                    login = "Some_not_empty_login",
                    password = "Some_not_empty_password"
                });
            }).Result;

            Console.WriteLine("Test Post request /authorization/register performed!");
            var responseBody = Task.Run(async () =>
            {
                return await response.Content.ReadAsStringAsync();
            }).Result;
            Console.WriteLine($"Post request /authorization/register returned: {responseBody}\n with Status code: {response.StatusCode}");

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                Console.WriteLine("Post request /authorization/register was successfull!");
            }
            else
            {
                Console.WriteLine("Post request /authorization/register has failed!");
            }
        }
    }
}

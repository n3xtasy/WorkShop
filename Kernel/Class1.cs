using Kernel.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WSAPI.Models.User;

namespace Kernel
{
    public class Authentication
    {
        public static async Task<Task> Login(string email, string password)
        {
            var response = await Api.HttpClient.PostAsJsonAsync("authentication/login", new AuthenticateUserViewModel()
            {
                Email = email,
                Password = password
            });

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(await response.Content.ReadAsStringAsync());
            }

            var token = JsonConvert.DeserializeObject<TokenData>(await response.Content.ReadAsStringAsync());

            Api.HttpClient.DefaultRequestHeaders.Clear();

            Api.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Token}");

            return Task.CompletedTask;
        }

        public static async void Registration(string email, string password, string confirmPassword)
        {
            var response = await Api.HttpClient.PostAsJsonAsync("authentication/registration", new CreateUserViewModel()
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            });

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(await response.Content.ReadAsStringAsync());
            }
        }
    }
}

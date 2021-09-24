using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kernel
{
    public class Api
    {
        private static HttpClient httpClient;
        public static HttpClient HttpClient
        {
            get
            {
                if (httpClient == null)
                {
                    httpClient = new HttpClient();
                    httpClient.BaseAddress = new Uri("https://localhost:44365/");
                }

                return httpClient;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Exemplo3.CRMClient
{
    public class CRMRestClient
    {
        private HttpClient client;

        public CRMRestClient()
        {
            client = new HttpClient();
            //TODO - configure o endereço do CRM
            client.BaseAddress = new Uri("http://localhost:12345/api/");

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            //Mount the credentials in base64 encoding
            byte[] str1Byte = System.Text.Encoding.UTF8.GetBytes(string.Format("{0}:{1}", "crmwebapi", "crmwebapi"));
            String plaintext = Convert.ToBase64String(str1Byte);

            //Set the authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", plaintext);
        }

        public Customer GetCustomerByEmail(string email)
        {
            // Get a customer by ID
            HttpResponseMessage response = client.GetAsync("customers/byemail?email=" + email).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                Customer customer = (Customer)response.Content.ReadAsAsync<Customer>().Result;
                return customer;
            }
            return null;
        }
    }
}
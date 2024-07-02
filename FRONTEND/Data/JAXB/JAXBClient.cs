using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FRONTEND.Data.JAXB
{
    public class JAXBClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _serverUrl = "http://localhost:8001/validate";

        public JAXBClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> ValidateXmlAsync(string xmlContent)
        {
            try
            {
                var httpContent = new StringContent(xmlContent);
                httpContent.Headers.ContentType.MediaType = "application/xml";

                HttpResponseMessage response = await _httpClient.PostAsync(_serverUrl, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Read the error response content
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    return $"Error: {errorResponse}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception occurred: {ex.Message}";
            }
        }
    }
}

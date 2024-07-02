using System;
using System.Text;

namespace FRONTEND.Data.SOAP;

public class SOAPClient
{
    private readonly HttpClient client;

    public SOAPClient()
    {
        client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5181/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/xml"));
    }

    public async Task<string> GenerateAndSearchBooksXmlAsync(string searchTerm)
    {
        try
        {
            // Construct SOAP envelope
            string soapEnvelopeString = $@"
                <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                        <GenerateAndSearchBooksXml xmlns=""http://tempuri.org/"">
                            <searchTerm>{searchTerm}</searchTerm>
                        </GenerateAndSearchBooksXml>
                    </soap:Body>
                </soap:Envelope>";

            // Convert SOAP envelope string to byte array
            byte[] byteArray = Encoding.UTF8.GetBytes(soapEnvelopeString);

            // Prepare request
            using (var content = new ByteArrayContent(byteArray))
            {
                content.Headers.Add("Content-Type", "text/xml; charset=utf-8");

                // Send request
                HttpResponseMessage response = await client.PostAsync("BookService.asmx", content);

                // Check response status
                response.EnsureSuccessStatusCode();

                // Read response asynchronously
                string responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions appropriately
            throw new Exception("Error calling SOAP service", ex);
        }
    }
}

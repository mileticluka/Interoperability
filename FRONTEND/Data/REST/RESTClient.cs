using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using DAL.Model;

namespace FRONTEND.Data.REST
{
    public class RESTClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public RESTClient(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        private async Task AddBearerTokenToHeader()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<bool> GetAuthorised()
        {
            await AddBearerTokenToHeader();
            if (await _httpClient.GetFromJsonAsync<IEnumerable<Book>>($"http://localhost:5181/api/Book") != null)
            {
                return true;
            }
            return false;
        }


        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {

            await AddBearerTokenToHeader();
            return await _httpClient.GetFromJsonAsync<IEnumerable<Book>>($"http://localhost:5181/api/Book");

        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            await AddBearerTokenToHeader();
            return await _httpClient.GetFromJsonAsync<Book>($"http://localhost:5181/api/Book/{id}");
        }

        public async Task UpdateBookAsync(int id, Book updatedBook)
        {
            await AddBearerTokenToHeader();
            await _httpClient.PutAsJsonAsync($"http://localhost:5181/api/Book/{id}", updatedBook);
        }

        public async Task DeleteBookAsync(int id)
        {
            await AddBearerTokenToHeader();
            await _httpClient.DeleteAsync($"http://localhost:5181/api/Book/{id}");
        }

        public async Task<string> PostBookXmlAsync(string xmlContent, string endpoint)
        {
            await AddBearerTokenToHeader();
            var content = new StringContent(xmlContent);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");

            var response = await _httpClient.PostAsync($"http://localhost:5181/api/Book/post-{endpoint}", content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}

using LexiFlow.BLL.Models.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public class WordsApiClient : IWordsApiClient
    {
        private readonly HttpClient _httpClient;

        public WordsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WordsApiResponseModel?> GetWordAsync(
            string word,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(
                $"words/{word}",
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<WordsApiResponseModel>(
                cancellationToken: cancellationToken);
        }
    }
}

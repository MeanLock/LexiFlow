using LexiFlow.BLL.Models.Card;
using LexiFlow.BLL.Models.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public class DictionaryService : IDictionaryService
    {
        private readonly IWordsApiClient _wordsApiClient;
        private readonly HttpClient _httpClient;


        public DictionaryService(
            IWordsApiClient wordsApiClient, HttpClient httpClient)
        {
            _wordsApiClient = wordsApiClient;
            _httpClient = httpClient;   
        }

        public async Task<WordDefinitionResponseModel?> GetWordDefinitionAsync(
            string word,
            CancellationToken cancellationToken = default)
        {
            var response =
                await _wordsApiClient.GetWordAsync(
                    word,
                    cancellationToken);

            if (response is null)
            {
                return null;
            }

            return new WordDefinitionResponseModel
            {
                Word = response.Word,

                PartOfSpeeches = response.Results
                    .Where(x => !string.IsNullOrWhiteSpace(x.PartOfSpeech))
                    .GroupBy(x => x.PartOfSpeech)
                    .Select(group =>
                    {
                        string? phonetic = null;

                        if (response.Pronunciation is not null)
                        {
                            response.Pronunciation.TryGetValue(
                                group.Key,
                                out phonetic);

                            phonetic ??=
                                response.Pronunciation
                                    .GetValueOrDefault("all");
                        }

                        return new PartOfSpeechModel
                        {
                            PartOfSpeech = group.Key,

                            Phonetic = phonetic,

                            AudioUrl = string.Empty,

                            Meanings = group
                                .Select(x => new MeaningModel
                                {
                                    Definition = x.Definition,

                                    Example = x.Examples?.FirstOrDefault(),

                                    Synonyms = x.Synonyms ?? [],

                                    Antonyms = x.Antonyms ?? []
                                })
                                .ToList()
                        };
                    })
                    .ToList()
            };
        }

        public async Task<DictionaryResponse?> GetMeaningAsync(string term)
        {
            var url =
                $"https://api.dictionaryapi.dev/api/v2/entries/en/{Uri.EscapeDataString(term)}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            var dictionaryData =
                JsonSerializer.Deserialize<List<DictionaryApiResponse>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            var entry = dictionaryData?.FirstOrDefault();

            if (entry == null)
            {
                return null;
            }

            var result = new DictionaryResponse
            {
                Phonetic = entry.Phonetic,

                AudioUrl = entry.Phonetics?
                    .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Audio))
                    ?.Audio
            };

            if (entry.Meanings != null)
            {
                foreach (var meaning in entry.Meanings)
                {
                    var firstDefinition =
                        meaning.Definitions?.FirstOrDefault();

                    result.Meanings.Add(new DictionaryMeaningResponse
                    {
                        PartOfSpeech = meaning.PartOfSpeech ?? string.Empty,

                        Definition = firstDefinition?.Definition,

                        Example = firstDefinition?.Example,

                        Synonyms = firstDefinition?.Synonyms ?? [],

                        Antonyms = firstDefinition?.Antonyms ?? []
                    });
                }
            }

            return result;
        }

    }
}

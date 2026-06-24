using LexiFlow.BLL.Models.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public class DictionaryService : IDictionaryService
    {
        private readonly IWordsApiClient _wordsApiClient;

        public DictionaryService(
            IWordsApiClient wordsApiClient)
        {
            _wordsApiClient = wordsApiClient;
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
    }
}

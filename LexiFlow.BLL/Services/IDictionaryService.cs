using LexiFlow.BLL.Models.Card;
using LexiFlow.BLL.Models.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public interface IDictionaryService
    {
        Task<WordDefinitionResponseModel?> GetWordDefinitionAsync(
        string word,
        CancellationToken cancellationToken = default);

        Task<DictionaryResponse?> GetMeaningAsync(string term);

    }
}

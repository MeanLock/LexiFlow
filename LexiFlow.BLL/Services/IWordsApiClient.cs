using LexiFlow.BLL.Models.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public interface IWordsApiClient
    {
        Task<WordsApiResponseModel?> GetWordAsync(
       string word,
       CancellationToken cancellationToken = default);
    }
}

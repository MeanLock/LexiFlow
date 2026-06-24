using LexiFlow.BLL.Models.Deck;
using LexiFlow.BLL.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public interface IDeckService
    {
        Task<ResponseResult> AddDeckAsync(Guid userId, CreateDeckRequest request);

        Task<ResponseResult> UpdateDeckAsync(Guid userId, UpdateDeckRequest request);

        Task<ResponseResult> DeleteDeckAsync(Guid userId, Guid deckId);

        Task<DeckResponse> GetDeckByIdAsync(Guid userId, Guid deckId);

        Task<IEnumerable<DeckResponse>> GetAllDecksAsync(Guid userId);
    }
}

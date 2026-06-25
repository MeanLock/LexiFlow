using LexiFlow.BLL.Models.Card;
using LexiFlow.BLL.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public interface ICardService
    {
        Task<List<CardResponse>> GetDueCardsAsync(Guid userId, Guid deckId);

        Task<ResponseResult> CreateCardAsync(
        Guid userId,
        CreateCardRequest request);

        Task<ResponseResult> UpdateCardAsync(
            Guid userId,
            Guid cardId,
            CreateCardRequest request);

        Task<ResponseResult> DeleteCardAsync(
            Guid userId,
            Guid cardId);

        Task<CardResponse> GetCardByIdAsync(
            Guid userId,
            Guid cardId);

        Task<IEnumerable<CardResponse>> GetCardsByDeckAsync(
            Guid userId,
            Guid deckId);
    }
}

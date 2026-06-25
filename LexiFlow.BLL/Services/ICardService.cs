using LexiFlow.BLL.Models.Card;
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
    }
}

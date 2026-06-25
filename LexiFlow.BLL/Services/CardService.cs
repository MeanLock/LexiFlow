using LexiFlow.BLL.Models.Card;
using LexiFlow.DAL.Entities;
using LexiFlow.DAL.Repositories;
using LexiFlow.DAL.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public class CardService : ICardService
    {
        private readonly IGenericRepository<Deck> _deckRepository;
        private readonly IGenericRepository<Card> _cardRepository;
        private readonly IGenericRepository<ReviewState> _reviewStateRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;

        public CardService(
            IGenericRepository<Deck> deckRepository,
            IGenericRepository<Card> cardRepository,
            IGenericRepository<ReviewState> reviewStateRepository,
            IUnitOfWork unitOfWork,
            HttpClient httpClient)
        {
            _deckRepository = deckRepository;
            _cardRepository = cardRepository;
            _reviewStateRepository = reviewStateRepository;
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;

        }


        public async Task<List<CardResponse>> GetDueCardsAsync(Guid userId, Guid deckId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var states = await _reviewStateRepository
                .FindAll(x =>
                    x.UserId == userId &&
                    x.Card.DeckId == deckId &&
                    x.DueDate <= today,
                    x => x.Card)
                .ToListAsync();

            return states.Select(x => new CardResponse
            {
                Id = x.CardId,
                Term = x.Card.Term,
                PrimaryMeaning = x.Card.PrimaryMeaning,

            }).ToList();
        }


    }
}

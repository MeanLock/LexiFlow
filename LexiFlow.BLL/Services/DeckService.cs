using LexiFlow.BLL.Models;
using LexiFlow.BLL.Models.Deck;
using LexiFlow.BLL.Models.User;
using LexiFlow.DAL.Entities;
using LexiFlow.DAL.Repositories;
using LexiFlow.DAL.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public class DeckService : IDeckService
    {
        private readonly IGenericRepository<Deck> _deckRepository;
        private readonly IGenericRepository<Card> _cardRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeckService(
            IGenericRepository<Deck> deckRepository,
            IGenericRepository<Card> cardRepository,
            IUnitOfWork unitOfWork)
        {
            _deckRepository = deckRepository;
            _cardRepository = cardRepository;
            _unitOfWork = unitOfWork;

        }

        public async Task<ResponseResult> AddDeckAsync(Guid userId, CreateDeckRequest request)
        {
            var deck = new Deck
            {
                Id = Guid.NewGuid(),
                OwnerId = userId,
                Name = request.Name,
                Description = request.Description,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            _deckRepository.Add(deck);

            await _unitOfWork.SaveChangesAsync();

            return ResponseResult.Success("Deck created successfully.");
        }

        public async Task<ResponseResult> UpdateDeckAsync(Guid userId, Guid deckId, UpdateDeckRequest request)
        {
            var deck = await _deckRepository.FindSingleAsync(
                x => x.Id == deckId
                  && x.OwnerId == userId
                  && !x.IsDeleted);

            if (deck == null)
                throw new Exception("Deck not found");

            deck.Name = request.Name;
            deck.Description = request.Description;
            deck.UpdatedAt = DateTime.UtcNow;

            _deckRepository.Update(deck);

            await _unitOfWork.SaveChangesAsync();

            return ResponseResult.Success("Deck updated successfully.");
        }

        public async Task<IEnumerable<DeckResponse>> GetAllDecksAsync(Guid userId)
        {
            var decks = await _deckRepository
                .FindAll(x => x.OwnerId == userId && !x.IsDeleted, x => x.Cards)
                .ToListAsync();

            return decks.Select(x => new DeckResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                TotalCards = x.Cards.Count(c => !c.IsDeleted),
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt ?? x.CreatedAt
            });
        }

        public async Task<DeckResponse> GetDeckByIdAsync(Guid userId, Guid deckId)
        {
            var deck = await _deckRepository.FindSingleAsync( x => x.Id == deckId && x.OwnerId == userId && !x.IsDeleted, x => x.Cards);

            if (deck == null)
                throw new Exception("Deck not found");

            return new DeckResponse
            {
                Id = deck.Id,
                Name = deck.Name,
                Description = deck.Description,
                TotalCards = deck.Cards.Count(c => !c.IsDeleted),
                CreatedAt = deck.CreatedAt,
                UpdatedAt = deck.UpdatedAt ?? deck.CreatedAt,
            };
        }

        public async Task<ResponseResult> DeleteDeckAsync(Guid userId, Guid deckId)
        {
            var deck = await _deckRepository.FindSingleAsync(x => x.Id == deckId && x.OwnerId == userId && !x.IsDeleted, x => x.Cards);

            if (deck == null)
                throw new Exception("Deck not found");

            deck.IsDeleted = true;
            deck.UpdatedAt = DateTime.UtcNow;

            foreach (var card in deck.Cards)
            {
                card.IsDeleted = true;
                card.UpdatedAt = DateTime.UtcNow;
            }

            _deckRepository.Update(deck);

            await _unitOfWork.SaveChangesAsync();

            return ResponseResult.Success("Deck deleted successfully.");
        }
    }
}

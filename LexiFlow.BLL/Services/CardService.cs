using LexiFlow.BLL.Models.Card;
using LexiFlow.BLL.Models.User;
using LexiFlow.DAL.Entities;
using LexiFlow.DAL.Enums;
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
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Card> _cardRepository;
        private readonly IGenericRepository<VocabularyCardDetail> _vocabularyCardDetailRepository;
        private readonly IGenericRepository<VocabularyMeaning> _vocabularyMeaningRepository;
        private readonly IGenericRepository<CollocationCardDetail> _collocationRepository;
        private readonly IGenericRepository<IdiomCardDetail> _idiomRepository;
        private readonly IGenericRepository<PhraseCardDetail> _phraseRepository;
        private readonly IGenericRepository<ReviewState> _reviewStateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CardService(
            IGenericRepository<Deck> deckRepository,
            IGenericRepository<Card> cardRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<VocabularyCardDetail> vocabularyCardDetailRepository,
            IGenericRepository<VocabularyMeaning> vocabularyMeaningRepository,
            IGenericRepository<CollocationCardDetail> collocationRepository,
            IGenericRepository<IdiomCardDetail> idiomRepository,
            IGenericRepository<PhraseCardDetail> phraseRepository,
            IGenericRepository<ReviewState> reviewStateRepository,
            IUnitOfWork unitOfWork)
        {
            _deckRepository = deckRepository;
            _cardRepository = cardRepository;
            _userRepository = userRepository;
            _reviewStateRepository = reviewStateRepository;
            _collocationRepository = collocationRepository;
            _idiomRepository = idiomRepository;
            _phraseRepository = phraseRepository;
            _vocabularyCardDetailRepository = vocabularyCardDetailRepository;
            _vocabularyMeaningRepository = vocabularyMeaningRepository;
            _unitOfWork = unitOfWork;

        }

        public async Task<ResponseResult> CreateCardAsync(Guid userId, CreateCardRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var deck = await _deckRepository.FindSingleAsync(
                    x => x.Id == request.DeckId
                      && x.OwnerId == userId
                      && !x.IsDeleted);

                if (deck == null)
                    throw new Exception("Deck not found");

                var user = await _userRepository.FindSingleAsync(x => x.Id == userId);

                if (user == null)
                    throw new Exception("User not found");

                if (request.SourceType != CardSourceType.Manual && request.IsPublic)
                {
                    throw new Exception(
                        "Only manual card can be public");
                }

                var card = new Card
                {
                    Id = Guid.NewGuid(),
                    DeckId = request.DeckId,

                    CardType = request.CardType,

                    Term = request.Term,
                    NormalizedTerm = request.Term.Trim().ToLowerInvariant(),

                    PrimaryMeaning = request.PrimaryMeaning,

                    IsPublic = request.IsPublic,

                    SourceType = request.SourceType,

                    IsDeleted = false,

                    CreatedAt = DateTime.UtcNow
                };

                _cardRepository.Add(card);

                if (request.CardType == CardType.Vocabulary)
                {
                    if (request.Vocabulary == null)
                        throw new Exception("Vocabulary detail required");

                    var detail = new VocabularyCardDetail
                    {
                        CardId = card.Id,
                        Phonetic = request.Vocabulary.Phonetic,
                        AudioUrl = request.Vocabulary.AudioUrl,
                        CefrLevel = request.Vocabulary.CefrLevel
                    };

                    _vocabularyCardDetailRepository.Add(detail);

                    foreach (var meaning in request.Vocabulary.Meanings)
                    {
                        var vocabularyMeaning =
                            new VocabularyMeaning
                            {
                                Id = Guid.NewGuid(),
                                CardId = card.Id,

                                PartOfSpeech =
                                    meaning.PartOfSpeech,

                                Definition =
                                    meaning.Definition,

                                Example =
                                    meaning.Example,

                                ExampleMeaning =
                                    meaning.ExampleMeaning,

                                Synonyms =
                                    meaning.Synonyms == null
                                        ? null
                                        : string.Join(",",
                                            meaning.Synonyms),

                                Antonyms =
                                    meaning.Antonyms == null
                                        ? null
                                        : string.Join(",",
                                            meaning.Antonyms)
                            };

                        _vocabularyMeaningRepository
                            .Add(vocabularyMeaning);
                    }
                }


                if (request.CardType == CardType.Collocation)
                {
                    if (request.Collocation == null)
                        throw new Exception("Collocation detail required");

                    _collocationRepository.Add(
                        new CollocationCardDetail
                        {
                            CardId = card.Id,

                            Pattern =
                                request.Collocation.Pattern,

                            Explanation =
                                request.Collocation.Explanation,

                            Example =
                                request.Collocation.Example,

                            ExampleMeaning =
                                request.Collocation.ExampleMeaning
                        });
                }

                if (request.CardType == CardType.Idiom)
                {
                    if (request.Idiom == null)
                        throw new Exception("Idiom detail required");

                    _idiomRepository.Add(
                        new IdiomCardDetail
                        {
                            CardId = card.Id,

                            LiteralMeaning =
                                request.Idiom.LiteralMeaning,

                            FigurativeMeaning =
                                request.Idiom.FigurativeMeaning,

                            UsageNote =
                                request.Idiom.UsageNote,

                            Example =
                                request.Idiom.Example,

                            ExampleMeaning =
                                request.Idiom.ExampleMeaning
                        });
                }

                if (request.CardType == CardType.Phrase)
                {
                    if (request.Phrase == null)
                        throw new Exception("Phrase detail required");

                    _phraseRepository.Add(
                        new PhraseCardDetail
                        {
                            CardId = card.Id,

                            PhraseType =
                                request.Phrase.PhraseType,

                            UsageNote =
                                request.Phrase.UsageNote,

                            Example =
                                request.Phrase.Example,

                            ExampleMeaning =
                                request.Phrase.ExampleMeaning
                        });
                }

                var reviewState = new ReviewState
                {
                    Id = Guid.NewGuid(),

                    UserId = userId,

                    CardId = card.Id,

                    DueDate =
                        DateOnly.FromDateTime(DateTime.UtcNow),

                    EaseFactor = 2.5m,

                    IntervalDays = 0,

                    Repetition = 0,

                    Lapses = 0,

                    CreatedAt = DateTime.UtcNow
                };

                _reviewStateRepository.Add(reviewState);

                if (request.IsPublic)
                {
                    user.ContributeScore++;

                    _userRepository.Update(user);
                }

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return ResponseResult.Success(
                    "Card created successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<ResponseResult> DeleteCardAsync(Guid userId, Guid cardId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var card = await _cardRepository.FindSingleAsync(x => x.Id == cardId && !x.IsDeleted, x => x.Deck);

                if (card == null)
                    throw new Exception("Card not found");

                if (card.Deck.OwnerId != userId)
                    throw new Exception("Permission denied");

                var reviewStates = await _reviewStateRepository
                    .FindAll(x => x.CardId == cardId)
                    .ToListAsync();

                foreach (var state in reviewStates)
                {
                    _reviewStateRepository.Delete(state);
                }

                card.IsDeleted = true;
                card.UpdatedAt = DateTime.UtcNow;

                _cardRepository.Update(card);

                if (card.IsPublic)
                {
                    var user = await _userRepository.FindSingleAsync(x => x.Id == userId);

                    if (user != null)
                    {
                        user.ContributeScore =
                            Math.Max(0, user.ContributeScore - 1);

                        _userRepository.Update(user);
                    }
                }


                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return ResponseResult.Success(
                    "Card deleted successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<CardResponse> GetCardByIdAsync(Guid userId, Guid cardId)
        {
            var card = await _cardRepository.FindSingleAsync(
                x => x.Id == cardId
                  && !x.IsDeleted,
                x => x.Deck,
                x => x.VocabularyCardDetail,
                x => x.VocabularyMeanings,
                x => x.CollocationCardDetail,
                x => x.IdiomCardDetail,
                x => x.PhraseCardDetail);

            if (card == null)
                throw new Exception("Card not found");

            if (card.Deck.OwnerId != userId)
                throw new Exception("Permission denied");

            return new CardResponse
            {
                Id = card.Id,
                Term = card.Term,
                PrimaryMeaning = card.PrimaryMeaning,
                CardType = card.CardType,
                SourceType = card.SourceType,
                IsPublic = card.IsPublic,
                Detail = BuildCardDetail(card)
            };
        }

        public async Task<IEnumerable<CardResponse>> GetCardsByDeckAsync(Guid userId, Guid deckId)
        {
            var deck = await _deckRepository.FindSingleAsync(
                x => x.Id == deckId
                  && x.OwnerId == userId
                  && !x.IsDeleted);

            if (deck == null)
                throw new Exception("Deck not found");

            var cards = await _cardRepository
                .FindAll(
                    x => x.DeckId == deckId
                      && !x.IsDeleted,
                    x => x.VocabularyCardDetail,
                    x => x.VocabularyMeanings,
                    x => x.CollocationCardDetail,
                    x => x.IdiomCardDetail,
                    x => x.PhraseCardDetail)
                .ToListAsync();

            return cards.Select(card => new CardResponse
            {
                Id = card.Id,
                Term = card.Term,
                PrimaryMeaning = card.PrimaryMeaning,
                CardType = card.CardType,
                SourceType = card.SourceType,
                IsPublic = card.IsPublic,
                Detail = BuildCardDetail(card)
            });
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
                CardType = x.Card.CardType,
                SourceType = x.Card.SourceType,
                IsPublic = x.Card.IsPublic,
                Detail = BuildCardDetail(x.Card)

            }).ToList();
        }

        public async Task<ResponseResult> UpdateCardAsync(Guid userId, Guid cardId, CreateCardRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var card = await _cardRepository.FindSingleAsync(
                    x => x.Id == cardId && !x.IsDeleted,
                    x => x.Deck);

                if (card == null)
                    throw new Exception("Card not found");

                if (card.Deck.OwnerId != userId)
                    throw new Exception("Unauthorized");

                card.Term = request.Term;
                card.NormalizedTerm = request.Term.Trim().ToLower();
                card.PrimaryMeaning = request.PrimaryMeaning;
                card.CardType = request.CardType;
                card.SourceType = request.SourceType;
                card.IsPublic = request.IsPublic;
                card.UpdatedAt = DateTime.UtcNow;

                var owner = await _userRepository.FindSingleAsync(x => x.Id == userId);

                if (owner != null)
                {
                    if (request.SourceType == CardSourceType.Manual && request.IsPublic)
                    {
                        owner.ContributeScore += 1;
                        _userRepository.Update(owner);
                    }

                    if (request.SourceType != CardSourceType.Manual && request.IsPublic)
                    {
                        throw new Exception("Only manual cards can be public");
                    }
                }


                await UpdateCardDetail(card, request);

                _cardRepository.Update(card);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return ResponseResult.Success("Card updated successfully");
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        private object? BuildCardDetail(Card card)
        {
            switch (card.CardType)
            {
                case CardType.Vocabulary:
                    return new
                    {
                        card.VocabularyCardDetail?.Phonetic,
                        card.VocabularyCardDetail?.AudioUrl,
                        card.VocabularyCardDetail?.CefrLevel,

                        Meanings = card.VocabularyMeanings.Select(x => new
                        {
                            x.PartOfSpeech,
                            x.Definition,
                            x.Example,
                            x.ExampleMeaning,

                            Synonyms = string.IsNullOrWhiteSpace(x.Synonyms) ? new List<string>() : x.Synonyms.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),

                            Antonyms = string.IsNullOrWhiteSpace(x.Antonyms) ? new List<string>() : x.Antonyms.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                        })
                    };

                case CardType.Collocation:
                    return new
                    {
                        card.CollocationCardDetail?.Pattern,
                        card.CollocationCardDetail?.Explanation,
                        card.CollocationCardDetail?.Example,
                        card.CollocationCardDetail?.ExampleMeaning
                    };

                case CardType.Idiom:
                    return new
                    {
                        card.IdiomCardDetail?.LiteralMeaning,
                        card.IdiomCardDetail?.FigurativeMeaning,
                        card.IdiomCardDetail?.UsageNote,
                        card.IdiomCardDetail?.Example,
                        card.IdiomCardDetail?.ExampleMeaning
                    };

                case CardType.Phrase:
                    return new
                    {
                        card.PhraseCardDetail?.PhraseType,
                        card.PhraseCardDetail?.UsageNote,
                        card.PhraseCardDetail?.Example,
                        card.PhraseCardDetail?.ExampleMeaning
                    };

                default:
                    return null;
            }
        }

        private async Task UpdateCardDetail(Card card, CreateCardRequest request)
        {
            // VOCABULARY
            if (request.CardType == CardType.Vocabulary)
            {
                if (card.VocabularyCardDetail == null)
                {
                    card.VocabularyCardDetail = new VocabularyCardDetail
                    {
                        CardId = card.Id
                    };
                }

                card.VocabularyCardDetail.Phonetic = request.Vocabulary?.Phonetic;
                card.VocabularyCardDetail.AudioUrl = request.Vocabulary?.AudioUrl;
                card.VocabularyCardDetail.CefrLevel = request.Vocabulary?.CefrLevel;

                // meanings
                card.VocabularyMeanings.Clear();

                if (request.Vocabulary?.Meanings != null)
                {
                    foreach (var m in request.Vocabulary.Meanings)
                    {
                        card.VocabularyMeanings.Add(new VocabularyMeaning
                        {
                            Id = Guid.NewGuid(),
                            CardId = card.Id,
                            PartOfSpeech = m.PartOfSpeech,
                            Definition = m.Definition,
                            Example = m.Example,
                            ExampleMeaning = m.ExampleMeaning,
                            Synonyms = string.Join(",", m.Synonyms ?? new List<string>()),
                            Antonyms = string.Join(",", m.Antonyms ?? new List<string>())
                        });
                    }
                }
            }

            // IDIOM
            if (request.CardType == CardType.Idiom)
            {
                if (card.IdiomCardDetail == null)
                    card.IdiomCardDetail = new IdiomCardDetail { CardId = card.Id };

                card.IdiomCardDetail.LiteralMeaning = request.Idiom?.LiteralMeaning;
                card.IdiomCardDetail.FigurativeMeaning = request.Idiom?.FigurativeMeaning;
                card.IdiomCardDetail.Example = request.Idiom?.Example;
            }

            // PHRASE
            if (request.CardType == CardType.Phrase)
            {
                if (card.PhraseCardDetail == null)
                    card.PhraseCardDetail = new PhraseCardDetail { CardId = card.Id };

                card.PhraseCardDetail.PhraseType = request.Phrase?.PhraseType;
                card.PhraseCardDetail.Example = request.Phrase?.Example;
            }

            // COLLOCATION
            if (request.CardType == CardType.Collocation)
            {
                if (card.CollocationCardDetail == null)
                    card.CollocationCardDetail = new CollocationCardDetail { CardId = card.Id };

                card.CollocationCardDetail.Pattern = request.Collocation?.Pattern;
                card.CollocationCardDetail.Example = request.Collocation?.Example;
            }
        }
    }
}

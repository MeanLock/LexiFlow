using LexiFlow.BLL.Models.ReviewRecord;
using LexiFlow.BLL.Models.ReviewSession;
using LexiFlow.BLL.Models.User;
using LexiFlow.DAL.Entities;
using LexiFlow.DAL.Repositories;
using LexiFlow.DAL.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public class ReviewSessionService : IReviewSessionService
    {
        private readonly IGenericRepository<ReviewSession> _reviewSessionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewSessionService(
            IGenericRepository<ReviewSession> reviewSessionRepository,
            IUnitOfWork unitOfWork)
        {
            _reviewSessionRepository = reviewSessionRepository;
            _unitOfWork = unitOfWork;

        }

        public async Task<ResponseResult> StartSessionAsync(Guid userId, Guid deckId)
        {
            var session = new ReviewSession
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DeckId = deckId,
                StartedAt = DateTime.UtcNow,
                TotalCards = 0,
                CorrectCount = 0
            };

            _reviewSessionRepository.Add(session);

            await _unitOfWork.SaveChangesAsync();

            return ResponseResult.Success("Review session start");
        }

        public async Task<ResponseResult> FinishSessionAsync(Guid userId, Guid sessionId)
        {
            var session = await _reviewSessionRepository.FindSingleAsync( x => x.Id == sessionId && x.UserId == userId);

            if (session == null)
                throw new Exception("Review session not found");

            if (session.CompletedAt != null)
                return ResponseResult.Success("Session already completed");

            session.CompletedAt = DateTime.UtcNow;

            // NOTE: nếu sau này cần chuẩn hơn thì join ReviewRecords để tính
            // session.TotalCards / CorrectCount ở ReviewService

            _reviewSessionRepository.Update(session);

            await _unitOfWork.SaveChangesAsync();

            return ResponseResult.Success("Review session finished");
        }

        public async Task<IEnumerable<ReviewSessionResponse>> GetAllSessionsAsync(Guid userId)
        {
            var sessions = await _reviewSessionRepository
                .FindAll(x => x.UserId == userId)
                .OrderByDescending(x => x.StartedAt)
                .ToListAsync();

            return sessions.Select(x => new ReviewSessionResponse
            {
                Id = x.Id,
                DeckId = x.DeckId,
                StartedAt = x.StartedAt,
                CompletedAt = x.CompletedAt,
                TotalCards = x.TotalCards,
                CorrectCount = x.CorrectCount
            });
        }

        public async Task<ReviewSessionDetailResponse> GetSessionByIdAsync(Guid userId, Guid sessionId)
        {
            var session = await _reviewSessionRepository.FindSingleAsync(x => x.Id == sessionId && x.UserId == userId, x => x.ReviewRecords);

            if (session == null)
                throw new Exception("Review session not found");

            return new ReviewSessionDetailResponse
            {
                Id = session.Id,
                DeckId = session.DeckId,
                StartedAt = session.StartedAt,
                CompletedAt = session.CompletedAt,
                TotalCards = session.TotalCards,
                CorrectCount = session.CorrectCount,

                Records = session.ReviewRecords.Select(r => new ReviewRecordResponse
                {
                    CardId = r.CardId,
                    Rating = (int)r.Rating,
                    IsCorrect = r.IsCorrect,
                    ReviewedAt = r.ReviewedAt,
                    ResponseTimeMs = r.ResponseTimeMs
                }).ToList()
            };

        }


    }
}

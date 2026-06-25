using LexiFlow.BLL.Models.Review;
using LexiFlow.BLL.Models.User;
using LexiFlow.DAL.Entities;
using LexiFlow.DAL.Repositories;
using LexiFlow.DAL.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IGenericRepository<ReviewState> _reviewStateRepository;
        private readonly IGenericRepository<Card> _cardRepository;
        private readonly IGenericRepository<ReviewRecord> _reviewRecordRepository;
        private readonly IGenericRepository<ReviewSession> _reviewSessionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(
            IGenericRepository<ReviewState> reviewStateRepository,
            IGenericRepository<Card> cardRepository,
            IGenericRepository<ReviewRecord> reviewRecordRepository,
            IGenericRepository<ReviewSession> reviewSessionRepository,
            IUnitOfWork unitOfWork)
        {
            _reviewStateRepository = reviewStateRepository;
            _cardRepository = cardRepository;
            _reviewRecordRepository = reviewRecordRepository;
            _reviewSessionRepository = reviewSessionRepository;
            _unitOfWork = unitOfWork;
        }

       
        public async Task<ResponseResult> SubmitReviewAsync(Guid userId, SubmitReviewRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.UtcNow;

                // 1. Get review state
                var state = await _reviewStateRepository.FindSingleAsync(x =>
                    x.UserId == userId &&
                    x.CardId == request.CardId);

                if (state == null)
                    throw new Exception("Review state not found");

                // 2. Get session
                var session = await _reviewSessionRepository.FindSingleAsync(x =>
                    x.Id == request.SessionId &&
                    x.UserId == userId);

                if (session == null)
                    throw new Exception("Session not found");

                // 3. Convert rating to q
                int q = (int)request.Rating;

                var oldEf = state.EaseFactor;
                var oldInterval = state.IntervalDays;
                var oldRepetition = state.Repetition;

                int newRepetition;
                int newInterval;
                int newLapses = state.Lapses;

                bool isCorrect = q >= 3;


                if (q < 3)
                {
                    newRepetition = 0;
                    newInterval = 1;
                    newLapses++;
                }
                else
                {
                    newRepetition = oldRepetition + 1;

                    if (newRepetition == 1)
                        newInterval = 1;
                    else if (newRepetition == 2)
                        newInterval = 6;
                    else
                        newInterval = (int)Math.Round(oldInterval * oldEf);
                }

                // EF calculation
                var newEf =oldEf + (0.1m - (5 - q) * (0.08m + (5 - q) * 0.02m));
                if (newEf < 1.3m) newEf = 1.3m;

                // 4. Update state
                state.Repetition = newRepetition;
                state.IntervalDays = newInterval;
                state.EaseFactor = Math.Round(newEf, 2);
                state.DueDate = DateOnly.FromDateTime(now.AddDays(newInterval));
                state.Lapses = newLapses;
                state.LastReviewedAt = now;
                state.UpdatedAt = now;

                _reviewStateRepository.Update(state);

                // 5. Create review record
                var record = new ReviewRecord
                {
                    Id = Guid.NewGuid(),
                    ReviewSessionId = request.SessionId,
                    UserId = userId,
                    CardId = request.CardId,
                    Rating = request.Rating,
                    AnswerText = request.AnswerText,
                    ResponseTimeMs = request.ResponseTimeMs,
                    IsCorrect = isCorrect,

                    PreviousEaseFactor = oldEf,
                    NewEaseFactor = state.EaseFactor,
                    PreviousIntervalDays = oldInterval,
                    NewIntervalDays = newInterval,

                    ReviewedAt = now
                };

                _reviewRecordRepository.Add(record);

                // 6. Update session stats
                session.TotalCards += 1;
                if (isCorrect)
                    session.CorrectCount += 1;

                _reviewSessionRepository.Update(session);

                // 7. Save all
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return ResponseResult.Success("Review submitted successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
           
        }
    }
}
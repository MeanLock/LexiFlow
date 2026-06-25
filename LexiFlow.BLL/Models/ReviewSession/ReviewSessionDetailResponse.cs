using LexiFlow.BLL.Models.ReviewRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.ReviewSession
{
    public class ReviewSessionDetailResponse
    {
        public Guid Id { get; set; }

        public Guid DeckId { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int TotalCards { get; set; }

        public int CorrectCount { get; set; }

        public List<ReviewRecordResponse> Records { get; set; } = new();
    }
}

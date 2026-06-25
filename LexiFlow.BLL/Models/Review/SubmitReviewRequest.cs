using LexiFlow.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Review
{
    public class SubmitReviewRequest
    {
        public Guid SessionId { get; set; }

        public Guid CardId { get; set; }

        public ReviewRating Rating { get; set; }

        public string? AnswerText { get; set; }

        public int? ResponseTimeMs { get; set; }
    }
}

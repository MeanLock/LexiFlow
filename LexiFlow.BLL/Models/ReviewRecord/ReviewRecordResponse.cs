using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.ReviewRecord
{
    public class ReviewRecordResponse
    {
        public Guid CardId { get; set; }

        public int Rating { get; set; }

        public bool IsCorrect { get; set; }

        public int? ResponseTimeMs { get; set; }

        public DateTime ReviewedAt { get; set; }
    }
}

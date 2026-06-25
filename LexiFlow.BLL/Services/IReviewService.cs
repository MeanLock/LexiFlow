using LexiFlow.BLL.Models.Review;
using LexiFlow.BLL.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public interface IReviewService
    {
        Task<ResponseResult> SubmitReviewAsync(Guid userId, SubmitReviewRequest request);
    }
}

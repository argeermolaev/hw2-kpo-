using Pos.Domain.Entities;
using Pos.Domain.ValueObjects;

namespace Pos.Application
{
    public interface IEvaluationFactory
    {
        Evaluation Create(Guid studentId, Criterion criterion, decimal scoreValue, string? comment);
    }

    public class EvaluationFactory : IEvaluationFactory
    {
        public Evaluation Create(Guid studentId, Criterion criterion, decimal scoreValue, string? comment)
        {
            var score = Score.Create(scoreValue, criterion.MaxScore);
            return new Evaluation(Guid.NewGuid(), studentId, criterion.Id, score, DateTime.UtcNow, comment);
        }
    }
}
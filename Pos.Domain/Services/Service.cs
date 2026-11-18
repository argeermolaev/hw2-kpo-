using Pos.Domain.Entities;

namespace Pos.Domain.Services
{
    public interface IGradeCalculationStrategy
    {
        decimal Calculate(IEnumerable<Evaluation> evaluations, IEnumerable<Criterion> criteria);
    }
    public class GradeCalculator
    {
        private readonly IGradeCalculationStrategy _strategy;

        public GradeCalculator(IGradeCalculationStrategy strategy)
        {
            _strategy = strategy;
        }

        public decimal CalculateFinalGrade(Guid studentId, IEnumerable<Evaluation> allEvaluations, IEnumerable<Criterion> allCriteria)
        {
            var studentEvaluations = allEvaluations.Where(e => e.StudentId == studentId).ToList();

            if (!studentEvaluations.Any()) return 0m;

            return _strategy.Calculate(studentEvaluations, allCriteria);
        }
    }
    public class WeightedSumStrategy : IGradeCalculationStrategy
    {
        public decimal Calculate(IEnumerable<Evaluation> evaluations, IEnumerable<Criterion> criteria)
        {
            decimal total = 0;
            foreach (var eval in evaluations)
            {
                var criterion = criteria.FirstOrDefault(c => c.Id == eval.CriterionId);
                if (criterion != null)
                {
                    total += (eval.Score.Value / criterion.MaxScore) * criterion.Weight.Value;
                }
            }
            return Math.Round(total * 100, 2);
        }
    }
}
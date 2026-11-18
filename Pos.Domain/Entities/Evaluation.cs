using Pos.Domain.ValueObjects;

namespace Pos.Domain.Entities
{
    public class Evaluation
    {
        public Guid Id { get; private set; }
        public Guid StudentId { get; private set; }
        public Guid CriterionId { get; private set; }
        public Score Score { get; private set; }
        public DateTime Date { get; private set; }
        public string? Comment { get; private set; }

        private Evaluation() { }

        public Evaluation(Guid id, Guid studentId, Guid criterionId, Score score, DateTime date, string? comment)
        {
            Id = id;
            StudentId = studentId;
            CriterionId = criterionId;
            Score = score;
            Date = date;
            Comment = comment;
        }

        public void UpdateScore(Score newScore, string? newComment)
        {
            Score = newScore;
            Comment = newComment;
            Date = DateTime.UtcNow;
        }
    }
}
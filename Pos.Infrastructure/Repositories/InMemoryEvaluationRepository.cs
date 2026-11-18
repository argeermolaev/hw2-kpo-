using Pos.Domain.Entities;
using Pos.Domain.Repositories;

namespace Pos.Infrastructure.Repositories
{
    public class InMemoryEvaluationRepository : IEvaluationRepository
    {
        private readonly List<Evaluation> _evaluations = new();

        public Evaluation? GetById(Guid id) => _evaluations.FirstOrDefault(e => e.Id == id);
        public IEnumerable<Evaluation> GetByStudentId(Guid studentId) => _evaluations.Where(e => e.StudentId == studentId);
        public void Add(Evaluation evaluation) => _evaluations.Add(evaluation);

        public void Update(Evaluation evaluation)
        {
            var index = _evaluations.FindIndex(e => e.Id == evaluation.Id);
            if (index != -1) _evaluations[index] = evaluation;
        }

        public void Delete(Guid id) => _evaluations.RemoveAll(e => e.Id == id);
    }
}
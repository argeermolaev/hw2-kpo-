using Pos.Domain.Entities;
using Pos.Domain.Repositories;

namespace Pos.Infrastructure.Repositories
{
    public class InMemoryCriterionRepository : ICriterionRepository
    {
        private readonly List<Criterion> _criteria = new();

        public Criterion? GetById(Guid id) => _criteria.FirstOrDefault(c => c.Id == id);
        public IEnumerable<Criterion> GetAll() => _criteria;
        public void Add(Criterion criterion) => _criteria.Add(criterion);
    }
}
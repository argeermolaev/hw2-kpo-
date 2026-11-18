using Pos.Domain.Entities;
using Pos.Domain.Repositories;
using Pos.Domain.ValueObjects;

namespace Pos.Application
{
    public class CriterionService
    {
        private readonly ICriterionRepository _criterionRepo;

        public CriterionService(ICriterionRepository criterionRepo)
        {
            _criterionRepo = criterionRepo;
        }

        public void AddCriterion(string name, decimal maxScore, decimal weightValue)
        {
            var weight = Weight.Create(weightValue);
            var criterion = new Criterion(Guid.NewGuid(), name, maxScore, weight);
            _criterionRepo.Add(criterion);
        }

        public IEnumerable<Criterion> GetAllCriteria()
        {
            return _criterionRepo.GetAll();
        }
    }
}
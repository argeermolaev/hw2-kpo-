using Pos.Domain.Entities;

namespace Pos.Domain.Repositories
{
    public interface IStudentRepository
    {
        Student? GetById(Guid id);
        IEnumerable<Student> GetAll();
        void Add(Student student);
        void Update(Student student);
        void Delete(Guid id);
    }

    public interface ICriterionRepository
    {
        Criterion? GetById(Guid id);
        IEnumerable<Criterion> GetAll();
        void Add(Criterion criterion);
    }

    public interface IEvaluationRepository
    {
        Evaluation? GetById(Guid id);
        IEnumerable<Evaluation> GetByStudentId(Guid studentId);
        void Add(Evaluation evaluation);
        void Update(Evaluation evaluation);
        void Delete(Guid id);
    }
}
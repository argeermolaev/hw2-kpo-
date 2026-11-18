using Pos.Domain.Entities;
using Pos.Domain.Repositories;

namespace Pos.Infrastructure.Repositories
{
    public class InMemoryStudentRepository : IStudentRepository
    {
        private readonly List<Student> _students = new();

        public Student? GetById(Guid id) => _students.FirstOrDefault(s => s.Id == id);
        public IEnumerable<Student> GetAll() => _students;
        public void Add(Student student) => _students.Add(student);

        public void Update(Student student)
        {
            var index = _students.FindIndex(s => s.Id == student.Id);
            if (index != -1) _students[index] = student;
        }

        public void Delete(Guid id) => _students.RemoveAll(s => s.Id == id);
    }
}
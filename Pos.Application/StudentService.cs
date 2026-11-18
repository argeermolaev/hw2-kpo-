using Pos.Domain.Entities;
using Pos.Domain.Repositories;

namespace Pos.Application
{
    public class StudentService
    {
        private readonly IStudentRepository _studentRepo;

        public StudentService(IStudentRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }

        public void AddStudent(string fullName)
        {
            var student = new Student(Guid.NewGuid(), fullName);
            _studentRepo.Add(student);
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _studentRepo.GetAll();
        }
    }
}
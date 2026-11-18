namespace Pos.Domain.Entities
{
    public class Student
    {
        public Guid Id { get; private set; }
        public string FullName { get; private set; }
        private Student() { }

        public Student(Guid id, string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Имя студента не может быть пустым.");

            Id = id;
            FullName = fullName;
        }

        public void Rename(string newFullName)
        {
            if (string.IsNullOrWhiteSpace(newFullName))
                throw new ArgumentException("Новое имя не может быть пустым.");

            FullName = newFullName;
        }
    }
}
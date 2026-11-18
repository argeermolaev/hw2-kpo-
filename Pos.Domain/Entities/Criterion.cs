using Pos.Domain.ValueObjects;

namespace Pos.Domain.Entities
{
    public class Criterion
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal MaxScore { get; private set; }
        public Weight Weight { get; private set; }

        private Criterion() { }

        public Criterion(Guid id, string name, decimal maxScore, Weight weight)
        {
            if (maxScore <= 0) throw new ArgumentException("Максимальный балл должен быть больше нуля.");

            Id = id;
            Name = name;
            MaxScore = maxScore;
            Weight = weight;
        }
    }
}
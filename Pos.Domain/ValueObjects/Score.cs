namespace Pos.Domain.ValueObjects
{
    public record Score
    {
        public decimal Value { get; }

        private Score(decimal value) => Value = value;
        public static Score Create(decimal value, decimal maxScore)
        {
            if (value < 0 || value > maxScore)
                throw new ArgumentException($"Балл должен быть в диапазоне от 0 до {maxScore}. Передано: {value}");
            return new Score(value);
        }
    }
}
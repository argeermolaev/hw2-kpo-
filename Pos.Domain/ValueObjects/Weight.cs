namespace Pos.Domain.ValueObjects
{
    public record Weight
    {
        public decimal Value { get; }

        private Weight(decimal value) => Value = value;

        public static Weight Create(decimal value)
        {
            if (value < 0 || value > 1)
                throw new ArgumentException("Вес критерия должен быть в диапазоне от 0 до 1.");

            return new Weight(value);
        }
    }
}
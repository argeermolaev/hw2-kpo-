using Pos.Domain.Entities;
using Pos.Domain.Services;
using Pos.Domain.ValueObjects;
using Xunit;

namespace Pos.Tests.DomainTests
{
    public class GradeCalculatorTests
    {
        [Fact]
        public void CalculateFinalGrade_NoEvaluations_ReturnsZero()
        {
            // Arrange
            var calculator = new GradeCalculator(new WeightedSumStrategy());
            var studentId = Guid.NewGuid();

            // Act
            var result = calculator.CalculateFinalGrade(studentId, new List<Evaluation>(), new List<Criterion>());

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void CalculateFinalGrade_ValidEvaluations_ReturnsCorrectWeightedSum()
        {
            // Arrange
            var calculator = new GradeCalculator(new WeightedSumStrategy());
            var studentId = Guid.NewGuid();

            var criterion1 = new Criterion(Guid.NewGuid(), "Лаба", 10m, Weight.Create(0.3m));
            var criterion2 = new Criterion(Guid.NewGuid(), "Экзамен", 100m, Weight.Create(0.7m));
            var criteria = new List<Criterion> { criterion1, criterion2 };

            var eval1 = new Evaluation(Guid.NewGuid(), studentId, criterion1.Id, Score.Create(9m, 10m), DateTime.UtcNow, null);
            var eval2 = new Evaluation(Guid.NewGuid(), studentId, criterion2.Id, Score.Create(80m, 100m), DateTime.UtcNow, null);
            var evaluations = new List<Evaluation> { eval1, eval2 };

            // Act
            // Ожидаемый расчет: (9/10)*0.3 + (80/100)*0.7 = 0.27 + 0.56 = 0.83. Умножаем на 100 = 83.
            var result = calculator.CalculateFinalGrade(studentId, evaluations, criteria);

            // Assert
            Assert.Equal(83m, result);
        }
    }
}
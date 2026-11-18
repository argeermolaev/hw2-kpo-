using Pos.Domain.ValueObjects;
using Xunit;

namespace Pos.Tests.DomainTests
{
    public class ScoreTests
    {
        [Fact]
        public void Create_ValidScore_ReturnsScoreObject()
        {
            // Arrange
            decimal value = 8.5m;
            decimal maxScore = 10m;

            // Act
            var score = Score.Create(value, maxScore);

            // Assert
            Assert.Equal(value, score.Value);
        }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(11, 10)]
        public void Create_InvalidScore_ThrowsArgumentException(decimal value, decimal maxScore)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Score.Create(value, maxScore));
        }
    }
}
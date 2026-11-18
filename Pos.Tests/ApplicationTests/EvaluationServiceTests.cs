using Moq;
using Pos.Application;
using Pos.Domain.Entities;
using Pos.Domain.Repositories;
using Pos.Domain.Services;
using Xunit;

namespace Pos.Tests.ApplicationTests
{
    public class EvaluationServiceTests
    {
        [Fact]
        public void AddEvaluation_StudentNotFound_ThrowsException()
        {
            // Arrange
            var studentRepoMock = new Mock<IStudentRepository>();
            var criterionRepoMock = new Mock<ICriterionRepository>();
            var evaluationRepoMock = new Mock<IEvaluationRepository>();
            var factoryMock = new Mock<IEvaluationFactory>();
            var calculator = new GradeCalculator(new WeightedSumStrategy());

            // Настраиваем мок: GetById возвращает null (студент не найден)
            studentRepoMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns((Student?)null);

            var service = new EvaluationService(
                studentRepoMock.Object, criterionRepoMock.Object,
                evaluationRepoMock.Object, factoryMock.Object, calculator);

            // Act & Assert
            Assert.Throws<Exception>(() => service.AddEvaluation(Guid.NewGuid(), Guid.NewGuid(), 5, "test"));
        }
    }
}
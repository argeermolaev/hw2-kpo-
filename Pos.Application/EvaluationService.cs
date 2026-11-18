using Pos.Domain.Entities;
using Pos.Domain.Repositories;
using Pos.Domain.Services;
using Pos.Domain.ValueObjects;
using System.Collections;

namespace Pos.Application
{
    public class EvaluationService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly ICriterionRepository _criterionRepo;
        private readonly IEvaluationRepository _evaluationRepo;
        private readonly IEvaluationFactory _evaluationFactory;
        private readonly GradeCalculator _gradeCalculator;

        public EvaluationService(
            IStudentRepository studentRepo,
            ICriterionRepository criterionRepo,
            IEvaluationRepository evaluationRepo,
            IEvaluationFactory evaluationFactory,
            GradeCalculator gradeCalculator)
        {
            _studentRepo = studentRepo;
            _criterionRepo = criterionRepo;
            _evaluationRepo = evaluationRepo;
            _evaluationFactory = evaluationFactory;
            _gradeCalculator = gradeCalculator;
        }

        public void AddEvaluation(Guid studentId, Guid criterionId, decimal scoreValue, string? comment)
        {
            _ = _studentRepo.GetById(studentId) ?? throw new Exception("Студент не найден");
            var criterion = _criterionRepo.GetById(criterionId) ?? throw new Exception("Критерий не найден");
            var evaluation = _evaluationFactory.Create(studentId, criterion, scoreValue, comment);
            _evaluationRepo.Add(evaluation);
        }

        public void UpdateEvaluation(Guid evaluationId, decimal newScoreValue, string? newComment)
        {
            var evaluation = _evaluationRepo.GetById(evaluationId) ?? throw new Exception("Оценка не найдена");
            var criterion = _criterionRepo.GetById(evaluation.CriterionId) ?? throw new Exception("Критерий не найден");
            var newScore = Score.Create(newScoreValue, criterion.MaxScore);

            evaluation.UpdateScore(newScore, newComment);
            _evaluationRepo.Update(evaluation);
        }

        public void DeleteEvaluation(Guid evaluationId)
        {
            _evaluationRepo.Delete(evaluationId);
        }
        public IEnumerable<Evaluation> GetStudentEvaluations(Guid studentId)
        {
            _ = _studentRepo.GetById(studentId) ?? throw new Exception("Студент не найден");
            return _evaluationRepo.GetByStudentId(studentId);
        }
        public decimal CalculateFinalGrade(Guid studentId)
        {
            _ = _studentRepo.GetById(studentId) ?? throw new Exception("Студент не найден");
            var evaluations = _evaluationRepo.GetByStudentId(studentId);
            var criteria = _criterionRepo.GetAll();

            return _gradeCalculator.CalculateFinalGrade(studentId, evaluations, criteria);
        }
    }
}
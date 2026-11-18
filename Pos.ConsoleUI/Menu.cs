using Pos.Application;
using Pos.Domain.Entities;

namespace Pos.ConsoleUI
{
    public class Menu
    {
        private readonly StudentService _studentService;
        private readonly CriterionService _criterionService;
        private readonly EvaluationService _evaluationService;

        public Menu(StudentService studentService, CriterionService criterionService, EvaluationService evaluationService)
        {
            _studentService = studentService;
            _criterionService = criterionService;
            _evaluationService = evaluationService;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА ОЦЕНКИ СТУДЕНТОВ (ПОС) ===");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Добавить критерий");
                Console.WriteLine("3. Выставить оценку");
                Console.WriteLine("4. Показать всех студентов");
                Console.WriteLine("5. Показать все критерии");
                Console.WriteLine("6. Показать оценки студента");
                Console.WriteLine("7. Рассчитать итоговый балл студента");
                Console.WriteLine("8. Изменить оценку");
                Console.WriteLine("9. Удалить оценку");
                Console.WriteLine("10. Запустить сквозной сценарий (Демо)");
                Console.WriteLine("0. Выход");
                Console.Write("\nВыберите действие: ");

                switch (Console.ReadLine())
                {
                    case "1": AddStudent(); break;
                    case "2": AddCriterion(); break;
                    case "3": AddEvaluation(); break;
                    case "4": ShowStudents(); break;
                    case "5": ShowCriteria(); break;
                    case "6": ShowEvaluations(); break;
                    case "7": CalculateFinalGrade(); break;
                    case "8": UpdateEvaluation(); break;
                    case "9": DeleteEvaluation(); break;
                    case "10": RunDemoScenario(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Неверный ввод. Нажмите Enter...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private void AddStudent()
        {
            Console.Write("Введите ФИО студента: ");
            var name = Console.ReadLine();
            try
            {
                _studentService.AddStudent(name!);
                Console.WriteLine("Студент успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Pause();
        }

        private void AddCriterion()
        {
            Console.Write("Название критерия: ");
            var name = Console.ReadLine();
            Console.Write("Максимальный балл: ");
            if (!decimal.TryParse(Console.ReadLine(), out var maxScore)) { Console.WriteLine("Ошибка ввода"); Pause(); return; }
            Console.Write("Вес (от 0 до 1): ");
            if (!decimal.TryParse(Console.ReadLine(), out var weight)) { Console.WriteLine("Ошибка ввода"); Pause(); return; }

            try
            {
                _criterionService.AddCriterion(name!, maxScore, weight);
                Console.WriteLine("Критерий успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Pause();
        }

        private void AddEvaluation()
        {
            var students = _studentService.GetAllStudents().ToList();
            var criteria = _criterionService.GetAllCriteria().ToList();

            if (!students.Any() || !criteria.Any())
            {
                Console.WriteLine("Сначала добавьте хотя бы одного студента и один критерий.");
                Pause(); return;
            }

            Console.WriteLine("Выберите студента:");
            for (int i = 0; i < students.Count; i++) Console.WriteLine($"{i + 1}. {students[i].FullName}");

            if (!int.TryParse(Console.ReadLine(), out var sIndex) || sIndex < 1 || sIndex > students.Count) { Console.WriteLine("Неверный выбор"); Pause(); return; }

            Console.WriteLine("Выберите критерий:");
            for (int i = 0; i < criteria.Count; i++) Console.WriteLine($"{i + 1}. {criteria[i].Name} (Макс: {criteria[i].MaxScore})");

            if (!int.TryParse(Console.ReadLine(), out var cIndex) || cIndex < 1 || cIndex > criteria.Count) { Console.WriteLine("Неверный выбор"); Pause(); return; }

            Console.Write("Введите балл: ");
            if (!decimal.TryParse(Console.ReadLine(), out var scoreValue)) { Console.WriteLine("Ошибка ввода"); Pause(); return; }
            Console.Write("Комментарий (необязательно): ");
            var comment = Console.ReadLine();

            try
            {
                _evaluationService.AddEvaluation(students[sIndex - 1].Id, criteria[cIndex - 1].Id, scoreValue, comment);
                Console.WriteLine("Оценка выставлена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Pause();
        }

        private void ShowStudents()
        {
            var students = _studentService.GetAllStudents().ToList();
            if (!students.Any()) Console.WriteLine("Список пуст.");
            else foreach (var s in students) Console.WriteLine($"- {s.FullName} (ID: {s.Id})");
            Pause();
        }

        private void ShowCriteria()
        {
            var criteria = _criterionService.GetAllCriteria().ToList();
            if (!criteria.Any()) Console.WriteLine("Список пуст.");
            else foreach (var c in criteria) Console.WriteLine($"- {c.Name} | Макс: {c.MaxScore} | Вес: {c.Weight.Value}");
            Pause();
        }

        private void ShowEvaluations()
        {
            var students = _studentService.GetAllStudents().ToList();
            if (!students.Any()) { Console.WriteLine("Нет студентов."); Pause(); return; }

            Console.WriteLine("Выберите студента:");
            for (int i = 0; i < students.Count; i++) Console.WriteLine($"{i + 1}. {students[i].FullName}");
            if (!int.TryParse(Console.ReadLine(), out var sIndex) || sIndex < 1 || sIndex > students.Count) { Console.WriteLine("Неверный выбор"); Pause(); return; }

            var studentId = students[sIndex - 1].Id;
            var evaluations = _evaluationService.GetStudentEvaluations(studentId);
            var criteria = _criterionService.GetAllCriteria().ToList();

            if (!evaluations.Any()) Console.WriteLine("У студента нет оценок.");
            else
            {
                foreach (var e in evaluations)
                {
                    var critName = criteria.FirstOrDefault(c => c.Id == e.CriterionId)?.Name ?? "Неизвестно";
                    Console.WriteLine($"- Критерий: {critName} | Балл: {e.Score.Value} | Комментарий: {e.Comment ?? "нет"}");
                }
            }
            Pause();
        }

        private void CalculateFinalGrade()
        {
            var students = _studentService.GetAllStudents().ToList();
            if (!students.Any()) { Console.WriteLine("Нет студентов."); Pause(); return; }

            Console.WriteLine("Выберите студента:");
            for (int i = 0; i < students.Count; i++) Console.WriteLine($"{i + 1}. {students[i].FullName}");
            if (!int.TryParse(Console.ReadLine(), out var sIndex) || sIndex < 1 || sIndex > students.Count) { Console.WriteLine("Неверный выбор"); Pause(); return; }

            try
            {
                var result = _evaluationService.CalculateFinalGrade(students[sIndex - 1].Id);
                Console.WriteLine($"\nИтоговый балл: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Pause();
        }
        private void UpdateEvaluation()
        {
            var students = _studentService.GetAllStudents().ToList();
            if (!students.Any()) { Console.WriteLine("Нет студентов."); Pause(); return; }

            Console.WriteLine("Выберите студента:");
            for (int i = 0; i < students.Count; i++) Console.WriteLine($"{i + 1}. {students[i].FullName}");
            if (!int.TryParse(Console.ReadLine(), out var sIndex) || sIndex < 1 || sIndex > students.Count)
            { Console.WriteLine("Неверный выбор"); Pause(); return; }

            var student = students[sIndex - 1];

            var evaluations = _evaluationService.GetStudentEvaluations(student.Id).ToList();
            if (!evaluations.Any()) { Console.WriteLine("У студента нет оценок."); Pause(); return; }

            var criteria = _criterionService.GetAllCriteria().ToList();
            Console.WriteLine("Выберите оценку для изменения:");
            for (int i = 0; i < evaluations.Count; i++)
            {
                var critName = criteria.FirstOrDefault(c => c.Id == evaluations[i].CriterionId)?.Name ?? "Неизвестно";
                Console.WriteLine($"{i + 1}. Критерий: {critName} | Текущий балл: {evaluations[i].Score.Value} (Макс: {criteria.First(c => c.Id == evaluations[i].CriterionId).MaxScore})");
            }

            if (!int.TryParse(Console.ReadLine(), out var eIndex) || eIndex < 1 || eIndex > evaluations.Count)
            { Console.WriteLine("Неверный выбор"); Pause(); return; }

            var evaluationToUpdate = evaluations[eIndex - 1];

            Console.Write("Введите новый балл: ");
            if (!decimal.TryParse(Console.ReadLine(), out var newScore))
            { Console.WriteLine("Ошибка ввода"); Pause(); return; }

            Console.Write("Введите новый комментарий (или оставьте пустым, чтобы удалить старый): ");
            var newComment = Console.ReadLine();

            try
            {
                _evaluationService.UpdateEvaluation(evaluationToUpdate.Id, newScore, string.IsNullOrWhiteSpace(newComment) ? null : newComment);
                Console.WriteLine("Оценка успешно изменена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Pause();
        }
        private void DeleteEvaluation()
        {
            var students = _studentService.GetAllStudents().ToList();
            if (!students.Any()) { Console.WriteLine("Нет студентов."); Pause(); return; }

            Console.WriteLine("Выберите студента:");
            for (int i = 0; i < students.Count; i++) Console.WriteLine($"{i + 1}. {students[i].FullName}");
            if (!int.TryParse(Console.ReadLine(), out var sIndex) || sIndex < 1 || sIndex > students.Count)
            { Console.WriteLine("Неверный выбор"); Pause(); return; }

            var student = students[sIndex - 1];

            var evaluations = _evaluationService.GetStudentEvaluations(student.Id).ToList();
            if (!evaluations.Any()) { Console.WriteLine("У студента нет оценок."); Pause(); return; }

            var criteria = _criterionService.GetAllCriteria().ToList();
            Console.WriteLine("Выберите оценку для удаления:");
            for (int i = 0; i < evaluations.Count; i++)
            {
                var critName = criteria.FirstOrDefault(c => c.Id == evaluations[i].CriterionId)?.Name ?? "Неизвестно";
                Console.WriteLine($"{i + 1}. Критерий: {critName} | Балл: {evaluations[i].Score.Value}");
            }

            if (!int.TryParse(Console.ReadLine(), out var eIndex) || eIndex < 1 || eIndex > evaluations.Count)
            { Console.WriteLine("Неверный выбор"); Pause(); return; }

            var evaluationToDelete = evaluations[eIndex - 1];
            Console.Write($"Вы уверены, что хотите удалить оценку? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y")
            {
                Console.WriteLine("Удаление отменено.");
                Pause();
                return;
            }
            try
            {
                _evaluationService.DeleteEvaluation(evaluationToDelete.Id);
                Console.WriteLine("Оценка успешно удалена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Pause();
        }
        private void Pause()
        {
            Console.WriteLine("\nНажмите Enter для продолжения...");
            Console.ReadLine();
        }
        private void RunDemoScenario()
        {
            Console.Clear();
            Console.WriteLine("=== ЗАПУСК СКВОЗНОГО СЦЕНАРИЯ ===\n");

            // 1. Заводим студента и критерии
            _studentService.AddStudent("Иван Иванов");
            _criterionService.AddCriterion("Лабораторная", 10m, 0.3m);
            _criterionService.AddCriterion("Экзамен", 100m, 0.7m);

            var student = _studentService.GetAllStudents().First();
            var criteria = _criterionService.GetAllCriteria().ToList();

            Console.WriteLine($"Добавлен студент: {student.FullName}");
            Console.WriteLine("Добавлены критерии: Лабораторная (макс 10, вес 0.3), Экзамен (макс 100, вес 0.7)\n");

            // 2. Выставляем оценки
            _evaluationService.AddEvaluation(student.Id, criteria[0].Id, 9m, "Хорошо");
            _evaluationService.AddEvaluation(student.Id, criteria[1].Id, 85m, "Отлично");
            Console.WriteLine("Выставлены оценки: Лабораторная - 9, Экзамен - 85\n");

            // 3. Считаем итог
            var finalGrade = _evaluationService.CalculateFinalGrade(student.Id);
            Console.WriteLine($"Рассчитан итоговый балл: {finalGrade}");

            // 4. Экспорт (если нужно) - можно просто вывести в консоль красивую табличку
            Console.WriteLine("\n--- Экспорт ведомости ---");
            Console.WriteLine($"Студент: {student.FullName} | Итог: {finalGrade}");
            Console.WriteLine("-------------------------");

            Pause();
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Pos.Application;
using Pos.Domain.Repositories;
using Pos.Domain.Services;
using Pos.Infrastructure.Repositories;
using Pos.ConsoleUI;

namespace Pos.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            // Регистрация зависимостей (как у вас было)
            services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
            services.AddSingleton<ICriterionRepository, InMemoryCriterionRepository>();
            services.AddSingleton<IEvaluationRepository, InMemoryEvaluationRepository>();
            services.AddTransient<IGradeCalculationStrategy, WeightedSumStrategy>();
            services.AddTransient<GradeCalculator>();
            services.AddTransient<IEvaluationFactory, EvaluationFactory>();
            services.AddTransient<StudentService>();
            services.AddTransient<CriterionService>();
            services.AddTransient<EvaluationService>();

            // Регистрация самого меню
            services.AddTransient<Menu>();

            var serviceProvider = services.BuildServiceProvider();

            // Запуск
            var menu = serviceProvider.GetRequiredService<Menu>();
            menu.Run();
        }
    }
}
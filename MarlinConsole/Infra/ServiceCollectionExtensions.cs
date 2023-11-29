using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MarlinConsole.Infra
{
    public static class ServiceCollectionExtensions
    {
        private static void As(this IServiceCollection _, Type t, object[] marks, Action<Type> asRegister)
        {
            for (var i = 0; i < marks.Length; i++)
            {
                if (marks[i] is Register r && r == Register.As)
                {
                    i++;
                    asRegister((Type)marks[i]);
                }
            }
        }

        private static readonly Dictionary<Register, Action<IServiceCollection, Type, object[]>> Registrars = new()
        {
            [Register.Singleton] = (s, t, m) => s.AddSingleton(t).As(t, m, (ti) => s.AddSingleton(ti, t)),
            [Register.Transient] = (s, t, m) => s.AddTransient(t).As(t, m, (ti) => s.AddTransient(ti, t)),
            [Register.Scoped] = (s, t, m) => s.AddScoped(t).As(t, m, (ti) => s.AddScoped(ti, t)),
        };

        public static IServiceCollection AddMarkedFrom(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly
                .GetTypes()
                .Select(s => new { s, Mark = s.GetCustomAttribute<MarkAttribute>() })
                .Where(s => s.Mark != null && s.Mark.Marks.Length > 0 && s.Mark.Marks[0].GetType() == typeof(Register))
                .Select(s => new { Type = s.s, Register = (Register)s.Mark!.Marks[0], s.Mark!.Marks });

            foreach (var t in types)
                Registrars[t.Register](services, t.Type, t.Marks);

            return services;
        }
    }
}

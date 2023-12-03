using System.Reflection;
using Marks.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Marks.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private static void AsType(this IServiceCollection _, Type t, object[] marks, Action<Type> asRegister)
        {
            for (var i = 0; i < marks.Length; i++)
            {
                if (marks[i] is As r && r == As.Type)
                {
                    i++;
                    asRegister((Type)marks[i]);
                }
            }
        }

        private static readonly Dictionary<Injects, Action<IServiceCollection, Type, object[]>> Registrars = new()
        {
            [Injects.Singleton] = (s, t, m) => s.AddSingleton(t).AsType(t, m, (ti) => s.AddSingleton(ti, t)),
            [Injects.Transient] = (s, t, m) => s.AddTransient(t).AsType(t, m, (ti) => s.AddTransient(ti, t)),
            [Injects.Scoped] = (s, t, m) => s.AddScoped(t).AsType(t, m, (ti) => s.AddScoped(ti, t)),
        };

        public static IServiceCollection AddMarkedFrom(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly
                .GetTypes()
                .Select(s => new { s, Mark = s.GetCustomAttribute<MarkAttribute>() })
                .Where(s => s.Mark != null && s.Mark.Marks.Length > 1 && s.Mark.Marks[0].GetType() == typeof(By))
                .Select(s => new { Type = s.s, Register = (Injects)s.Mark!.Marks[1], s.Mark!.Marks });

            foreach (var t in types)
                Registrars[t.Register](services, t.Type, t.Marks);

            return services;
        }
    }
}

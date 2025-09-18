using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PipelineExecutor.ActionExecutor;

namespace PipelineExecutor;

/// <summary>
/// Extension methods for registering pipeline executor actions in the DI container.
/// </summary>
public static class PipelineExecutorServiceExtension
{
    /// <summary>
    /// Registers all non-abstract classes implementing <see cref="IPipelineExecutorAction{T}"/> found in the given assembly
    /// as keyed scoped services in the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add registrations to.</param>
    /// <param name="assembly">The assembly to scan for pipeline action implementations.</param>
    public static void RegisterPipelineActions(this IServiceCollection services, Assembly assembly)
    {
        // Find all non-abstract classes implementing IPipelineExecutorAction<>
        var pipelineTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineExecutorAction<>)));
        
        // Register each implementation as a keyed scoped service
        foreach (var pipelineType in pipelineTypes)
        {
            foreach (var iface in pipelineType.GetInterfaces()
                         .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineExecutorAction<>)))
            {
                // Uses the full type name as the key for registration
                services.AddKeyedScoped(iface, pipelineType.FullName, pipelineType);
            }
        }
    }
}
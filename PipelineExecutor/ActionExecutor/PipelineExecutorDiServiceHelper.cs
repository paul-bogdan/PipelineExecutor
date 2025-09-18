using Microsoft.Extensions.DependencyInjection;
using PipelineExecutor.Models;

namespace PipelineExecutor.ActionExecutor;

/// <summary>
/// Helper for resolving and configuring pipeline actions from DI based on configuration contexts.
/// </summary>
/// <typeparam name="TCommand">The type of the command object.</typeparam>
/// <typeparam name="TResponse">The type of the response object.</typeparam>
public class PipelineExecutorDiServiceHelper<TCommand, TResponse>
    where TCommand : class
    where TResponse : class
{
    /// <summary>
    /// Resolves and configures pipeline actions from the service provider using the provided action configuration contexts.
    /// </summary>
    /// <param name="service">The service provider for resolving actions.</param>
    /// <param name="actionConfigContexts">The list of action configuration contexts.</param>
    /// <returns>
    /// A list of configured pipeline actions, or null if no contexts are provided.
    /// </returns>
    public List<PipelineActionConfigured<IPipelineExecutionCommandObject<TCommand, TResponse>>>? GetActions(
        IServiceProvider service,
        List<PipelineExecutorActionConfigContext>? actionConfigContexts)
    {
        if (actionConfigContexts == null || actionConfigContexts.Count == 0)
            return null;

        var actions = new List<PipelineActionConfigured<IPipelineExecutionCommandObject<TCommand, TResponse>>>();

        foreach (var actionConfig in actionConfigContexts)
        {
            var action = service.GetRequiredKeyedService<IPipelineExecutorAction<IPipelineExecutionCommandObject<TCommand, TResponse>>>(actionConfig.ActionName);
            actions.Add(new PipelineActionConfigured<IPipelineExecutionCommandObject<TCommand, TResponse>>
            {
                Action = action,
                ActionName = actionConfig.ActionName,
                Position = actionConfig.Position,
                HasCompensate = actionConfig.HasCompensate,
                ExecuteAsyncInParallel = actionConfig.ExecuteInParallel,
                Resilient = new PipelineActionConfiguredResilient
                {
                    IsResilient = actionConfig.ResilientConfig.IsResilient,
                    RetryCount = actionConfig.ResilientConfig.RetryCount,
                    RetryDelayInMilliseconds = actionConfig.ResilientConfig.RetryIntervalInMs,
                    CompensateBeforeRetry = actionConfig.ResilientConfig.CompensateBeforeRetry
                }
            });
        }

        return actions;
    }
}

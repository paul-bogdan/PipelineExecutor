using PipelineExecutor.ActionExecutor;
using PipelineExecutor.InMemoryPipeline;
using PipelineExecutor.Models;

namespace PipelineExecutor;

/// <summary>
/// Abstract base class for executing a pipeline of actions.
/// Supports initialization with either a list of actions or via dependency injection.
/// </summary>
/// <typeparam name="TCommand">The type of the command object.</typeparam>
/// <typeparam name="TResponse">The type of the response object.</typeparam>
public abstract class BasePipeline<TCommand, TResponse> : IBasePipeline<TCommand, TResponse>
    where TCommand : class
    where TResponse : class
{
    // The pipeline executor instance, responsible for executing actions.
    private IBasePipelineExecutor<TCommand, TResponse>? _executor;

    protected BasePipeline()
    {
        
    }
    

    protected void SetConfig(IPipelineExecutorConfig context, IServiceProvider serviceProvider)
    {
        
        ArgumentNullException.ThrowIfNull(context);
        if (context.UseInMemoryPipelineActions)
        {
            var pipelineActionDiHelper = new PipelineExecutorDiServiceHelper<TCommand, TResponse>();
            _executor = new InMemoryPipelineExecutor<TCommand, TResponse>(
                pipelineActionDiHelper.GetActions(serviceProvider, context.ActionConfigContexts));
        }
    }
    
    /// <summary>
    /// Initializes the pipeline with a list of configured actions.
    /// </summary>
    /// <param name="context">The pipeline execution context.</param>
    /// <param name="actions">The list of configured pipeline actions.</param>
    protected BasePipeline(
        IPipelineExecutorConfig context,
        List<PipelineActionConfigured<IPipelineExecutionCommandObject<TCommand, TResponse>>>? actions)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(actions);

        if (context.UseInMemoryPipelineActions)
        {
            _executor = new InMemoryPipelineExecutor<TCommand, TResponse>(actions);
        }
    }

    /// <summary>
    /// Initializes the pipeline using dependency injection to resolve actions.
    /// </summary>
    /// <param name="context">The pipeline execution context.</param>
    /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
    protected BasePipeline(IPipelineExecutorConfig context, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.UseInMemoryPipelineActions)
        {
            var pipelineActionDiHelper = new PipelineExecutorDiServiceHelper<TCommand, TResponse>();
            _executor = new InMemoryPipelineExecutor<TCommand, TResponse>(
                pipelineActionDiHelper.GetActions(serviceProvider, context.ActionConfigContexts));
        }
    }

    /// <summary>
    /// Gets the result of the pipeline execution.
    /// </summary>
    public BasePipelineExecutorResult<IPipelineExecutionCommandObject<TCommand, TResponse>> ExecutorResult { get; private set; }

    /// <summary>
    /// Executes the pipeline actions asynchronously.
    /// </summary>
    /// <param name="command">The command object to process.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    public async Task ProcessActionsAsync(
        IPipelineExecutionCommandObject<TCommand, TResponse> command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_executor);

        await _executor.ExecuteActionsAsync(command, cancellationToken);
        ExecutorResult = _executor.ExecutorResult;
    }
}

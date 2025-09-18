using PipelineExecutor;
using PipelineExecutor.Models;
using PipelineExecutorDemo.DynamicConfigurationPipelineExample.Models;

namespace PipelineExecutorDemo.DynamicConfigurationPipelineExample;

public interface IDynamicPipeline : IBasePipeline<DynamicPipelineCommand,DynamicPipelineResponse>
{
    Task InitializeAsync(DynamicPipelineCommand command,CancellationToken cancellation = default);   
}

public class DynamicPipeline : BasePipeline<DynamicPipelineCommand, DynamicPipelineResponse>, IDynamicPipeline
{
    private readonly IServiceProvider _serviceProvider;
    
    public DynamicPipeline(IServiceProvider serviceProvider) 
    {
        _serviceProvider = serviceProvider;
    }
    
    // this method can be replace with a config from db,api, consul etc
    public async Task InitializeAsync(DynamicPipelineCommand command,CancellationToken cancellation = default)
    {
        base.SetConfig(BuildConfiguration(command.Source),_serviceProvider);
        await Task.CompletedTask;
    }

    private IPipelineExecutorConfig BuildConfiguration(DynamicPipelineSource source)
    {
        var config = new PipelineExecutorConfig();
        switch (source)
        {
            case DynamicPipelineSource.Website:
                config = CreateConfigForWebsite();
                break;
            case DynamicPipelineSource.MobileApp:
                config = CreateConfigForMobileApp();
                break;
     
            default:
                throw new ArgumentOutOfRangeException(nameof(source), source, null);
        }

        return config;
    }
    
    
    private PipelineExecutorConfig CreateConfigForWebsite()
    {
        var config = new PipelineExecutorConfig()
        {
            ActionConfigContexts = new List<PipelineExecutorActionConfigContext>()
            {
                new ()
                {
                       ActionName= typeof(Actions.DynamicPipelineValidateCommandAction).ToString(), 
                        Position = 1,
                        HasCompensate = false
                },
                new()
                {
                    ActionName= typeof(Actions.DynamicPipelineSetUser).ToString(),
                    Position = 2,
                    HasCompensate = false
                },
                new()
                {
                    ActionName= typeof(Actions.DynamicPipelineSetUserDetails).ToString(),
                    Position = 3,
                    HasCompensate = false
                },
                new()
                {
                    ActionName= typeof(Actions.DynamicPipelineEmailNotification).ToString(),
                    Position = 3,
                    HasCompensate = false
                },
                new()
                {
                    ActionName= typeof(Actions.DynamicPipelineSmsNotification).ToString(),
                    Position = 4,
                    HasCompensate = false
                },
                new ()
                {
                    ActionName= typeof(Actions.DynamicPipelineAnalyticsUserAction).ToString(),
                    Position = 5,
                    HasCompensate = false
                }
            }
        };
        return config;
    }
    
    private PipelineExecutorConfig CreateConfigForMobileApp()
    {
        var config = new PipelineExecutorConfig()
        {
            ActionConfigContexts = new List<PipelineExecutorActionConfigContext>()
            {
                new ()
                {
                       ActionName= typeof(Actions.DynamicPipelineValidateCommandAction).ToString(), 
                        Position = 1,
                        HasCompensate = false
                },
                new()
                {
                    ActionName= typeof(Actions.DynamicPipelineSetUser).ToString(),
                    Position = 2,
                    HasCompensate = false
                },
                new()
                {
                    ActionName= typeof(Actions.DynamicPipelineSetUserDetails).ToString(),
                    Position = 3,
                    HasCompensate = false
                },
                new()
                {
                    ActionName= typeof(Actions.DynamicPipelinePushNotification).ToString(),
                    Position = 4,
                    HasCompensate = false
                },
                new ()
                {
                    ActionName= typeof(Actions.DynamicPipelineAnalyticsUserAction).ToString(),
                    Position = 6,
                    HasCompensate = false
                }
            }
        };
        return config;
    }
    
}
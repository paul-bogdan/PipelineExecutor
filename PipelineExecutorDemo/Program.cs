using Microsoft.AspNetCore.Http.HttpResults;
using PipelineExecutor;
using PipelineExecutorDemo.DynamicConfigurationPipelineExample;
using PipelineExecutorDemo.DynamicConfigurationPipelineExample.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// register the dynamic pipeline
builder.Services.AddScoped<IDynamicPipeline, DynamicPipeline>();
builder.Services.RegisterPipelineActions(typeof(DynamicPipeline).Assembly);
// 



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.MapGet("/user-registration/website", async (IDynamicPipeline pipeline) =>
{
    var executionModel = new DynamicPipelineModel()
    {
        Command = new DynamicPipelineCommand()
        {        Name = "John Doe",
            Email = "jHoidsandsaiDoe@gmail.com",
            Source = DynamicPipelineSource.Website
            
        }
    };
    
    await pipeline.InitializeAsync(executionModel.Command);
    await pipeline.ProcessActionsAsync(executionModel,CancellationToken.None);
    return Results.Ok(executionModel.Response);
});

app.MapGet("/user-registration/mobile", async (IDynamicPipeline pipeline) =>
{
    var executionModel = new DynamicPipelineModel()
    {
        Command = new DynamicPipelineCommand()
        {   
            Name = "John Doe",
            Email = "jHoidsandsaiDoe@gmail.com",
            Source = DynamicPipelineSource.MobileApp
            
        }
    };
    
    await pipeline.InitializeAsync(executionModel.Command);
    await pipeline.ProcessActionsAsync(executionModel,CancellationToken.None);
    if (pipeline.ExecutorResult.Success)
    {
        return Results.Ok(executionModel.Response); 
    }
    else
    {
        return Results.InternalServerError(pipeline.ExecutorResult.Errors);
    }
});

app.MapGet("/user-registration/mobile-faulted", async (IDynamicPipeline pipeline) =>
{
    var executionModel = new DynamicPipelineModel()
    {
        Command = new DynamicPipelineCommand()
        {   
            Name = string.Empty,
            Email = "jHoidsandsaiDoe@gmail.com",
            Source = DynamicPipelineSource.MobileApp
            
        }
    };
    
    await pipeline.InitializeAsync(executionModel.Command);
    await pipeline.ProcessActionsAsync(executionModel,CancellationToken.None);
    if (pipeline.ExecutorResult.Success)
    {
        return Results.Ok(executionModel.Response); 
    }
    else
    {
        return Results.InternalServerError(pipeline.ExecutorResult.Errors);
    }
});


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
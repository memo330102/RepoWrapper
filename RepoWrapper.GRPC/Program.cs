using Refit;
using RepoWrapper.Application.Interfaces;
using RepoWrapper.Application.Services;
using RepoWrapper.GRPC.Mapping;
using RepoWrapper.GRPC.Services;
using RepoWrapper.Infrastructure.GRPCInterfaces;
using Serilog;

Log.Logger = new LoggerConfiguration() 
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddGrpc();

builder.Services.AddScoped<IGithubService, GithubService>();
builder.Services.AddScoped<IGrpcMapper, GrpcMapper>();

var githubApiBaseUrl = builder.Configuration["GitHubApi:BaseUrl"];
builder.Services.AddRefitClient<IGithubServiceGrpcClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(githubApiBaseUrl);
        c.DefaultRequestHeaders.UserAgent.ParseAdd("Wrapper");
        c.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GithubGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

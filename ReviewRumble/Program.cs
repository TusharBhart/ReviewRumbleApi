using Microsoft.EntityFrameworkCore;
using Refit;
using ReviewRumble.Business;
using ReviewRumble.Models;
using ReviewRumble.Repository;
using ReviewRumble.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder => {
        policyBuilder.WithOrigins("https://localhost:4200");
        policyBuilder.WithMethods("GET", "POST", "PUT");
        policyBuilder.AllowAnyHeader();
    });
});

builder.Services.AddSingleton<ReviewsConfigManager>();
builder.Services.AddScoped<IPullRequestBal, PullRequestBal>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<GithubClientSettings>(
    builder.Configuration.GetSection(GithubClientSettings.GithubApiClientSettings));

builder.Services
	.AddRefitClient<IGithubApiClient>()
	.ConfigureHttpClient(httpClient =>
	{
		httpClient.BaseAddress = new Uri(builder.Configuration["GithubApiClientSettings:BaseUrl"]);
		httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
	});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthorization();

app.MapControllers();
app.Run();

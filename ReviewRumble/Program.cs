using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using ReviewRumble.Business;
using ReviewRumble.Extension;
using ReviewRumble.Models;
using ReviewRumble.Repository;
using ReviewRumble.utils;
using ReviewRumble.Utils;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.Configure<JwtOption>(builder.Configuration.GetSection(JwtOption.SectionName));

builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.WithOrigins("https://localhost:4200", "http://localhost:4200");
        policyBuilder.WithMethods("GET", "POST", "PUT");
        policyBuilder.AllowAnyHeader();
    });
});

builder.Services.AddSingleton<ReviewsConfigManager>();
builder.Services.AddScoped<IPullRequestBal, PullRequestBal>();
builder.Services.AddScoped<IUserBal, UserBal>();
builder.Services.AddScoped<IAuthBal, AuthBal>();
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddJwtTokenServices(builder.Configuration);

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<GithubClientSettings>(
    builder.Configuration.GetSection(GithubClientSettings.GithubAppClientSettings));

builder.Services
	.AddRefitClient<IGithubClient>(GetDefaultRefitSettings(new SnakeCaseNamingStrategy()))
	.ConfigureHttpClient(httpClient =>
	{
		httpClient.BaseAddress = new Uri(builder.Configuration["GithubAppClientSettings:BaseUrl"]);
		httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
	});

builder.Services
	.AddRefitClient<IGithubApiClient>(GetDefaultRefitSettings(new SnakeCaseNamingStrategy()))
	.ConfigureHttpClient(httpClient =>
	{
		httpClient.BaseAddress = new Uri(builder.Configuration["GithubApiClientSettings:BaseUrl"]);
		httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
		httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.3");
	});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

RefitSettings GetDefaultRefitSettings(NamingStrategy strategy)
{
	return new RefitSettings
	{
		ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
		{
			ContractResolver = new DefaultContractResolver()
			{
				NamingStrategy = strategy
			},
			NullValueHandling = NullValueHandling.Ignore,
			MissingMemberHandling = MissingMemberHandling.Ignore
		})
	};
}
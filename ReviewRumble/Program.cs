using Microsoft.EntityFrameworkCore;
using Refit;
using ReviewRumble;
using ReviewRumble.Business;
using ReviewRumble.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<ConfigurationService>();
builder.Services.AddScoped<IPullRequestBal, PullRequestBal>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
	.AddRefitClient<IGithubApiClient>()
	.ConfigureHttpClient(httpClient =>
	{
		httpClient.BaseAddress = new Uri(builder.Configuration["GithubApiClientSettings:BaseUrl"]);
		httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();

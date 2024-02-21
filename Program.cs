using Microsoft.EntityFrameworkCore;
using PlatformService;
using PlatformService.Data;
using PlatformService.Repositories;
using PlatformService.SyncDataServices.Http;

/* .ENV Loading */
var root = Directory.GetCurrentDirectory();
var dotenvFile = Path.Combine(root, ".env");
DotEnv.Load(dotenvFile);

var builder = WebApplication.CreateBuilder(args);

/*** Add Configuration to the Container ***/
builder.Configuration.AddEnvironmentVariables();

/*** Add Services to the Container ***/
// if (builder.Environment.IsDevelopment())
// {
//     builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));
// }
// else if (builder.Environment.IsProduction())
// {
//     try
//     {
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var server = Environment.GetEnvironmentVariable("MSSQL_DATABASE_SERVER");
    var port = Environment.GetEnvironmentVariable("MSSQL_DATABASE_PORT");
    var database = Environment.GetEnvironmentVariable("MSSQL_DATABASE_DATABASE");
    var username = Environment.GetEnvironmentVariable("MSSQL_DATABASE_USERNAME");
    var password = Environment.GetEnvironmentVariable("MSSQL_DATABASE_PASSWORD");

    options.UseSqlServer(
        $"Server={server},{port};Database={database};User ID={username};Password={password};Trusted_Connection=False;TrustServerCertificate=True"
    );
});
//     }
//     catch (Exception e)
//     {
//         Console.WriteLine(e.Message);
//     }
// }
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

/* Initial Data Seeding */
// Seeder.Initialize(app, app.Environment.IsProduction());

app.Run();
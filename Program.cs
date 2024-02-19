using Microsoft.EntityFrameworkCore;
using PlatformService;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

/*** Add Configuration to the Container ***/
builder.Configuration.AddEnvironmentVariables();

/*** Add Services to the Container ***/
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));
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

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

/* .ENV Loading */
var root = Directory.GetCurrentDirectory();
var dotenvFile = Path.Combine(root, ".env");
DotEnv.Load(dotenvFile);

/* Initial Data Seeding */
Seeder.Initialize(app);

app.Run();
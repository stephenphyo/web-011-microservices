using PlatformService.Models;

namespace PlatformService.Data
{
    public static class Seeder
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        public static void SeedData(AppDbContext ctx)
        {
            if (!ctx.Platforms.Any())
            {
                Console.WriteLine("Seeding Data ...");
                ctx.Platforms.AddRange(
                    new Platform() { Name = ".NET", Publisher = "Microsoft" },
                    new Platform() { Name = "SQL Server", Publisher = "Microsoft" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation" }
                );
                ctx.SaveChanges();
                Console.WriteLine("Data Seeded Successfully ...");
            }
            else
            {
                Console.WriteLine("Data Already Existed");
            }
        }
    }
}
using Store.Memory;

namespace Store.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IJewelryRepository, JewelryRepository>();
            builder.Services.AddSingleton<JewelryService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapGet("/hi", () => "Hello!");

            app.MapDefaultControllerRoute();
            app.MapRazorPages();

            app.Run();
        }
    }
}
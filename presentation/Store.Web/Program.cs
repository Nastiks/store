using Store.Contractors;
using Store.Data.EF;
using Store.Messages;
using Store.Web.App;
using Store.Web.Contractors;
using Store.YandexKassa;

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
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddEfRepositories(builder.Configuration.GetConnectionString("Store"));

            builder.Services.AddSingleton<INotificationService, DebugNotificationService>();
            builder.Services.AddSingleton<IDeliveryService, PostamateDeliveryService>();
            builder.Services.AddSingleton<IPaymentService, CashPaymentService>();
            builder.Services.AddSingleton<IPaymentService, YandexKassaPaymentService>();
            builder.Services.AddSingleton<IWebContractorService, YandexKassaPaymentService>();
            builder.Services.AddSingleton<JewelryService>();
            builder.Services.AddSingleton<OrderService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapDefaultControllerRoute();                
            });

            app.MapRazorPages();

            app.Run();
        }
    }
}
using Blazored.LocalStorage;
using FRONTEND.Components;
using FRONTEND.Data.Auth;
using FRONTEND.Data.JAXB;
using FRONTEND.Data.REST;
using FRONTEND.Data.SOAP;
using FRONTEND.Data.XML_RPC;

namespace FRONTEND
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddHttpClient();

            builder.Services.AddScoped<AuthClient>();
            builder.Services.AddScoped<SOAPClient>();
            builder.Services.AddScoped<DHMZClient>();
            builder.Services.AddScoped<RESTClient>();
            builder.Services.AddScoped<JAXBClient>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}

using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JSRuntimeRepro
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryConsole();
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection TryConsole(this IServiceCollection serviceCollection)
        {
            Console.WriteLine("Write to the console from the .net");
            Task t = LogFromJS();
            t.RunSynchronously();
            return serviceCollection;
        }

        public static Task<bool> LogFromJS()
        {
            return Interop.Log();
        }
    }

    public class Interop
    {
        public static Task<bool> Log()
        {
            Console.WriteLine("Is JSRuntime.Current null?");
            Console.WriteLine(JSRuntime.Current == null);
            return JSRuntime.Current.InvokeAsync<bool>("TryConsole.log");
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Digst.DigitalPost.SSLClient
{
    public class SSLClientApplication
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
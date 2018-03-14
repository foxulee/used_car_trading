using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace project_vega
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //you can call ConfigureAppConfiguration again after creating the default builder. Doing so will not replace the default configuration, but add on top of it.
            //add UserSecrets including ConnectinString to Startup config
            .ConfigureAppConfiguration((builderContext, config) =>
                {
                    if (builderContext.HostingEnvironment.IsDevelopment())
                    {
                        config.AddUserSecrets<Startup>();
                    }
                })
                .UseStartup<Startup>()
                .Build();

    }
}

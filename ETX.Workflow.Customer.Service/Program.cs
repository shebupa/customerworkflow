IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
     {
         var startUp = new Startup(hostContext.Configuration,
                                   hostContext.HostingEnvironment);
         System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
         startUp.ConfigureServices(services);
     }).Build();

await host.RunAsync();
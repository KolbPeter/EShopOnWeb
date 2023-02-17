using Microsoft.Extensions.Hosting;
using OrderItemsReserver;

var configurator = new Configurator();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(c => configurator.Configure(c))
    .ConfigureServices(s => configurator.Configure(s))
    .Build();

host.Run();

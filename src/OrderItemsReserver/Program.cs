using Microsoft.Extensions.Hosting;
using OrderItemsReserver;

var configurator = new Configurator();

var host = new HostBuilder()
    .ConfigureAppConfiguration(c => configurator.Configure(c))
    .ConfigureServices(s => configurator.Configure(s))
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();

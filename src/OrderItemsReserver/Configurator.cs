using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderItemsReserver;

internal class Configurator
{
    public IServiceCollection Configure(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IStorage, BlobStorage>();
        
        return serviceCollection;
    }

    public IConfigurationBuilder Configure(IConfigurationBuilder builder)
    {
        builder.AddJsonFile("appsettings.json", false, true);
        return builder;
    }
}

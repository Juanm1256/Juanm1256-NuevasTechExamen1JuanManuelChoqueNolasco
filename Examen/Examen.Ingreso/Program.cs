using Examen.Ingreso.Contratos.Repositorios;
using Examen.Ingreso.Implentacion.Repositorio;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IProveedorLogic, ProveedorRepo>();
        services.AddScoped<IProductoRepo, ProductoRepo>();
    })
    .Build();

host.Run();

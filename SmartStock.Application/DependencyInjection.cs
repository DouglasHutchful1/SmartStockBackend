using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SmartStock.Application.Features.Auth.Handlers;
using SmartStock.Application.Features.Products.Handlers;
using SmartStock.Application.Features.Sales.Handlers;
using SmartStock.Application.Features.Creditors.Handlers;
using SmartStock.Application.Features.Reports.Handlers;

namespace SmartStock.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        return services;
    }
}

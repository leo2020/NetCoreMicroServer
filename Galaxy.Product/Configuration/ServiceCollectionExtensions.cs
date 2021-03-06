﻿using Galaxy.Product;
using Galaxy.Product.Abstraction;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProductService(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            return services;
        }

        public static IServiceCollection AddProductServiceAsGrpc(this IServiceCollection services, Action<GrpcOptions> options)
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            var grpcOptions = new GrpcOptions();
            options?.Invoke(grpcOptions);

            services.AddScoped(sp => GrpcChannel.ForAddress(grpcOptions.ServerAddress));
            services.AddScoped(sp =>
            {
                var channel = sp.GetRequiredService<GrpcChannel>();
                return channel.CreateGrpcService<IProductService>();
            });

            return services;
        }
    }
}

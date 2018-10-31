using System;
using System.Collections.Generic;
using System.Text;
using Armin.Suitsupply.Domain.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Armin.Suitsupply.Domain
{
    public static class DomainRegisterar
    {
        public static void AddDomain(this IServiceCollection service)
        {
            service.AddScoped<IProductStore, ProductStore>();

        }
    }
}

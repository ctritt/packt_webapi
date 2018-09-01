using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pact_WebApi.Services;

namespace Pact_WebApi.Middleware
{
    public static class MiddlewareExtensions
    {
        public static async void AddSeedData(this IApplicationBuilder app)
        {

            var seedDataService = app.ApplicationServices.GetService<ISeedDataService>();
            await seedDataService.EnsureSeedData();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Synk.Installers
{
    public class CorsInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
                services.AddCors(options =>
                {
                    options.AddPolicy("localhost",
                        builder =>
                        {
                            builder.WithOrigins("http://localhost:4200")
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();
                        });
                });
        }
    }
}

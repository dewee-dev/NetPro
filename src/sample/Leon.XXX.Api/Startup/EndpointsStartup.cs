﻿// <auto-generated>
//  UseEndpoints   
// </auto-generated>

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NetPro.Web.Api
{
    /// <summary>
    /// Endpoints 
    /// </summary>
    public class EndpointsStartup : INetProStartup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="typeFinder"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, ITypeFinder typeFinder)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder application, IWebHostEnvironment env)
        {
            application.UseEndpoints(s =>
            {
                s.MapControllers();
            });
        }

        /// <summary>
        /// UseEndpoints should be loaded last
        /// </summary>
        public double Order { get; set; } = 1000;
    }
}

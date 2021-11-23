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
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, TypeFinder.ITypeFinder typeFinder)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        public void Configure(IApplicationBuilder application)
        {
            application.UseEndpoints(s =>
            {
                s.MapControllers();//add other operation
            });
        }

        /// <summary>
        /// UseEndpoints should be loaded last
        /// </summary>
        public double Order { get; set; } = 1000;
    }
}

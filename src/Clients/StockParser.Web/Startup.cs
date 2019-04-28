using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockParser.Data;
using StockParser.Data.WebParser;
using StockParser.Sql;
using StockParser.Sql.Repositories;

namespace StockParser.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                   .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var conn = Configuration.GetConnectionString("mssql");
            services.AddSingleton(new SqlContext(conn))
                .AddSingleton<IStockRepository, StockRepository>()
                .AddSingleton<IBistRepository, BistRepository>()
                .AddScoped<ParserService>()
                .AddSingleton<IWebParser, BigParaParser>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}

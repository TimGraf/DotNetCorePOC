using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Xml.XPath;
using DotNetCouchbasePOC.Config;
using DotNetCouchbasePOC.Services;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Annotations;

namespace DotNetCouchbasePOC
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;
        public Startup(IHostingEnvironment env)
        {
            _hostingEnv = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup options with DI
            services.AddOptions();
            
            // Configure SearchConfig using a sub-section of the appsettings.json file
            services.Configure<SearchServiceConfig>(Configuration.GetSection("SearchServiceConfig"));
            services.Configure<DocumentServiceConfig>(Configuration.GetSection("DocumentServiceConfig"));

            // DI for custom services
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IBeerService, BeerService>();

            // Add framework services.
            services.AddMvc();

            //var pathToDoc = Configuration["Swagger:Path"];

            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Beer Search API",
                    Description = "A simple api to search beers using Elasticsearch and Couchbase",
                    TermsOfService = "None"
                });
                //options.IncludeXmlComments(pathToDoc);
                options.DescribeAllEnumsAsStrings();

                //var comments = new XPathDocument($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");
                //options.OperationFilter<XmlCommentsOperationFilter>(comments);
                //options.ModelFilter<XmlCommentsModelFilter>(comments);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUi();
        }
    }
}

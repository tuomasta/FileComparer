using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi;
using Applications;
using Applications.Repository;
using Application.Repository;
using Microsoft.EntityFrameworkCore;

namespace FileComparer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

                // TODO: should be enabled only in development
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });

            ConfigureIOC(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("api", new Swashbuckle.AspNetCore.Swagger.Info()
                {
                    Title = "FileComparer"
                });
                c.OperationFilter<FileUploadOperation>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(setup => setup.SwaggerEndpoint("/swagger/api/swagger.json", "FileComparer"));

            app.UseMvc();
        }

        private static void ConfigureIOC(IServiceCollection services)
        {
            services.AddTransient<IStoreText, TextRepository>();
            services.AddTransient<ICompareFiles, Applications.FileComparer>();

            // using in memory database for simplicity (don't want to have any infrastructure dependencies)
            services.AddDbContextPool<Context>(options => options.UseInMemoryDatabase("fileDb"));
        }
    }
}

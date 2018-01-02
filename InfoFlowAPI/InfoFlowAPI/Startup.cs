using InfoFlowAPI.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace InfoFlowAPI
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
            services.AddDbContext<DbCtx>(opt => opt.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddSwaggerGen(opt => opt.SwaggerDoc("v1", new Info {Title = "Info Flow API", Version="1.0" }));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc()
               .UseSwagger()
               .UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Info Flow API V1.0"));
        }
    }
}

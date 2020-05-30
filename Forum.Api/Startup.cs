using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Forum.Api.Requests;
using Forum.Api.Validator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Forum.Core.Abstract.Managers;
using Forum.Core.Abstract.Repositories;
using Forum.Core.Concrete.Managers;
using Forum.Core.Concrete.Models;
using Forum.Core.Concrete.Repositories;
using Forum.Core.Persistence;
using Newtonsoft.Json;

namespace Forum.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", optional: true);

            if (Environment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(o =>
                o.UseMySql(Configuration["Data:ConnectionString"],
                    mySqlOptions =>
                        mySqlOptions.ServerVersion(Configuration["Data:ServerVersion"])));

            services.AddControllers(options => options.OutputFormatters.Clear())
                .AddNewtonsoftJson(o => { o.UseMemberCasing(); }).AddFluentValidation(fv =>
                {
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    fv.RegisterValidatorsFromAssemblyContaining<TagRequestValidator>();
                });


            // Add Mapper and Mapper Configs
            services.AddAutoMapper(typeof(Startup).Assembly);


            // Bind Repositories and Managers Here...
            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostManager, PostManager>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagManager, TagManager>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
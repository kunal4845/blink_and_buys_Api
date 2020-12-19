using AutoMapper;
using BlinkAndBuys.Helpers;
using Core.Common;
using Core.Helper;
using DataAccessLayer.IRepository;
using DataAccessLayer.Repository;
using Grocery.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.Extensions.Logging;

namespace BlinkAndBuys
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
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<SmtpCredentials>(Configuration.GetSection("SmtpCredentials"));

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //Fetching Connection string from APPSETTINGS.JSON  
            var ConnectionString = Configuration.GetConnectionString("BlinkandBuys");

            //Entity Framework  
            services.AddDbContext<BlinkandBuysContext>(options => options.UseSqlServer(ConnectionString));

            // CORS  
            services.AddCors(options =>
            {
                options.AddPolicy("CORS", corsPolicyBuilder => corsPolicyBuilder.WithOrigins("http://localhost:4200")
                    // Apply CORS policy for any type of origin  
                    .AllowAnyMethod()
                    // Apply CORS policy for any type of http methods  
                    .AllowAnyHeader()
                    // Apply CORS policy for any headers  
                    .AllowCredentials());
                // Apply CORS policy for all users  
            });

            //files
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            // make sure the cors statement is above AddMvc() method.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Repositories and services injection
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IDealerRepository, DealerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IServiceProviderRepository, ServiceProviderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IContactUsRepository, ContactUsRepository>();
            services.AddScoped<IBookedProductRepository, BookedProductRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            loggerFactory.AddFile("Logs/BlinkBuys-{Date}.txt");

            app.UseHttpsRedirection();
            app.UseCors("CORS");

            app.UseStaticFiles(); // For the wwwroot folder
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                          Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            app.UseMiddleware<JwtMiddleware>();

            app.UseMvc();
        }
    }
}

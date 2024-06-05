using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using MediatR;
using AutoMapper;
using Data.Context;
using Data.Repositories;
using Domain.Services;
using Domain.Interfaces;
using Infra;

namespace OsmoseInverse
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
            //CORS
            //method 1
            //services.AddCors();

            //OR
            //method 2
            services.AddCors(option =>
            {
                option.AddPolicy("MyCorsPolicy",
                builder =>
                {
                    builder
                    //.AllowAnyOrigin()
                    .WithOrigins("https://localhost:4200", "http://localhost:4200")       //4200: Angular's port
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    ;
                });
            });

            //DbContext
            services.AddDbContext<OsmoseInverseDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddSwaggerGen();   //swagger

            services.AddSwaggerGen(options =>    //swagger
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OsmoseInverse",
                    Description = "OsmoseInverse",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",

                    }
                });
            });


            ////jwt
            //services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            ////jwt
            ////services.AddScoped<IUserService, UserService>();
            //services.AddTransient<IUserService, UserService>();

            //services.Configure<AuthOptions>(Configuration.GetSection("AuthOptions"));

            //var authOptions = Configuration.GetSection("AuthOptions").Get<AuthOptions>();

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        RequireExpirationTime = true,
            //        ValidIssuer = authOptions.Issuer,
            //        ValidAudience = authOptions.Audience,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SecureKey))
            //    };
            //});

            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));     //AutoMapper
            services.AddMediatR(typeof(Startup));
            services.AddMediatR(Assembly.GetExecutingAssembly());

            RegisterServices(services);

            //IUserRepository can ONLY be implemented by UserRepository
            services.AddScoped<IUserRepository, UserRepository>();  //User repo

            //IRoleRepository can ONLY be implemented by RoleRepository
            services.AddScoped<IRoleRepository, RoleRepository>();  //Role repo
            
            services.AddScoped<AuthService>();   //jwt service

            services.AddControllers().AddNewtonsoftJson(options =>
              options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddLogging(); // register logging service
        }

        private void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "OsmoseInverse");    //change api name to WebApplication

            });

            //app.UseHttpsRedirection();    //the server redirects HTTP requests to HTTPS. comment this line when using HTTP endpoints (e.g. 'http://localhost:5000/Users')
            app.UseRouting();

            //CORS
            //method 1
            app.UseCors("MyCorsPolicy");

            //OR
            //method 2
            //app.UseCors(option => option
            ////.WithOrigins(new[] { "http://localhost:4200", "https://localhost:4200" })   //4200: Angular port
            //.WithOrigins("http://localhost:4200","https://localhost:4200")
            ////.AllowAnyOrigin()
            //.AllowAnyHeader()
            //.AllowAnyMethod()
            //.AllowCredentials()
            //);

            //method 3
            //app.UseCors(builder =>
            //    builder.WithOrigins("http://localhost:4200", "https://localhost:4200"));

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //jwt
            //app.UseAuthentication();
        }
    }
}

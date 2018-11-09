﻿using System;
using System.IO;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCompany.MyProject.Common;
using MyCompany.MyProject.Data;
using MyCompany.MyProject.Web.Application;

namespace MyCompany.MyProject.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly AppSettings _settings;

        public Startup(IOptions<AppSettings> options, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _env = env;
            _settings = options.Value;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(b =>
            {
                b.AllowAnyHeader().AllowAnyMethod();
                if (_env.IsProduction()) b.WithOrigins(_settings.CorsOrigins);
                else b.AllowAnyOrigin();
            });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                if (!_env.IsProduction())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.Migrate();
                }
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseMvc();
            app.UseAppSwagger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(w => w.FullName.StartsWith(Constants.AppName));
            services.AddMediatR(appAssemblies);
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.AddAppSwagger();
            services.AddAppAuthentication();
            services.AddAppAuthorization();
            services.AddDbContextPool<AppDbContext>(b => b.UseSqlServer(_settings.ConnectionStrings.Default));
            services.AddLoggingFileUI(o => o.Path = Path.Combine(AppContext.BaseDirectory, Constants.DataPath, "logs"));

            services.AddMvc(o =>
            {
                o.Filters.Add<GlobalExceptionHandleFilter>();
                o.ModelMetadataDetailsProviders.Add(new RequiredBindingMetadataProvider());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime;
using DreamingHome.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace DreamingHome
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc();
            services.AddEntityFrameworkSqlite()
                .AddDbContext<MainContext>(options => options.UseSqlite(Configuration["database:connection"]));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    info: new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Dreaming Home 智能家装平台",
                        Description = "智能家装平台 Api 文档",
                        TermsOfService = new Uri("http://blog.deali.cn"),
                        Contact = new OpenApiContact
                        {
                            Name = "DealiAxy",
                            Email = "dealiaxy@gmail.com",
                            Url = new Uri("https://zhuanlan.zhihu.com/deali"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "GNU GENERAL PUBLIC LICENSE Version 2",
                            Url = new Uri("https://www.gnu.org/licenses/old-licenses/gpl-2.0.html"),
                        }
                    });
                // 为 Swagger JSON and UI设置xml文档注释路径
                //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "Doc", "DreamingHome.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dreaming Home Api V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
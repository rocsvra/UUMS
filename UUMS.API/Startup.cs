using AdunTech.ExceptionDetail;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UUMS.Infrastructure;

namespace UUMS.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterDbContexts(Configuration.GetConnectionString("Bull_HR"));
            services.RegisteRepository();
            services.AddProblemDetails(ConfigureProblemDetails);

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    //��ʹ���շ���ʽ
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new DefaultNamingStrategy()
                    };
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    //ʱ���ʽ
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    //����ѭ������
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddJsonOptions(options =>
                {
                    //Swagger�ĵ���������˵����ʹ���շ�
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                }); ;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UUMS.API", Version = "v1" });

                c.OrderActionsBy(o => o.HttpMethod); 
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "UUMS.API.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "UUMS.Domain.xml"));
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "UUMS.Application.xml"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UUMS.API v1"));
            }

            app.UseProblemDetails();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            options.IncludeExceptionDetails = (ctx, ex) => Env.IsDevelopment();

            options.Rethrow<NotSupportedException>();
            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

            //=====�Զ����쳣=====
            string errUrl = "http://aduntech.com/error/codes/";
            options.Map<ResourceNotFoundException>(ex => new ResourceNotFoundExceptionDetails(errUrl, ex));
            options.Map<BadRequestException>(ex => new BadRequestExceptionDetails(errUrl, ex));
            options.Map<InternalServerException>(ex => new InternalServerExceptionDetails(errUrl, ex));
            //=====�Զ����쳣=====

            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        }
    }
}

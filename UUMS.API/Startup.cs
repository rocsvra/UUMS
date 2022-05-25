using AdunTech.ExceptionDetail;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net.Http;
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
            //跨域设置
            services.AddCors(options => options.AddPolicy("policy",
                    builder =>
                    {
                        builder.AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithOrigins("http://localhost:9528/");
                    }));

            //注册仓储
            services.RegisterRepository(Configuration.GetConnectionString("Bull_HR"));
            //注册异常处理
            services.AddProblemDetails(ConfigureProblemDetails);

            services.Configure<JwtTokenOptions>(Configuration.GetSection("JwtToken"));
            //注册认证
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
               {
                   var securityKey = Configuration.GetSection("JwtToken:SecurityKey").Value;
                   var issuer = Configuration.GetSection("JwtToken:Issuer").Value;
                   var audience = Configuration.GetSection("JwtToken:Audience").Value;
                   JwtTokenOptions jwtTokenOptions = new JwtTokenOptions()
                   {
                       SecurityKey = securityKey,
                       Issuer = issuer,
                       Audience = audience
                   };
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = jwtTokenOptions.Key,

                       ValidateIssuer = true,
                       ValidIssuer = jwtTokenOptions.Issuer,

                       ValidateAudience = true,
                       ValidAudience = jwtTokenOptions.Audience,

                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.FromMinutes(5)
                   };
               });
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    //不使用驼峰样式
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new DefaultNamingStrategy()
                    };
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    //时间格式
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    //忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddJsonOptions(options =>
                {
                    //Swagger文档样例参数说明不使用驼峰
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UUMS.API", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT 认证",
                    Description = "输入 JWT bearer token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, new string[] { } }
                });

                c.OrderActionsBy(o => o.HttpMethod);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "UUMS.API.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "UUMS.Domain.xml"));
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "UUMS.Application.xml"));
            });
        }
        // 18957813252
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UUMS.API v1"));

            app.UseProblemDetails();

            app.UseRouting();
            app.UseAuthentication();
            app.UseCors("policy");
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

            //=====自定义异常=====
            string errUrl = "http://aduntech.com/error/codes/";
            options.Map<ResourceNotFoundException>(ex => new ResourceNotFoundExceptionDetails(errUrl, ex));
            options.Map<BadRequestException>(ex => new BadRequestExceptionDetails(errUrl, ex));
            options.Map<InternalServerException>(ex => new InternalServerExceptionDetails(errUrl, ex));
            //=====自定义异常=====

            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        }
    }
}

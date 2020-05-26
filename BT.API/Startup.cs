using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BT.API.Models;
using BT.Data;
using BT.Service;
using BT.Service.Employee;
using BT.Service.Role;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BT.API
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
            services.AddControllers();

            //注册Swagger服务
            services.AddSwaggerGen(options=> {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CRM API开发文档", Version = "v1" });

                string path = Path.Combine(Directory.GetCurrentDirectory(), "BT.API.xml");

 
                options.IncludeXmlComments(path);
            });

            //注册数据库上下文

            services.AddDbContext<CRMContext>(optons=>optons.UseMySql(Configuration["Mysql"]));


            

            //注册仓储包装服务
            services.AddScoped<IWrapperRepository, WrapperRepository>();

            //注册JwtBearer身份认证
            //还原人一个当事人对象 Principal
            //设置默认认证方案为Jwt Bearer身份认证  /Cookie身份认证
            services.AddAuthentication(defaultScheme:JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options=> {
                    //配置Token的认证参数
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidIssuer = "www.zhangquque.com",  //发行人
                        ValidAudience = "www.gaozijian.com", //签收者
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes  ("asdadhajhdkjahsdkjahdkj9au8d9adasidoad89asu813e")),//密钥
                        ValidateLifetime =true,   //验证Token过期时间
                        // NameClaimType = JwtClaimTypes.Name           //Name的声明类型    
                    };
                    
                });


            //将配置文件内容注册到相对应的实体对象中
            services.Configure<CustomConfigOptions>(Configuration.GetSection("CustomConfigOptions"));


            //注册一个跨域方案
            services.AddCors(options=>options.AddPolicy("cors",options=>options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));


            //注册AutoMapper
            services.AddAutoMapper(typeof(Startup));


            //注册redis分布式缓冲服务
            services.AddDistributedRedisCache(options=> {
                options.Configuration = "129.211.14.185:6379";
                options.InstanceName = "zhangqueque";    

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors("cors");

            //配置Swagger中间件
            app.UseSwagger();

            app.UseSwaggerUI(options=>options.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API开发文档"));


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

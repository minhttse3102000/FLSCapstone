using BEAPICapstoneProjectFLS.AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.IRepositories;
using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Repositories;
using BEAPICapstoneProjectFLS.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS
{
    public class Startup
    {
        private readonly string _corsName = "FLS";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var path = Directory.GetCurrentDirectory();
            //FirebaseApp.Create(new AppOptions()
            //{
            //    Credential = GoogleCredential.FromFile($"{path}\\Firebase\\firebase-config.json"),
            //});

            services.AddCors(option =>
            {
                option.AddPolicy(name: _corsName,
                                    builder =>
                                    {
                                        builder.WithOrigins().AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                                    });
            });
            services.AddControllers();

            services.AddDbContext<FLSCapstoneDatabaseContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("MyDB"));
            });
            services.AddAutoMapper(typeof(DepartmentGroupMapper).Assembly);
            services.AddAutoMapper(typeof(DepartmentMapper).Assembly);
            services.AddAutoMapper(typeof(ScheduleMapper).Assembly);
            services.AddAutoMapper(typeof(RoleMapper).Assembly);
            services.AddAutoMapper(typeof(UserAndRoleMapper).Assembly);
            services.AddAutoMapper(typeof(UserMapper).Assembly);
            services.AddAutoMapper(typeof(SubjectMapper).Assembly);
            services.AddAutoMapper(typeof(SlotTypeMapper).Assembly);
            services.AddAutoMapper(typeof(SemesterMapper).Assembly);
            services.AddAutoMapper(typeof(RoomSemesterMapper).Assembly);
            services.AddAutoMapper(typeof(RoomTypeMapper).Assembly);
            services.AddAutoMapper(typeof(CourseMapper).Assembly);
            services.AddAutoMapper(typeof(LecturerCourseGroupMapper).Assembly);
            services.AddAutoMapper(typeof(RequestMapper).Assembly);
            services.AddAutoMapper(typeof(SubjectOfLecturerMapper).Assembly);
            services.AddAutoMapper(typeof(CourseAssignMapper).Assembly);
            services.AddAutoMapper(typeof(LecturerSlotConfigMapper).Assembly);
            services.AddAutoMapper(typeof(CourseGroupItemMapper).Assembly);



            services.AddScoped<IDepartmentGroupService, DepartmentGroupService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserAndRoleService, UserAndRoleService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ISlotTypeService, SlotTypeService>();
            services.AddScoped<ISemesterService, SemesterService>();
            services.AddScoped<IRoomSemesterService, RoomSemesterService>();
            services.AddScoped<IRoomTypeService, RoomTypeService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ILecturerCourseGroupService, LecturerCourseGroupService>();
            services.AddScoped<ISemesterService, SemesterService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<ISubjectOfLecturerService, SubjectOfLecturerService>();
            services.AddScoped<ICourseAssignService, CourseAssignService>();
            services.AddScoped<ILecturerSlotConfigService, LecturerSlotConfigService>();
            services.AddScoped<ICourseGroupItemService, CourseGroupItemService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISwapService, SwapService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICheckConstraintService, CheckConstraintService>();

            services.AddControllersWithViews()
                .AddNewtonsoftJson(option =>
                {

                    option.SerializerSettings.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy(),
                    };

                    option.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddScoped<FLSCapstoneDatabaseContext>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            services.Configure<AppSetting>(Configuration.GetSection("AppSettings"));

            var secretKey = Configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    //tự cấp token
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    //ký vào token
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                    ClockSkew = TimeSpan.Zero
                };
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BEAPICapstoneProjectFLS", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id ="Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BEAPICapstoneProjectFLS v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });*/

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsProduction() || env.IsStaging())
            {
                app.UseExceptionHandler("/Error/index.html");
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");

                // custom CSS
                c.InjectStylesheet("/swagger-ui/custom.css");
            });

            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(_corsName);

            
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}

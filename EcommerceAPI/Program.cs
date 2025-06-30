
using Ecommerce.API.mapping_profiles;
using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories;
using Ecommerce.Core.IRepositories.IService;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EcommerceAPI
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.CacheProfiles.Add("defaultCache",
                    new CacheProfile()
                    {
                        Duration = 30,
                        Location = ResponseCacheLocation.Any,
                    }
                    );
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies(false);


            });
            builder.Services.AddScoped(typeof(IProductsRepositories),typeof(ProductRepository));

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository <>));
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            builder.Services.AddScoped<ITokenService,TokenService>() ;
            builder.Services.AddTransient<IEmailServices, EmailService>();
            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan=TimeSpan.FromHours(1);

            });
            /*builder.Services.AddScoped(typeof(RoleManager<IdentityRole>));
            builder.Services.AddScoped(typeof(SignInManager<IdentityRole>));*/
            var key = builder.Configuration.GetValue<string>("ApiSetting:SecretKey");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken=true;
                options.TokenValidationParameters = new TokenValidationParameters { 
                ValidateIssuer=false, 
                    ValidateAudience=false,

                ValidateIssuerSigningKey=true,
                IssuerSigningKey=new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
                };
            });
            builder.Services.AddIdentity<LocalUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errors=ActionContext.ModelState.Where(x=>x.Value.Errors.Count()>0)
                    .SelectMany(x=>x.Value.Errors)
                    .Select(e=>e.ErrorMessage)
                    .ToList();
                    var validationResponse = new APIValidationResponse(statusCode: 400) { Errors = errors };
                    return new BadRequestObjectResult(validationResponse);
                }; 
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

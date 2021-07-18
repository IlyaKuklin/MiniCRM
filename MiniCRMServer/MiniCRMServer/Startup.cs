using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniCRMCore;
using MiniCRMCore.Areas.Auth;
using MiniCRMServer.Middleware;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCRMServer
{
	public class Startup
	{
		private const string CORS_NAME = "CorsPolicy";

		public IConfiguration Configuration { get; }

		public Startup(IWebHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();

			this.Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy(CORS_NAME,
					builder => builder.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader());
			});

			services.Configure<FormOptions>(o =>
			{
				o.ValueLengthLimit = int.MaxValue;
				o.MultipartBodyLengthLimit = int.MaxValue;
				o.MemoryBufferThreshold = int.MaxValue;
			});

			var connectionString = "Host=vm469442.eurodir.ru;Database=CRMData;Username=postgres;Password=books1";
			//var connectionString = "Host=localhost;Database=CRMData;Username=postgres;Password=books1";
			services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));

			services
			.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = "Bearer";
				options.DefaultScheme = "Bearer";
				options.DefaultChallengeScheme = "Bearer";
			})
			.AddJwtBearer(cfg =>
			{
				cfg.RequireHttpsMetadata = false;
				cfg.SaveToken = true;
				cfg.TokenValidationParameters = this.GetTokenValidationParameters();
			});

			services.AddAutoMapperProfiles();

			services.AddScoped<AuthService>();

			services.AddControllers().AddNewtonsoftJson();
			services.AddSwaggerGen(SwaggerGenApiExtentions.Configure);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwaggerApi();

			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseMiddleware<ExceptionMiddleware>();

			app.UseRouting();
			app.UseCors(CORS_NAME);
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		private TokenValidationParameters GetTokenValidationParameters()
		{
			return new TokenValidationParameters
			{
				ValidIssuer = this.Configuration["JwtIssuer"],
				ValidAudience = this.Configuration["JwtIssuer"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["JwtKey"])),
				ClockSkew = TimeSpan.Zero
			};
		}
	}

	public static class SwaggerGenApiExtentions
	{
		public static void UseSwaggerApi(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(x =>
			{
				x.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API");
			});
		}

		internal static void Configure(SwaggerGenOptions options)
		{
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "Books API",
				Description = "Books API Reference"
			});

			options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
			{
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer",
				Description = "� ���� ������ ������ � ������� 'Bearer {�����}'"
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement()
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						},
						Scheme = "oauth2",
						Name = "Bearer",
						In = ParameterLocation.Header,
					},
					new List<string>()
				}
			});

			options.CustomSchemaIds(x =>
			{
				var name = $"{x.Name}";
				var schemaName = x.DeclaringType != null ? $"{x.DeclaringType.Name}.{name}" : name;

				return schemaName;
			});
		}
	}

	public static class ServicesExtentions
	{
		public static void AddAutoMapperProfiles(this IServiceCollection services)
		{
			var profiles = new List<Type>
			{
				typeof(BooksMappingProfile)
			};

			services.AddAutoMapper(profiles.ToArray());
		}
	}

	public class BooksMappingProfile : AutoMapper.Profile
	{
		public BooksMappingProfile()
		{
		}
	}
}

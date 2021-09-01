using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniCRMCore;
using MiniCRMCore.Areas.Auth;
using MiniCRMCore.Areas.Auth.Models;
using MiniCRMCore.Areas.Clients;
using MiniCRMCore.Areas.Clients.Models;
using MiniCRMCore.Areas.Common;
using MiniCRMCore.Areas.Email;
using MiniCRMCore.Areas.Email.Models;
using MiniCRMCore.Areas.Offers;
using MiniCRMCore.Areas.Offers.Models;
using MiniCRMCore.Utilities.Serialization;
using MiniCRMServer.Middleware;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

			var connectionString = this.Configuration.GetSection("ConnectionString").Value;
			//var connectionString = "Host=vm469442.eurodir.ru;Database=CRMData;Username=postgres;Password=books1";
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
			services.AddScoped<ClientsService>();
			services.AddScoped<OffersService>();
			services.AddScoped<CommonService>();
			services.AddScoped<EmailSenderService>();

			services.AddControllers()
				.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

			services.AddSwaggerGen(SwaggerGenApiExtentions.Configure);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwaggerApi();

			//app.UseDefaultFiles();
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
				Title = "CRM API",
				Description = "CRM API Reference"
			});

			//options.SchemaFilter<ValueTypesRequiredSchemaFilter>();

			options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
			{
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer",
				Description = "В поле ввести данные в формате 'Bearer {токен}'"
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
				typeof(BaseMappingProfile),
				typeof(AuthMappingProfile),
				typeof(ClientsMappingProfile),
				typeof(OffersMappingProfile),
				typeof(CommonMappingProfile)
			};

			services.AddAutoMapper(profiles.ToArray());
		}
	}

	public class BaseMappingProfile : Profile
	{
		public BaseMappingProfile()
		{
			this.CreateMap<BaseEntity, BaseEntity.BaseDto>();
		}
	}

	public class CommonMappingProfile : Profile
	{
		public CommonMappingProfile()
		{
			this.CreateMap<CommunicationReport, CommunicationReport.Dto>();
			this.CreateMap<CommunicationReport.EditDto, CommunicationReport>()
				.ForMember(x => x.Id, opt => opt.Ignore())
				.ForMember(x => x.AuthorId, opt => opt.Ignore())
				;

			this.CreateMap<EmailSettings, EmailSettings.Dto>()
				.ReverseMap()
				.ForMember(x => x.Id, opt => opt.Ignore());
		}
	}

	public class AuthMappingProfile : Profile
	{
		public AuthMappingProfile()
		{
			this.CreateMap<User, User.Dto>();
		}
	}

	public class ClientsMappingProfile : Profile
	{
		public ClientsMappingProfile()
		{
			this.CreateMap<Client, Client.Dto>()
				.ForMember(x => x.Offers, opt => opt.MapFrom(src => src.Offers))
				.ReverseMap()
				.ForMember(x => x.Id, opt => opt.Ignore())
				.ForMember(x => x.Offers, opt => opt.Ignore())
				.ForMember(x => x.CommonCommunicationReports, opt => opt.Ignore())
				;
		}
	}

	public class OffersMappingProfile : Profile
	{
		public OffersMappingProfile()
		{
			this.CreateMap<Offer, Offer.Dto>()
				.ReverseMap()
				.ForMember(x => x.Id, opt => opt.Ignore())
				.ForMember(x => x.Number, opt => opt.Ignore())
				.ForMember(x => x.FileData, opt => opt.Ignore())
				.ForMember(x => x.Client, opt => opt.Ignore())
				.ForMember(x => x.Newsbreaks, opt => opt.Ignore())
				.ForMember(x => x.FeedbackRequests, opt => opt.Ignore())
				.ForMember(x => x.Rules, opt => opt.Ignore())
				.ForMember(x => x.CommonCommunicationReports, opt => opt.Ignore())
				//.ForMember(x => x.ClientId, opt => opt.Ignore())
				;
			this.CreateMap<Offer.EditDto, Offer>()
				.ForMember(x => x.Id, opt => opt.Ignore());

			this.CreateMap<OfferFileDatum, OfferFileDatum.Dto>()
				.ForMember(x => x.Path, opt => opt.MapFrom(src => src.FileDatum.Path))
				.ForMember(x => x.Name, opt => opt.MapFrom(src => src.FileDatum.Name))
				.ReverseMap()
				;

			this.CreateMap<OfferNewsbreak, OfferNewsbreak.Dto>();
			this.CreateMap<OfferFeedbackRequest, OfferFeedbackRequest.Dto>();

			this.CreateMap<OfferRule, OfferRule.Dto>()
				.ReverseMap()
				.ForMember(x => x.Id, opt => opt.Ignore());

			this.CreateMap<Offer, Offer.ClientViewDto>();
		}
	}

	public class ValueTypesRequiredSchemaFilter : ISchemaFilter
	{
		public void Apply(OpenApiSchema schema, SchemaFilterContext context)
		{
			if (context.Type.IsReferenceOrNullableType())
			{
				schema.Nullable = false;

				var notRequiredAttributeType = typeof(NotRequiredAttribute);

				var typeProperties = context.Type.GetProperties();
				var schemaProperties = schema.Properties.Select(x => x.Key);

				foreach (var property in typeProperties)
				{
					if (property.PropertyType == typeof(string)) continue;

					var notRequired = Attribute.IsDefined(property, notRequiredAttributeType);
					if (!notRequired)
					{
						var schemaProperty = schemaProperties.FirstOrDefault(x => x.ToLower() == property.Name.ToLower());

						if (schemaProperty != null && !schema.Properties[schemaProperty].Nullable)
							schema.Required.Add(schemaProperty);
					}
				}
			}
		}
	}
}
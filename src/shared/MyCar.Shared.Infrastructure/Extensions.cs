using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyCar.Shared.Abstractions;
using MyCar.Shared.Abstractions.Auth;
using MyCar.Shared.Abstractions.Modules;
using MyCar.Shared.Abstractions.Services;
using MyCar.Shared.Infrastructure.Api;
using MyCar.Shared.Infrastructure.Auth;
using MyCar.Shared.Infrastructure.Contexts;
using MyCar.Shared.Infrastructure.Database;
using MyCar.Shared.Infrastructure.Exceptions;
using MyCar.Shared.Infrastructure.Middleware;
using MyCar.Shared.Infrastructure.Modules;
using MyCar.Shared.Infrastructure.Services;
using MyCar.Shared.Infrastructure.Time;

namespace MyCar.Shared.Infrastructure;

public static class Extensions
{
	private const string _corsPolicy = "cors";
	private const string _corsFrontUrlHeaderPolicy = "cors-fronturl-header";
	private const string _docsVersion = "v0.01";
	private const string _titleApi = "MyCar API";

	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration,
		IList<IModule> modules)
	{
		AddSingletons(services);
		AddTransients(services);

		var disableModules = new List<string>();
		foreach(var (key, value) in configuration.AsEnumerable()) {
			if(!key.Contains(":module:enabled")) {
				continue;
			}

			if(!bool.Parse(value)) {
				disableModules.Add(key.Split(":")[0]);
			}
		}

		services.AddModuleInfo(modules);
		services.AddContexts();

		services.AddCors(cors =>
		{
			var a = configuration.GetSection("AllowedHost").Get<string>();
			cors.AddPolicy(name:_corsPolicy, x =>
			{
				x.WithOrigins(configuration.GetSection("AllowedHost").Get<string>())
				 .AllowCredentials()
				 .WithMethods("POST", "PUT", "DELETE")
				 .WithHeaders("Content-Type", "Authorization");
			});
			cors.AddPolicy(name: _corsFrontUrlHeaderPolicy, x =>
			{
				x.WithOrigins(configuration.GetSection("AllowedHost").Get<string>())
				 .AllowCredentials()
				 .WithMethods("POST", "PUT", "DELETE")
				 .WithHeaders("Content-Type", "Authorization", "X-Frontend-Url");
			});
		});
		services.AddAuth(configuration, modules);
		services.AddDatabase(configuration);
		services.AddErrorHandling();
		
		services.AddBackgroundServices(configuration);

		services.AddControllers(options => options.Filters.Add<VaidateModelAttribute>())
			.ConfigureApplicationPartManager(manager =>
			{
				var removedParts = new List<ApplicationPart>();
				foreach(var disableModule in disableModules) {
					var parts = manager.ApplicationParts.Where(x => x.Name.Contains(disableModule, StringComparison.InvariantCultureIgnoreCase));
					removedParts.AddRange(parts);
				}

				foreach(var part in removedParts) {
					manager.ApplicationParts.Remove(part);
				}

				manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
			});

		services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(swagger =>
		{
			swagger.CustomSchemaIds(x => x.FullName!.Replace("+","-"));
			swagger.SwaggerDoc(_docsVersion, new OpenApiInfo 
			{ 
				Title = _titleApi,
				Version = _docsVersion,
			});

			var securityScheme = new OpenApiSecurityScheme
			{
				Name = "JWT Authentication",
				Description = "Enter your JWT token in this field",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.Http,
				Scheme = JwtBearerDefaults.AuthenticationScheme,
				BearerFormat = "JWT"
			};

			swagger.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

			var securityRequirement = new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = JwtBearerDefaults.AuthenticationScheme
						}
					},
					[]
				}
			};

			swagger.AddSecurityRequirement(securityRequirement);
		});

		return services;
	}

	private static void AddSingletons(IServiceCollection services)
	{
		services.AddSingleton<IClock, UtcClock>();
		services.AddSingleton<ITokenValidator, TokenValidator>();
		services.AddSingleton<IEmailSenderFactory, EmailSenderFactory>();
	}

	private static void AddTransients(IServiceCollection services)
	{
		services.AddTransient<SmtpEmailSender>();
		services.AddTransient<FakeEmailSender>();
	}

	public static IApplicationBuilder UseInfrastructure(
		this IApplicationBuilder app,
		IWebHostEnvironment environment)
	{
		app.UseErrorHandling();
		
		if(environment.IsDevelopment()) {
			app.UseSwagger();
			app.UseSwaggerUI(x =>
			{
				x.RoutePrefix = "docs/swagger";
				x.SwaggerEndpoint($"/swagger/{_docsVersion}/swagger.json", _titleApi);
			});
			app.UseReDoc(x =>
			{
				x.RoutePrefix = "docs";
				x.SpecUrl($"/swagger/{_docsVersion}/swagger.json");
				x.DocumentTitle = _titleApi;
			});
		}
		
		app.UseMiddleware<RefreshTokenMiddleware>();
		app.UseAuthentication();

		app.UseRouting();
		app.UseCors(_corsPolicy);
		app.UseAuthorization();

		return app;
	}

	public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
	{
		var options = new T();
		configuration.GetSection(sectionName).Bind(options);

		return options;
	}
}

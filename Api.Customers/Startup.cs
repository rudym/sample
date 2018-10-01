using System.IO;
using System.Reflection;
using Api.Customers.Configs;
using Api.Customers.DataAccess;
using Api.Customers.Models;
using Api.Customers.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace Api.Customers
{
	public class Startup
	{
		private static readonly string _apiTitle = "Customers API";
		private static readonly string _apiVersion = "v1";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddApplicationInsightsTelemetry(Configuration);
			services.AddOptions();

			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, false)
				.AddEnvironmentVariables()
				.Build();

			var appConfig = builder.Get<AppSettings>();
			services.AddSingleton<IAppSettings>(appConfig);

			services.AddCors(options => options.AddPolicy("CorsPolicy",
				p => p.AllowAnyOrigin()
					.AllowAnyHeader()
					.AllowAnyMethod()));

			ApiServicesConfiguration(services, appConfig);

			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
				.AddJsonOptions(options =>
				{
					options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
				});

			services.AddSwaggerGen(
				c =>
				{
					c.SwaggerDoc(_apiVersion, new Info {Title = _apiTitle, Version = _apiVersion});

					var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
					var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, xmlFile);
					c.IncludeXmlComments(filePath);
				});
		}

		private static void ApiServicesConfiguration(IServiceCollection services, AppSettings appConfig)
		{
			services.AddSingleton<IAzureTableSettings>(appConfig.AzureTableSettings);
			services.AddSingleton<IAzureTableStorage<JsonValueTableEntity>, AzureTableStorage<JsonValueTableEntity>>();
			services.AddSingleton<IRepositoryService<Customer>, CustomersService>();
			services.AddSingleton<IRepositoryService<Note>, NotesService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			app.UseCors("CorsPolicy");
			app.UseMvc();

			app.UseSwagger();
			app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{_apiTitle} {_apiVersion}"); });
		}
	}
}
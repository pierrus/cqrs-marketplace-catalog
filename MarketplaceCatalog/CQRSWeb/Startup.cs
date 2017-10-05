using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CQRSlite.Bus;
using CQRSlite.Commands;
using CQRSlite.Messages;
using CQRSlite.Events;
using CQRSlite.Domain;
using CQRSCode.WriteModel.EventStore.Mongo;
using CQRSlite.Cache;
using CQRSlite.Config;
using CQRSCode.WriteModel.Handlers;
using Scrutor;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration.Json;
using System.IO;
using CQRSCode.ReadModel.Events;
using CQRSCode.ReadModel.Dtos;
using CQRSWeb.Models;
using AutoMapper;
using AutoMapper.Configuration;

namespace CQRSWeb
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            services.AddMemoryCache();

            //Add Cqrs services
            services.AddSingleton<InProcessBus>(new InProcessBus());
            services.AddSingleton<ICommandSender>(y => y.GetService<InProcessBus>());
            services.AddSingleton<IEventPublisher>(y => y.GetService<InProcessBus>());
            services.AddSingleton<IHandlerRegistrar>(y => y.GetService<InProcessBus>());
            services.AddScoped<ISession, Session>();

            //Factory to turn scanned IEvents into Type
            services.AddSingleton<IEventStore, EventStore>
                (sProvider => new EventStore(
                    sProvider.GetRequiredService<IEventPublisher>(),
                    sProvider.GetRequiredService<CQRSCode.ReadModel.Repository.MongoOptions>(),
                    sProvider.GetServices<EventType>().ToList()));

            services.AddScoped<ICache, CQRSlite.Cache.MemoryCache>();
            services.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));

            // REFACTORING REQUIRED: DIDN'T MANAGE TO PARSE JSON SETTINGS USING Configuration.GetSection("mongo").Get
            // var config = Configuration.GetSection("configuredClients").
            //services.Configure<CQRSCode.ReadModel.Repository.MongoOptions>(Configuration.GetSection("mongo"));
            services.AddTransient<CQRSCode.ReadModel.Repository.MongoOptions>(sp =>
            {
                var mOpt = new CQRSCode.ReadModel.Repository.MongoOptions();
                mOpt.ConnectionString = Configuration["mongo:connectionString"];
                mOpt.Database = Configuration["mongo:database"];
                return mOpt;
            });
            // services.Configure<CQRSCode.ReadModel.Repository.MongoOptions>(options => Configuration.GetSection("mongo").Get<CQRSCode.ReadModel.Repository.MongoOptions>());

            services.AddTransient(
                        typeof(CQRSCode.ReadModel.Repository.IRepository<>),
                        typeof(CQRSCode.ReadModel.Repository.Repository<>)
                    );

            //Scan for commandhandlers and eventhandlers
            services.Scan(scan => scan
                .FromAssemblies(typeof(CategoryCommandHandlers).GetTypeInfo().Assembly)
                    .AddClasses(classes => classes.Where(x => {
                        var allInterfaces = x.GetInterfaces();
                        return
                            allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IHandler<>)) ||
                            allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableHandler<>));
                    }))
                    .AsSelf()
                    .WithTransientLifetime()
            );

            //Register tous les IEvent
            // services.Scan(scan => scan
            //     .FromAssemblies(typeof(CQRSCode.ReadModel.Events.ProductCreated).GetTypeInfo().Assembly)
            //         .AddClasses(classes => classes.Where(x => {
            //             var allInterfaces = x.GetInterfaces();

            //             return
            //                 allInterfaces.Any(y => y == typeof(IEvent));
            //         }))
            //         .As(type => EventType<type>)
            //         //.AsImplementedInterfaces()
            //         .WithTransientLifetime()
            // );

            typeof(CQRSCode.ReadModel.Events.ProductCreated)
                .GetTypeInfo().Assembly.ExportedTypes
                .ToList().ForEach(type => {
                    if (type.GetInterfaces().Any(iType => iType == typeof(IEvent)))
                    {
                        services.AddTransient<EventType, EventType>(sProvider => new EventType(type));
                    }
                });

            // foreach (var service in services)
            // {
            //     System.Console.WriteLine(service.ServiceType.Name + " " + service.ServiceType.AssemblyQualifiedName);
            // }

            //Register bus
            var serviceProvider = services.BuildServiceProvider();

            var registrar = new BusRegistrar(new DependencyResolver(serviceProvider));
            registrar.Register(typeof(CategoryCommandHandlers));

            // Add framework services.
            services.AddMvc();

            // services.AddAutoMapper(typeof(Startup));

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            var mapper = config.CreateMapper();
            services.AddSingleton<IMapper>(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            AutoMapper.Mapper.Initialize(c => c.AddProfile<MappingProfile>());
        }
    }
}

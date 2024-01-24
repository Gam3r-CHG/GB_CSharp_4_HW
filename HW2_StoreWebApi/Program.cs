using Autofac;
using Autofac.Extensions.DependencyInjection;
using HW2_StoreWebApi.Abstraction;
using HW2_StoreWebApi.Db;
using HW2_StoreWebApi.Mapper;
using HW2_StoreWebApi.Repository;
using HW2_StoreWebApi.Services;
using Microsoft.Extensions.FileProviders;

namespace HW2_StoreWebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var app = BuildApp(args);

        RunApp(app);
    }

    private static WebApplication BuildApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.AddMemoryCache(opt => opt.TrackStatistics = true);

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        var connectionString = builder.Configuration.GetConnectionString("DB");

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterType<ProductRepository>().As<IProductRepository>();
            containerBuilder.RegisterType<GroupRepository>().As<IGroupRepository>();
            containerBuilder.RegisterType<CacheStatistics>().As<ICacheStatistics>().SingleInstance();
            containerBuilder.Register(x => new AppDbContext(connectionString));
        });

        return builder.Build();
    }

    private static void RunApp(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        var staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(staticFilesPath),
            RequestPath = "/static"
        });

        app.Run();
    }
}
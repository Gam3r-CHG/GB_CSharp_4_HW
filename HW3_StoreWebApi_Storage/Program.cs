using Autofac;
using Autofac.Extensions.DependencyInjection;
using HW3_StoreWebApi_Storage.Abstraction;
using HW3_StoreWebApi_Storage.Db;
using HW3_StoreWebApi_Storage.GraphQL;
using HW3_StoreWebApi_Storage.Mapper;
using HW3_StoreWebApi_Storage.Repository;


namespace HW3_StoreWebApi_Storage;

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

        builder.Services.AddGraphQLServer()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>();

        builder.Services.AddMemoryCache(opt => opt.TrackStatistics = true);

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        var connectionString = builder.Configuration.GetConnectionString("DB");

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterType<StorageRepository>().As<IStorageRepository>();
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

        app.MapGraphQL();

        app.Run();
    }
}
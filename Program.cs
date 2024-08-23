using Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PolyglotPersistence.Context;
using PolyglotPersistence.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string? connectionString = builder.Configuration.GetConnectionString("Localhost");
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(connectionString), ServiceLifetime.Transient, ServiceLifetime.Transient
);

// Configura e registra a configuração do MongoDB
builder.Services.Configure<MongoDatabaseConfig>(builder.Configuration.GetSection("MongoDatabaseConfig"));

builder.Services.AddScoped<ProntuarioService>();

// Configura o MongoClient como um singleton
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var mongoConfig = sp.GetRequiredService<IOptions<MongoDatabaseConfig>>().Value;
    return new MongoClient(mongoConfig.ConnectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
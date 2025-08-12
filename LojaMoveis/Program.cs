//using LojaMoveis.Configurations;
//using LojaMoveis.Services;
//using static System.Net.Mime.MediaTypeNames;

//var builder = WebApplication.CreateBuilder(args);
//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

//// Configura MongoDB Settings
//builder.Services.Configure<MongoDbSettings>(
//    builder.Configuration.GetSection("MongoDbSettings"));

//builder.Services.Configure<EmailSettings>(
//    builder.Configuration.GetSection("EmailSettings"));

//// Injetar os serviços
//builder.Services.AddSingleton<ProdutoService>();
//builder.Services.AddSingleton<ClienteService>();
//builder.Services.AddSingleton<AdminService>();
//builder.Services.AddSingleton<PedidoService>();
//builder.Services.AddSingleton<EnderecoService>();
//builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
//builder.Services.AddSingleton<CloudinaryService>();
//builder.Services.AddSingleton<ResetTokenService>();
//builder.Services.AddSingleton<EmailService>();
//builder.Services.AddSingleton<TokenRedefinicaoService>();
//builder.Services.AddSingleton<TokenService>();



//// Configura CORS para permitir acesso do frontend local e online
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontend",
//        policy =>
//        {
//            policy.WithOrigins(
//                    "http://localhost:5173",              // ambiente local
//                    "https://moveis-classic.vercel.app"   // produção Vercel
//                )
//                .AllowAnyHeader()
//                .AllowAnyMethod()
//                .AllowCredentials();
//        });
//});

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure o pipeline HTTP
////if (app.Environment.IsDevelopment())
////{
////    app.UseSwagger();
////    app.UseSwaggerUI();
////}
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LojaMoveis API V1");
//    c.RoutePrefix = string.Empty; // Isso vai exibir o Swagger direto em "/"
//});

//app.UseHttpsRedirection();

//// Habilita o uso da pasta wwwroot para servir arquivos estáticos (como imagens)
//app.UseStaticFiles();

//// Ativa o CORS ANTES da autorização
//app.UseCors("AllowFrontend");

//app.UseAuthentication();
//app.UseAuthorization();
//app.UseDeveloperExceptionPage();

//app.MapControllers();

//app.Run();


using LojaMoveis.Configurations;
using LojaMoveis.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configura MongoDB Settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Registra o MongoClient como singleton (única instância durante o app)
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Registra o MongoDatabase como scoped (uma instância por requisição)
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Registra o AvaliacaoService (escopo compatível com IMongoDatabase)
builder.Services.AddScoped<AvaliacaoService>();

// Injetar os outros serviços
builder.Services.AddSingleton<ProdutoService>();
builder.Services.AddSingleton<ClienteService>();
builder.Services.AddSingleton<AdminService>();
builder.Services.AddSingleton<PedidoService>();
builder.Services.AddSingleton<EnderecoService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddSingleton<CloudinaryService>();
builder.Services.AddSingleton<ResetTokenService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<TokenRedefinicaoService>();
builder.Services.AddSingleton<TokenService>();

// Configura CORS para permitir acesso do frontend local e online
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:5173",              // ambiente local
                    "https://moveis-classic.vercel.app"   // produção Vercel
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LojaMoveis API V1");
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

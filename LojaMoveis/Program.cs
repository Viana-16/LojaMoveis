//using LojaMoveis.Configurations;
//using LojaMoveis.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Configura MongoDB Settings
//builder.Services.Configure<MongoDbSettings>(
//    builder.Configuration.GetSection("MongoDbSettings"));
//builder.Services.Configure<MongoDbSettings>(
//    builder.Configuration.GetSection("MongoDbSettings"));




//// Injetar os serviços
//builder.Services.AddSingleton<ProdutoService>();
//builder.Services.AddSingleton<ClienteService>();
//builder.Services.AddSingleton<AdminService>();
//builder.Services.AddSingleton<EnderecoService>();


//// Configura CORS para permitir o front-end em http://localhost:5173
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontend",
//        policy =>
//        {
//            policy.WithOrigins("http://localhost:5173")
//                  .AllowAnyHeader()
//                  .AllowAnyMethod();
//        });
//});

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//// HABILITA A SERVIR A PASTA wwwroot
//app.UseStaticFiles();

//// Ativa CORS ANTES do UseAuthorization
//app.UseCors("AllowFrontend");

//app.UseAuthorization();

//app.MapControllers();

//app.Run();


using LojaMoveis.Configurations;
using LojaMoveis.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configura MongoDB Settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Injetar os serviços
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



// Configura CORS para permitir o front-end em http://localhost:5173
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilita o uso da pasta wwwroot para servir arquivos estáticos (como imagens)
app.UseStaticFiles();

// Ativa o CORS ANTES da autorização
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();
app.UseDeveloperExceptionPage();

app.MapControllers();

app.Run();

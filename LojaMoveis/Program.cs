using LojaMoveis.Configurations;
using LojaMoveis.Services;

var builder = WebApplication.CreateBuilder(args);

// Configura MongoDB Settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Injetar os serviços
builder.Services.AddSingleton<ProdutoService>();
builder.Services.AddSingleton<ClienteService>();
builder.Services.AddSingleton<AdminService>();


// Configura CORS para permitir o front-end em http://localhost:5173
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ativa CORS ANTES do UseAuthorization
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MovtechProject._3___Domain.CommandHandlers;
using MovtechProject.DataAcess.Data;
using MovtechProject.DataAcess.Repositories;
using MovtechProject.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // pega a conexão do appSettings pelo DefaultConnection
builder.Services.AddSingleton(new Database(connectionString));// AddSingleton indica que uma única instância da classe Database será criada e usada durante toda a vida útil da aplicação

builder.Services.AddScoped<FormGroupService>();
builder.Services.AddScoped<FormService>();
builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<AnswerService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<FormGroupRepository>();
builder.Services.AddScoped<FormRepository>();
builder.Services.AddScoped<QuestionRepository>();
builder.Services.AddScoped<AnswerRepository>();
builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<FormGroupCommandHandlers>();

string jwtSecretKey = "43e4dbf0-52ed-4203-895d-42b586496bd4";

//JWT Authentication configuration0
builder.Logging.AddConsole();
builder.Services.AddAuthentication(options => // Configuração de autentificação, vai procurar a cada requisição para ver se o token existe
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => // 
{
    options.RequireHttpsMetadata = false; // não precisa do https
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters // parametros para efetuar a validação do token
    {
        ValidateIssuerSigningKey = true, // validar a chave
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)), // passando qual chave ele irá usar para validar, e pegando os bites das chaves
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

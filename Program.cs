using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovtechProject.Data;
using MovtechProject.Repositories;
using MovtechProject.Services;
using System.Text;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new Database(connectionString));

builder.Services.AddScoped<FormsGroupService>();
builder.Services.AddScoped<FormsService>();
builder.Services.AddScoped<QuestionsService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AnswerService>();

builder.Services.AddScoped<FormsGroupRepository>();
builder.Services.AddScoped<FormsRepository>();
builder.Services.AddScoped<QuestionsRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AnswerRepository>();

//JWT Authentication configuration
builder.Logging.AddConsole();
builder.Services.AddAuthentication(options =>
{   
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HJAksfaAFJ*(@*!lÇKASKDH)89fpaIDSD")),
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

using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovtechProject.Data;
using MovtechProject.Services;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

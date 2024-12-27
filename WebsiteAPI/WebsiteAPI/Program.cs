using System.Runtime;
using WebsiteAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "DefaultPolicy",
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ConfigService>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}






app.UseRouting();
app.UseCors("DefaultPolicy");
app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using Practice_11_10_2026.Data;
using Practice_11_10_2026.Mappings;
using Practice_11_10_2026.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(AppMappingProfile).Assembly);
});
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskItemService, TaskItemService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
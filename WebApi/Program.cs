using Application;
using Application.Interfaces;
using Infrastructure.Photos;
using Infrastructure.Reports;
using Persistence;
using WebApi.Extensions;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);

builder.Services.AddPoliciesServices();

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection(nameof(CloudinarySettings)));
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped(typeof(IReportService<>), typeof(ReportService<>));

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddSwaggerDocumentation();

builder.Services.AddCors(o => o.AddPolicy("corsapp", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwaggerDocumentation();

app.UseCors("corsapp");

app.UseAuthentication();
app.UseAuthorization();

await app.SeedDataAuthentication();

app.MapControllers();
app.Run();

using Application;
using Persistence;
using WebApi.Extensions;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);

builder.Services.AddPoliciesServices();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddSwaggerDocumentation();

builder.Services.AddOpenApi();

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

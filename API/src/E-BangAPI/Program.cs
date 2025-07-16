using E_BangAPI.BackgroundWorker;
using E_BangAPI.Middleware;
using E_BangAPI.MinimalApi;
using E_BangApplication.Exetensions;
using E_BangDomain.Entities;
using E_BangInfrastructure.Database;
using E_BangInfrastructure.Extensions;
using NLog.Web;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Host.UseNLog();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ErrorHandlerMiddleware>();
builder.Services.AddHostedService<BackgroundMessagerWorker>();
builder.Services.AddScoped<TransactionHandlerMiddleware>();
var app = builder.Build();
using IServiceScope scope = app.Services.CreateScope();
PendingMigrations pendingMigrations = scope.ServiceProvider.GetRequiredService<PendingMigrations>();
pendingMigrations.GetPendingMigrations();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Version one");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<TransactionHandlerMiddleware>();
app.MapIdentityApiFilterable<Account>(new IdentityApiEndpointRouteBuilderOptions
{
    ExcludeRegisterPost = false,
    ExcludeLoginPost = false,
    ExcludeRefreshPost = false,
    ExcludeConfirmEmailGet = false,
    ExcludeResendConfirmationEmailPost = false,
    ExcludeForgotPasswordPost = true,
    ExcludeResetPasswordPost = true,
    ExcludeManageGroup = true,
    Exclude2faPost = false,
    ExcludegInfoGet = true,
    ExcludeInfoPost = true,
});
app.Run();

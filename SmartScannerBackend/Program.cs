using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartScannerBackend.DataAccess;
using SmartScannerBackend.Services.Authentication;
using SmartScannerBackend.Services.AzureAI;
using SmartScannerBackend.Services.OpenAI;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    var sqliteConnection = new SqliteConnection("Data Source=InMemoryDatabase;Mode=Memory;Cache=Shared");
    sqliteConnection.Open();

    builder.Services.AddDbContext<SmartScannerDbContext>(
        options => options.UseSqlite(sqliteConnection)
    );
};

// Add services to the container.
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = PasswordUtilities.tokenIssuer,
                ValidAudience = PasswordUtilities.tokenAudience,
                IssuerSigningKey = new X509SecurityKey(PasswordUtilities.x509Certificate2),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateTokenReplay = true,
            };
        });
builder.Services.AddAuthorization();

builder.Services.AddScoped<IContentExtraction, ContentExtraction>();
builder.Services.AddScoped<IResolveContent, ResolveContent>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

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

public partial class Startup { }
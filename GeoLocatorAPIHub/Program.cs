using GeoLocatorApiHub.Infrastructure.Data;
using GeoLocatorApiHub.Infrastructure.Repositories;
using GeoLocatorApiHub.Services.Interfaces;
using GeoLocatorApiHub.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// IHttpClientFactory
builder.Services.AddScoped<IGeoLocationService, GeoLocationService>();
builder.Services.AddScoped<IGeoLocationRepository, GeoLocationRepository>();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<ApiKeyService>();
builder.Services.AddDbContext<GeoLocatorApiHubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration JWT
var key = Encoding.ASCII.GetBytes("YourSecretKeyHere");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // In production, set to true
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

MappingConfig.RegisterMappings();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
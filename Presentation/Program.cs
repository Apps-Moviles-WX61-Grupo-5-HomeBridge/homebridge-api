using System.Reflection;
using System.Text;
using _1_API.Mapper;
using _1_API.Middleware;
using _2_Domain;
using _2_Domain.IAM.Repositories;
using _2_Domain.IAM.Services;
using _2_Domain.IAM.Services.Commands;
using _2_Domain.IAM.Services.Queries;
using _2_Domain.Publication.Repositories;
using _2_Domain.Publication.Services;
using _2_Domain.Search.Repositories;
using _2_Domain.Search.Services;
using _3_Data;
using _3_Data.IAM.Persistence;
using _3_Data.Publication.Persistence;
using _3_Data.Search.Persistence;
using _3_Data.Shared.Contexts;
using Application.IAM.CommandServices;
using Application.IAM.QueryServices;
using Application.Publication.CommandServices;
using Application.Publication.QueryServices;
using Application.Search.CommandServices;
using Application.Search.QueryServices;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//  @CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", 
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

//  @Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Propertunity API Documentation v1.0.0",
            Description = "An ASP.NET Core Web API for managing Propertunity domain, data, and presentation layers.",
            Contact = new OpenApiContact
            {
                Name = "Contact the team",
                Url = new Uri("https://propertunity.com")
            },
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://opensource.org/licenses/MIT")
            },
            TermsOfService = new Uri("https://propertunity.com/terms-of-service")
        });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        });
        
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }
);

builder.Services.AddScoped<IUserRegisterData, UserRegisterData>();
builder.Services.AddScoped<IUserRegistrationQueryService, UserRegistrationQueryService>();
builder.Services.AddScoped<IUserRegistrationCommandService, UserRegistrationCommandService>();
builder.Services.AddScoped<IUserAuthenticationData, UserAuthenticationData>();
builder.Services.AddScoped<IUserAuthenticationQueryService, UserAuthenticationQueryService>();
builder.Services.AddScoped<IUserAuthenticationCommandService, UserAuthenticationCommandService>();
builder.Services.AddScoped<ISearchCommandService, SearchCommandService>();
builder.Services.AddScoped<ISearchQueryService, SearchQueryService>();
builder.Services.AddScoped<ISearchRepository, SearchRepository>();
builder.Services.AddScoped<IPublicationCommandService, PublicationCommandService>();
builder.Services.AddScoped<IPublicationQueryService, PublicationQueryService>();
builder.Services.AddScoped<IPublicationRepository, PublicationRepository>();
builder.Services.AddScoped<IUserManagerCommandService, UserManagerCommandService>();
builder.Services.AddScoped<IUserManagerQueryService, UserManagerQueryService>();
builder.Services.AddScoped<IUserManagerRepository, UserManagerRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEncryptService, EncryptService>();

var key = builder.Configuration.GetValue<string>("JwtSettings:key");
var keyBytes = Encoding.ASCII.GetBytes(key);

//  @Authentication
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAutoMapper(
    typeof(RequestToModels)
);

//  @Database
var connectionString = builder.Configuration.GetConnectionString("propertunityDataCenterConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
builder.Services.AddDbContext<PropertunityDataCenterContext>(
    dbContextOptions =>
    {
        dbContextOptions.UseMySql(connectionString,
            ServerVersion.AutoDetect(connectionString),
            options => options.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: System.TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null)
        );
    });


var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors("AllowAllOrigins");


using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<PropertunityDataCenterContext>())
{
    context.Database.EnsureCreated();
}

//  Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
    });
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<AuthenticationMiddleware>();

app.Run();
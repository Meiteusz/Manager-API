
using EscNet.IoC.Cryptography;
using EscNet.IoC.Hashers;
using Isopoh.Cryptography.Argon2;
using Manager.Ioc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region Inject And Mapper

builder.Services.AddSingleton(x => builder.Configuration);
Injector.InjectBusinessIocServices(builder.Services);

#endregion

#region Jwt

var secretKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = true;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

#endregion

#region swagger

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Manager - API",
        Version = "v1",
        Description = "Crud API escalável usando boas práticas de programação e DDD - Aulas feitas na Udemy com Lucas Eschechola",
        Contact = new OpenApiContact()
        {
            Name = "Matheus Teixeira",
            Email = "contato.matheuswt@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/matheus-teixeira-43b9821b8/")
        },
    });

    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Description = "Por favor, utilize Bearer (TOKEN)",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[]{ }
    }
    });
});

#endregion

#region Hash

var config = new Argon2Config
{
    Type = Argon2Type.DataIndependentAddressing,
    Version = Argon2Version.Nineteen,
    Threads = Environment.ProcessorCount,
    TimeCost = int.Parse(builder.Configuration["Hash:TimeCost"]),
    MemoryCost = int.Parse(builder.Configuration["Hash:MemoryCost"]),
    Lanes = int.Parse(builder.Configuration["Hash:Lanes"]),
    HashLength = int.Parse(builder.Configuration["Hash:HashLength"]),
    Salt = Encoding.UTF8.GetBytes(builder.Configuration["Hash:Salt"])
};

builder.Services.AddArgon2IdHasher(config);

#endregion


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Configure the HTTP request pipeline.

builder.Host.ConfigureAppConfiguration((app, builder) =>
{
    //if (app.HostingEnvironment.IsProduction())
    //{
        var buildConfig = builder.Build();

        builder.AddAzureKeyVault
        (
            buildConfig["AzureKeyVault:Vault"],    
            buildConfig["AzureKeyVault:ClientId"],    
            buildConfig["AzureKeyVault:ClientSecret"]
        );
    //}
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

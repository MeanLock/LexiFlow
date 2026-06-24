using LexiFlow.API;
using LexiFlow.API.Middlewares;
using LexiFlow.BLL;
using LexiFlow.BLL.Models;
using LexiFlow.BLL.Models.Emails;
using LexiFlow.DAL;
using LexiFlow.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);

});

builder.Services.AddRepositoryUnitOfWork();
builder.Services.AddServices();
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<JwtOption>(
    builder.Configuration.GetSection("JwtOption"));


builder.Services.AddDbContext<LexiFlowDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

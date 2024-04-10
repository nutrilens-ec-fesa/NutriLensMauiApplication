using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NutriLensWebApp.Entities;
using NutriLensWebApp.Interfaces;
using NutriLensWebApp.Repositories;
using NutriLensWebApp.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Serviços interface/repositório

builder.Services.AddTransient<ILogin, LoginRepository>();
builder.Services.AddTransient<ITbcaItemRepository, TbcaItemRepository>();
builder.Services.AddTransient<ITacoItemRepository, TacoItemRepository>();
builder.Services.AddTransient<IUserInfo, UserInfoRepository>();
builder.Services.AddTransient<IBarcodeItem, BarcodeItemRepository>();
builder.Services.AddTransient<IMongoImage, MongoImageRepository>();
builder.Services.AddTransient<IOpenAiPrompt, OpenAiPromptRepository>();

#endregion

#region Configuração Mongo DB

AppMongoDbContext.SetMongoContext(builder.Configuration.GetValue<string>("MongoDbConnectionString"), "NutriLensDtb");

#endregion

#region Configuração sistema de autenticação

TokenService.UpdateSecret(builder.Configuration.GetValue<string>("TokenServiceSecret"));
TokenService.UpdateHoursToExpire(1);

#endregion

#region Configuração OpenAi

OpenAiEntity.SetApiKey(builder.Configuration.GetValue<string>("OpenAiKey"));

#endregion

#region Configuração GeminiAi

GeminiAiEntity.SetApiKey(builder.Configuration.GetValue<string>("GeminiAiKey"));

#endregion

#region Swagger Config

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "NutriLens API - ASP.NET CORE",
            Version = "v1",
            Description = "Api",
            Contact = new OpenApiContact { Name = "Equipe NutriLens", Email = "nutrilens@outlook.com.br" }
        });
});

#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();

byte[] key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("TokenServiceSecret"));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", $"NutriLens API");
});

app.Run();

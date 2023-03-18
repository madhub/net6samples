using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
 const string BearerPrefix = "Bearer ";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwtConfigureOptions =>
{
    jwtConfigureOptions.RequireHttpsMetadata = false;
    jwtConfigureOptions.SaveToken = true;

    var result = new TokenValidationParameters
    {

        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = false,
        SignatureValidator = delegate (string token,
                            TokenValidationParameters parameters)
        {
            var jwt = new JwtSecurityToken(token);
            return jwt;
        },
        ValidateLifetime = false, // this works, need to check why does it check
        //ClockSkew = TimeSpan.Zero,
        //RequireSignedTokens = false,
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("abcdefghi12345"))

    };
    jwtConfigureOptions.TokenValidationParameters= result;

    jwtConfigureOptions.Events = new JwtBearerEvents()
    {
        OnMessageReceived = (messageReceivedContext =>
         {
             if (messageReceivedContext.Request.Headers.TryGetValue("AccessToken", out StringValues headerValue))
             {
                 string token = headerValue;
                 if (!string.IsNullOrEmpty(token) && token.StartsWith(BearerPrefix))
                 {
                     token = token.Substring(BearerPrefix.Length);
                 }

                 messageReceivedContext.Token = token;
             } //else
               //{
               //    messageReceivedContext.NoResult();
               //}


             return Task.CompletedTask;
         })
    };
    

});
builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("mypolicy", configurePolicy =>
    {
        configurePolicy.AddRequirements(new DenyAnonymousAuthorizationRequirement());
    });
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", [Authorize("mypolicy")](HttpContext context) =>
{
    var user = context.User;
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");


app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
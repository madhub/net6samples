using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(OAuth2IntrospectionDefaults.AuthenticationScheme)
    .AddOAuth2Introspection(options =>
    {
        options.IntrospectionEndpoint = "http://localhost";
        //options.Authority = "https://base_address_of_token_service";

        options.ClientId = "J8NFmU4tJVgDxKaJFmXTWvaHO";
        options.ClientSecret = "client_secret_for_introspection_endpoint";
    });
builder.Services.AddHttpClient(OAuth2IntrospectionDefaults.BackChannelHttpClientName)
    .AddHttpMessageHandler(() =>
    {
        //Configure client/handler for the back channel HTTP Client here
        return new DemoHandler();

    });

builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.AddPolicy("demo", builder =>
    {
        builder.AddRequirements(new DenyAnonymousAuthorizationRequirement());
    });

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#route-parameters


// Validate Authorization Header, if present extracts Bearer toke , introspects & return results 
app.MapGet("/api/validate", [Authorize("demo")] (HttpContext context, HttpRequest req, HttpResponse res,ClaimsPrincipal claimsPrincipal) =>
{
    var identity = claimsPrincipal.Identities.FirstOrDefault();
    JwtSecurityToken jwt = new JwtSecurityToken("abc","xyz",identity.Claims);

    res.Headers.Add("X-Authorization", jwt.ToString());
    
    return Results.Ok();

});

app.UseAuthentication();
app.UseAuthorization();
app.Run();



internal class DemoHandler : DelegatingHandler
{
    string response = "{ \"active\": true,\"scope\": \"read write email\",\"client_id\": \"J8NFmU4tJVgDxKaJFmXTWvaHO\", \"username\": \"bing\", \"exp\": 1710676855}";
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(response, Encoding.UTF8, "application/json")
        };

        return Task.FromResult(responseMessage);
    }
}
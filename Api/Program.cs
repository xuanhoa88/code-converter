using Microsoft.OpenApi.Models;
using ICSharpCode.CodeConverter.Util;
using ICSharpCode.CodeConverter.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Code Converter API", Version = "v1" });
});

builder.Services.AddEndpointsApiExplorer();

string localOrigins = "local";
builder.Services.AddCors(options => {
    options.AddPolicy(name: localOrigins, x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(_ => true) // allow any origin
        .AllowCredentials() // allow credentials
    );
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment()) {
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors(localOrigins);

app.MapPost("/api/convert", async (ConvertRequest request) => await ApiConverter.ConvertAsync(request))
    .RequireCors(localOrigins)
    .Produces<ConvertResponse>();

app.MapGet("/api/version", () => CodeConverterVersion.GetVersion())
    .RequireCors(localOrigins)
    .Produces<string>();

app.Run();

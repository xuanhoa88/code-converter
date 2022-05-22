using Microsoft.OpenApi.Models;
using ICSharpCode.CodeConverter.Util;
using ICSharpCode.CodeConverter.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Code Converter API", Version = "v1" });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment()) {
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.MapPost("/api/convert", async (ConvertRequest request) => await ApiConverter.ConvertAsync(request))
    .Produces<ConvertResponse>();

app.MapGet("/api/version", () => CodeConverterVersion.GetVersion())
    .Produces<string>();

app.Run();

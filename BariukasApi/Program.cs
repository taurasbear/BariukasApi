using BariukasApi.Capabilities;
using BariukasApi.Shared;
using BariukasApi.WorksheetFeatures.GetWorksheet;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

BsonSerializer.TryRegisterSerializer(typeof(Guid), GuidSerializer.StandardInstance);
var builder = WebApplication.CreateBuilder(args);

// TODO: remove the modify request options
builder.AddGraphQL().AddBariukasApiTypes().ModifyRequestOptions(o => o.IncludeExceptionDetails = true);

builder.Services.AddScoped<GetWorksheetHandler>();

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration["MongoDB:ConnectionString"]));

builder.Services.AddSingleton<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>()
        .GetDatabase(builder.Configuration["MongoDB:DatabaseName"]));

builder.Services.AddSingleton<IMongoCollection<WorksheetDocument>>(sp =>
    sp.GetRequiredService<IMongoDatabase>()
        .GetCollection<WorksheetDocument>(builder.Configuration["MongoDB:WorksheetCollectionName"]));

builder.RegisterEndpoints();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.MapGraphQL();
app.MapEndpoints();

// app.RunWithGraphQLCommands(args);
app.Run();


// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };
//
// app.MapGet("/weatherforecast", () =>
//     {
//         var forecast = Enumerable.Range(1, 5).Select(index =>
//                 new WeatherForecast
//                 (
//                     DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                     Random.Shared.Next(-20, 55),
//                     summaries[Random.Shared.Next(summaries.Length)]
//                 ))
//             .ToArray();
//         return forecast;
//     })
//     .WithName("GetWeatherForecast");
//
// app.Run();
//
// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
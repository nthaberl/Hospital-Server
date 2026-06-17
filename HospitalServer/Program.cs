using HospitalServer;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapHub<HospitalHub>("/hospitalhub");

//// --- Vital Simulator --- //
//used to emit vitals to the patient management form to emulate live update vitals in critical care area

//enables ability to broadcast to clients from outside hub class 
var hubContext = app.Services.GetRequiredService<IHubContext<HospitalHub>>();
var random = new Random();

//hardcoded patients for simulation to demonstrate functionality
var patients = new List<(string Name, int Room)>
{
    ("Patient 0",          101),
    ("Commander Shephard", 102),
    ("Cloud Strife",       103),
    ("Master Chief",       104)
};

//timer fires every 5 seconds, pushing randomized vitals for each patient
var timer = new Timer(_ =>
{
    foreach (var patient in patients)
    {
        var vitals = $"Room: {patient.Room} | " +
                     $"HR: {random.Next(60, 100)} bpm | " +
                     $"BP: {random.Next(110, 130)}/{random.Next(70, 90)} | " +
                     $"O2: {random.Next(95, 100)}% | " +
                     $"Temp: {Math.Round(97.5 + random.NextDouble() * 2, 1)}°F";

        hubContext.Clients.All.SendAsync("ReceiveVitalsUpdate", patient.Name, vitals);
    }
}, null, 0, 5000);


app.Run();



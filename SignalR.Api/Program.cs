using SignalR.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Api uygulamam?za bir web taray?c? üzerinden istekte bulunuyorsa e?er cors ayat? yapmam?z gerekiyor 
//A?a??da Web uygulamas? için cors ayar? ve izinler verildi
builder.Services.AddCors(action =>
{
    action.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7256").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Cors kullan?m? aktif ediliyor
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MyHub>("/myhub");


app.Run();

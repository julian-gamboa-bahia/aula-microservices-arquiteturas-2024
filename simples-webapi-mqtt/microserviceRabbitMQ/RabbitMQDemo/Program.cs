using RabbitMQDemo.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseUrls("http://*:5225");
builder.WebHost.UseUrls("http://*:80");


// Adiciona o RabbitMQService como Singleton
//builder.Services.AddSingleton<RabbitMQService>();
builder.Services.AddScoped<RabbitMQService>();


// Configuração padrão do projeto
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

using Microsoft.EntityFrameworkCore; // Certifique-se de ter este using
// Outras usings que seu projeto necessitar

var builder = WebApplication.CreateBuilder(args);

// Configuração explícita do Kestrel para HTTP e HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5266); // Porta HTTP
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps(); // Porta HTTPS
    });
});

// Registrar o DbContext com EF Core usando SQLite
builder.Services.AddDbContext<BancoContext>(options =>
    options.UseSqlite("Data Source=banco.db"));

// Registrar serviços MVC e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar Swagger/UI se estiver no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configurações de middleware
// app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
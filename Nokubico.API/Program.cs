using Nokubico.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços ao contentor.
builder.Services.AddControllers();

// Regista os serviços de infraestrutura.
builder.Services.AddInfrastructure(builder.Configuration);

// Configura o Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura os pedidos HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

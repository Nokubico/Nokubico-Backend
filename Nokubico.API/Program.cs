using Nokubico.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços ao contentor.
builder.Services.AddControllers();

// Regista a infraestrutura, incluindo o contexto da base de dados.
builder.Services.AddInfrastructure(builder.Configuration);

// Configura a documentação Swagger (OpenAPI).
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura o processamento dos pedidos HTTP.
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

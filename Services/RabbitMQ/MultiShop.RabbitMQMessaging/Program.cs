using MultiShop.RabbitMQMessaging.Configuration;
using MultiShop.RabbitMQMessaging.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddOptions<RabbitMqOptions>()
    .BindConfiguration(RabbitMqOptions.SectionName)
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.HostName),
        "RabbitMQ host adı zorunludur.")
    .Validate(
        options => options.Port is > 0 and <= 65535,
        "RabbitMQ port numarası geçerli olmalıdır.")
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.UserName),
        "RabbitMQ kullanıcı adı zorunludur.")
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.Password),
        "RabbitMQ parolası user-secrets veya environment üzerinden verilmelidir.")
    .ValidateOnStart();

builder.Services.AddSingleton<RabbitMqConnection>();
builder.Services.AddSingleton<RabbitMqTopology>();
builder.Services.AddSingleton<ProcessedMessageStore>();
builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();
builder.Services.AddHostedService<RabbitMqConsumer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

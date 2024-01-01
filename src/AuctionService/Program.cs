using AuctionService;
using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
  opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x =>
{
  // Create an outbox, if the item cannot be publish to RabbitMQ, it will be store in DB (postgres)
  // Every 10 seconds it will look for item in Outbox to sync
  // Logic to check and add item to outbox is in DBcontext
  x.AddEntityFrameworkOutbox<AuctionDbContext>(o => {
    o.QueryDelay = TimeSpan.FromSeconds(10);
    o.UsePostgres();
    o.UseBusOutbox();
  });

  x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();
  x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));
  x.UsingRabbitMq((context, cfg) =>
  {
    cfg.ConfigureEndpoints(context);
  });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(op => 
  {
    op.Authority = builder.Configuration["IdentityServiceUrl"];
    op.RequireHttpsMetadata = false;
    op.TokenValidationParameters.ValidateAudience = false;
    op.TokenValidationParameters.NameClaimType = "username";
  });
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
  DbInitializer.InitDb(app);
}
catch (Exception e)
{
  Console.WriteLine(e);
}

app.Run();

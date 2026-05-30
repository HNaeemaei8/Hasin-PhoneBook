using PhoneBook.Api.Common;
using PhoneBook.Api.Middleware;
using PhoneBook.Application.Services;
using PhoneBook.Domain.Repositories;
using PhoneBook.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "دفترچه تلفن ترابرنت (DDD سبک)", Version = "v1" });
});

builder.Services.AddSingleton<IContactRepository, InMemoryContactRepository>();

builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ResultFilter>();
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
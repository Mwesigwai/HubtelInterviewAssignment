using AutoMapper;
using Hubtel.Interview.Assignment.UserWallet.Data;
using Hubtel.Interview.Assignment.UserWallet.Mappers;
using Hubtel.Interview.Assignment.UserWallet.Repositories;
using Hubtel.Interview.Assignment.UserWallet.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WalletApiDbContext>(options => options.UseInMemoryDatabase("WalletsDatabase"));
builder.Services.AddAutoMapper(typeof(WalletMappingProfile));
builder.Services.AddScoped<IRelationaDbWalletsRepository, RelationalDbWalletsRepository>();
builder.Services.AddScoped<IWalletService, WalletService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

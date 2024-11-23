using AutoMapper;
using Hubtel.Interview.Assignment.UserWallet.Data;
using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
using Hubtel.Interview.Assignment.UserWallet.Mappers;
using Hubtel.Interview.Assignment.UserWallet.Models;
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
builder.Services.AddScoped<IErrorHandlerStrategyFactory, ErrorHandlerStrategyFactory>();
builder.Services.AddScoped(provider => ErrorHandlerChainBuilder<List<WalletModel>, string>.BuildErrorHandlerChain());
builder.Services.AddScoped(provider => ErrorHandlerChainBuilder<List<WalletModel>, List<ResponseDto>>.BuildErrorHandlerChain());
builder.Services.AddScoped(provider => ErrorHandlerChainBuilder<WalletModel, ResponseDto>.BuildErrorHandlerChain());


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
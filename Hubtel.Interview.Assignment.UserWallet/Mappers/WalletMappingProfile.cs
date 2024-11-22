using AutoMapper;
using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.Enums;
using Hubtel.Interview.Assignment.UserWallet.Models;

namespace Hubtel.Interview.Assignment.UserWallet.Mappers;
public class WalletMappingProfile : Profile
{
    public WalletMappingProfile()
    {
        CreateMap<WalletModelDto, WalletModel>()
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber.Substring(0, Math.Min(6, src.AccountNumber.Length))))
            .ForMember(dest => dest.AccountScheme, opt => opt.MapFrom(src => MapAccountScheme(src.AccountScheme)))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => MapWalletType(src.Type)));

        CreateMap<WalletModel, WalletModelDto>()
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
            .ForMember(dest => dest.AccountScheme, opt => opt.MapFrom(src => src.AccountScheme.ToString()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
    }

    private WalletAccountScheme MapAccountScheme(string accountScheme)
    {
        if (Enum.TryParse<WalletAccountScheme>(accountScheme, true, out var parsedScheme))
        {
            return parsedScheme;
        }
        throw new ArgumentException($"Invalid AccountScheme value: {accountScheme}");
    }

    private WalletType MapWalletType(string type)
    {
        if (Enum.TryParse<WalletType>(type, true, out var parsedType))
        {
            return parsedType;
        }
        throw new ArgumentException($"Invalid Type value: {type}");
    }
}

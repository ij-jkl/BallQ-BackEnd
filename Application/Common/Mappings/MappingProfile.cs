namespace Application.Common.Mappings;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<CreateStrikerCommand, StrikerEntity>().ReverseMap();
        CreateMap<CreateStrikerDto, StrikerEntity>().ReverseMap();
        CreateMap<UpdateStrikerDto, StrikerEntity>().ReverseMap();
        CreateMap<GetStrikerDto, StrikerEntity>().ReverseMap();
        
        CreateMap<RatingEntity, GetRatingDto>()
            .ForMember(dest => dest.PlayerName, opt => 
                opt.MapFrom(src => src.Player != null ? src.Player.Name : null))
            .ForMember(dest => dest.Nationality, opt => 
                opt.MapFrom(src => src.Player != null ? src.Player.Nationality : null));

    }
}

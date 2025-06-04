namespace Application.Common.Mappings;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<CreateStrikerCommand, StrikerEntity>().ReverseMap();
        CreateMap<CreateStrikerDto, StrikerEntity>().ReverseMap();
        CreateMap<UpdateStrikerDto, StrikerEntity>().ReverseMap();
    }
}

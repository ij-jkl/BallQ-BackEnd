using Application.Strikers.Dtos.QueriesDto;

namespace Application.Common.Mappings;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<CreateStrikerCommand, StrikerEntity>().ReverseMap();
        CreateMap<CreateStrikerDto, StrikerEntity>().ReverseMap();
        CreateMap<UpdateStrikerDto, StrikerEntity>().ReverseMap();
        CreateMap<GetStrikerDto, StrikerEntity>().ReverseMap();
    }
}

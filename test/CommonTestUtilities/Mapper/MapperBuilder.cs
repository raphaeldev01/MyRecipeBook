
using AutoMapper;
using MyRecipeBook.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        return new MapperConfiguration(opt => opt.AddProfile(new AutoMap())).CreateMapper();
    }
}

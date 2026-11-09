using Dictionaries.Application.Dtos;
using Dictionaries.Domain;
using Mapster;

namespace Dictionaries.Application.Mapping;

public sealed class ThemeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Theme, ThemeDto>()
            .Map(dest => dest.Status, src => src.Status.ToString());
    }
}

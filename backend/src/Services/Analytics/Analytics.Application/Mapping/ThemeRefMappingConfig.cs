using Analytics.Domain;
using Analytics.Application.Dtos;
using Mapster;

namespace Analytics.Application.Mapping;

public sealed class ThemeRefMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ThemeRef, ThemeRefDto>();
        config.NewConfig<ThemeRef, ThemeTagDto>();
    }
}

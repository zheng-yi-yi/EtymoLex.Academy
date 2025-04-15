using AutoMapper;
using System.Reflection;
using EtymoLex.Academy;
using System;
using EtymoLex.Academy.Dtos.Morpheme;

namespace EtymoLex.Academy;

public class AcademyApplicationAutoMapperProfile : Profile
{
    public AcademyApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Morpheme, MorphemeDto>();
        CreateMap<CreateUpdateMorphemeDto, Morpheme>(MemberList.Source);
        CreateMap<Morpheme, ExportMorphemeDto>();
        CreateMap<ExportMorphemeDto, Morpheme>();
        CreateMap<MorphemeExample, MorphemeExampleDto>();
        CreateMap<CreateUpdateMorphemeExampleDto, MorphemeExample>(MemberList.Source);
        CreateMap<MorphemeExample, ExportMorphemeExampleDto>();
        CreateMap<ExportMorphemeExampleDto, MorphemeExample>();
    }

    public static void MapNotNullProperty(object source, object target)
    {
        if (source == null || target == null)
        {
            throw new ArgumentNullException(source == null ? nameof(source) : nameof(target));
        }


        PropertyInfo[] dtoProperties = source.GetType().GetProperties();


        foreach (PropertyInfo dtoProperty in dtoProperties)
        {
            if (dtoProperty.Name == "Id")
            {
                continue;
            }

            PropertyInfo targetProperty = target.GetType().GetProperty(dtoProperty.Name);

            if (targetProperty != null && dtoProperty.CanRead && targetProperty.CanWrite)
            {

                object dtoValue = dtoProperty.GetValue(source, null);

                if (dtoValue != null)
                {

                    targetProperty.SetValue(target, dtoValue, null);
                }
            }
        }

    }
}

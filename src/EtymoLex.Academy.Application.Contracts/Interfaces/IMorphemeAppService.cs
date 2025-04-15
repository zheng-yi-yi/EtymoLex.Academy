using System;
using EtymoLex.Academy.Dtos.Morpheme;
using EtymoLex.Academy.Interfaces;

namespace EtymoLex.Academy;


public interface IMorphemeAppService :
INameObjectAppService < 
        MorphemeDto, 
        Guid, 
        MorphemeGetListInput,
        CreateUpdateMorphemeDto,
        ExportMorphemeDto>
{

}
using EtymoLex.Academy.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace EtymoLex.Academy.Dtos.Morpheme;

[Serializable]
public class MorphemeDto : NameObjectDto<Guid>
{
    public string Value { get; set; }

    public MorphemeType Type { get; set; }

    public string OriginLanguage { get; set; }

    public string Meaning { get; set; }
    public List<MorphemeExampleDto>? Examples { get; set; }
}


[Serializable]
public class MorphemeExampleDto : AuditedEntityDto<Guid>
{
    public string Word { get; set; }
    public string Definition { get; set; }
    public string Breakdown { get; set; }
}
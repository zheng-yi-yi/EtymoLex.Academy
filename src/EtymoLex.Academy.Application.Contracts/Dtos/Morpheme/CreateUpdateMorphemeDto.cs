using EtymoLex.Academy.Enums;
using System;
using System.Collections.Generic;

namespace EtymoLex.Academy.Dtos.Morpheme;

[Serializable]
public class CreateUpdateMorphemeDto : CreateUpdateNameObjectDto
{
    public string Value { get; set; }

    public MorphemeType Type { get; set; }

    public string OriginLanguage { get; set; }

    public string Meaning { get; set; }
    public List<CreateUpdateMorphemeExampleDto>? Examples { get; set; }
}

[Serializable]
public class CreateUpdateMorphemeExampleDto
{
    public string Word { get; set; }
    public string Definition { get; set; }
    public string Breakdown { get; set; }
}
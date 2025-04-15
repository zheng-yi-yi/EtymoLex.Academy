using EtymoLex.Academy.Enums;
using System;

namespace EtymoLex.Academy.Dtos.Morpheme;

[Serializable]
public class CreateUpdateMorphemeDto:CreateUpdateNameObjectDto
{
    public string Value { get; set; }

    public MorphemeType Type { get; set; }

    public string OriginLanguage { get; set; }

    public string Meaning { get; set; }
}
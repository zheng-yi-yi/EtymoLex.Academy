using EtymoLex.Academy.Enums;

namespace EtymoLex.Academy.Dtos.Morpheme
{
    public class ExportMorphemeDto
    {
        public string Value { get; set; }

        public MorphemeType Type { get; set; }

        public string OriginLanguage { get; set; }

        public string Meaning { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

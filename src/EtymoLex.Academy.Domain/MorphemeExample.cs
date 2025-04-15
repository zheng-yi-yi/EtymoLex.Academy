using System;
using System.Text.Json.Serialization;

namespace EtymoLex.Academy
{
    public class MorphemeExample : BaseEntity<Guid>
    {
        public Guid ParentId { get; set; }
        [JsonIgnore]
        public Morpheme Parent { get; set; }
        public string Word { get; set; }       // 例词（如 "inspect"）
        public string Definition { get; set; } // 单词释义
        public string Breakdown { get; set; }  // 词素分解（in- + spect
    }
}

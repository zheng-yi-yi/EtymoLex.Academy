using EtymoLex.Academy.Enums;
using System;

namespace EtymoLex.Academy
{
    public class Morpheme : NameObject<Guid>
    {
        public required string Value { get; set; }           // 词素内容（如 "spect"）
        public MorphemeType Type { get; set; }               // 类型枚举
        public required string OriginLanguage { get; set; }  // 来源语言
        public required string Meaning { get; set; }         // 核心含义
    }
}

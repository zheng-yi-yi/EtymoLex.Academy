using System;

namespace EtymoLex.Academy.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ExportAttribute : Attribute
{
    public string RelatedDomain { get; }
    public string RelatedProperty { get; }
    public string RelatedId { get; }

    public ExportAttribute(string relatedDomain, string relatedProperty, string relatedId)
    {
        RelatedDomain = relatedDomain;
        RelatedProperty = relatedProperty;
        RelatedId = relatedId;
    }
}

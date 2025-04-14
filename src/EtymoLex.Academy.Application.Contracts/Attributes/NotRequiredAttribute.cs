using System;
using System.Collections.Generic;
using System.Text;

namespace EtymoLex.Academy.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class NotRequiredAttribute : Attribute
{
    public NotRequiredAttribute()
    {
        
    }
}

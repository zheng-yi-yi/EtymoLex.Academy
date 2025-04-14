using System.ComponentModel.DataAnnotations;

namespace EtymoLex.Academy.Attributes;

public class OptionalAttribute : ValidationAttribute
{
    public OptionalAttribute()
    {
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        return ValidationResult.Success!;
    }
}

using System;
using System.Collections.Generic;

namespace EtymoLex.Academy.Dtos;

public class ModelingHistoryDto
{
    public Guid? Id { get; set; }
    public string? UserName { get; set; }
    public string? ExecutionTime { get; set; }
    public string? ChangeType { get; set; }
    public List<ModelingPropertyDto> Children { get; set; } = new List<ModelingPropertyDto>();
}

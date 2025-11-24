namespace TenjinX.Mappers.Moq.Tests.Models;

public record TestSourceModel
{
    public string PropertyOne { get; set; } = string.Empty;

    public string PropertyTwo { get; set; } = string.Empty;
}

public record TestDestinationModel
{
    public string Property01 { get; set; } = string.Empty;

    public string Property02 { get; set; } = string.Empty;
}

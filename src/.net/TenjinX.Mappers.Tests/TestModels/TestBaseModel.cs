namespace TenjinX.Mappers.Tests.TestModels;

/// <summary>
/// The test base model.
/// </summary>
public record TestBaseModel
{
    public bool Flag { get; set; }

    public int Number { get; set; }
    
    public string Text { get; set; } = string.Empty;
}

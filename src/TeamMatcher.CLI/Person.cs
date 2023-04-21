using CsvHelper.Configuration.Attributes;

namespace TeamMatcher.CLI;

[Serializable]
public class Person
{
    [Name("name")]
    public string? Name { get; set; }
    
    [Name("kind")]
    public string? Kind { get; set; }
    
    [Name("num")]
    public int Number { get; set; }
    
    [Name("val")]
    public int Value { get; set; }
}

using System.CommandLine;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using CsvHelper;
using TeamMatcher.Lib.Algorithms;
using TeamMatcher.CLI;

static void Print(IReadOnlyCollection<Person?> arr)
{
    var builder = new StringBuilder();
    builder
        .AppendLine("===============");
    arr
        .Aggregate(builder, (b, p) => b.AppendLine(p is null 
            ? "<null>" 
            : $"{p.Name?.ToString() ?? "<null>",4} {p.Number,3} : {p.Value}"))
        .AppendLine("===============")
        .AppendLine($" COUNT : {arr.Count}")
        .AppendLine($"   SUM : {arr.Sum(p => p?.Value)}")
        .AppendLine("===============");
    Console.WriteLine(builder.ToString());
}

var root = new RootCommand("A cross-platform stable team matcher, written in C#.");
var file = new Option<FileInfo>("--csv-file") { IsRequired = true };
var team = new Option<int>("--team-count") { IsRequired = true };
root.AddOption(file);
root.AddOption(team);
root.SetHandler(static (file, team) =>
{
    using var reader = new StreamReader(file.FullName);
    
    var err = false;
    Unsafe.SkipInit<IEnumerable<Person>>(out var records);
    try
    {
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        records = csv.GetRecords<Person>()?.ToArray();
        if (records is null)
            err = true;
    }
    catch
    {
        err = true;
    }
    
    if (err)
    {
        var pre = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("error: invalid csv detected.");
        Console.ForegroundColor = pre;
        return;
    }

    if (team < 1)
    {
        var pre = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("error: team count must be greater than 0.");
        Console.ForegroundColor = pre;
        return;
    }

    if (team > 2)
    {
        var pre = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("warning: if team count is greater than 2, The matching will be done heuristically and may deviate from the best approximation.");
        Console.ForegroundColor = pre;
    }

    var dict = records!.StableShuffle(team, p => p.Kind, p => p.Value);
    foreach (var i in dict)
        Print(i.ToArray());
    
}, file, team);
return root.Invoke(args);

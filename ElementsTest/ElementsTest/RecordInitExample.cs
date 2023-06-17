namespace ElementsTest;

public class RecordInitExample
{
    public string Field { get; init; } = string.Empty;

    public void Write()
    {
        Console.WriteLine(this);
    }
}

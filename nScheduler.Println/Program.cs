using System.Collections;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Write("EnvironmentVariables: ");
        var env = Environment.GetEnvironmentVariables();
        foreach (DictionaryEntry e in env)
        {
            Console.WriteLine($"{e.Key} = {e.Value}");
        }

        Thread.Sleep(1000 * 60 * 5);

        Console.Write("Args: ");
        if (args.Length is var length && length > 0)
        {
            for (int i = 0; i < length; i++)
            {
                Console.WriteLine($"index{i}: {args[i]}");
            }
        }
        else
        {
            Console.WriteLine("No Args");
        }
    }
}
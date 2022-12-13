using System;
using System.IO;
using AdventOfCode2022;

namespace AdventOfCode2022.Cli
{
  internal class ArgException : Exception {
    public ArgException(string s) : base(s) { }
  }

  public static class Program
  {
    public static void Main(string[] args)
    {
      try {
        if(args.Length < 2) throw new ArgException("Bad args");

        var date = int.Parse(args[0]);
        if(date < 0 || date > 25) throw new ArgException("Bad date");

        var part = int.Parse(args[1]);
        if(part < 1 || part > 2) throw new ArgException("Bad part");
        
        var typeName = $"Dec{date:00}";
        var assemblyQualifiedTypeName = $"AdventOfCode2022.{typeName}, AdventOfCode2022";
        var type = Type.GetType(assemblyQualifiedTypeName);
        if(type == null) throw new Exception("type is null");

        var inputFile = (args.Length == 3 && args[2] == "test") ? "test-input.txt" : "input.txt";
        var inputFilePath = Path.Combine(RootFolderName(), "data", typeName.ToLower(), inputFile);

        var day = Activator.CreateInstance(type, inputFilePath);

        var methodName = $"Part{part}";
        var method = type.GetMethod(methodName);
        if(method == null) throw new Exception("method is null");
        var output = method.Invoke(day, null);

        Console.WriteLine(output);
      }
      catch(ArgException e) {
        Console.WriteLine();
        Console.WriteLine(e.Message);
        Console.WriteLine();
        ShowUsage();
      }
      catch(Exception e) {
        Console.WriteLine(e.Message);
        if(e.InnerException != null) Console.WriteLine(e.InnerException.Message);
      }
    }


    private static void ShowUsage()
    {
      Console.WriteLine();
      Console.WriteLine("USAGE:  aoc <date> <part> [test]");
      Console.WriteLine();
      Console.WriteLine("For example, this ...");
      Console.WriteLine("  aoc 13 1 test");
      Console.WriteLine("... will run part 1 of the Dec 13 challenge with test data");
      Console.WriteLine("And this ...");
      Console.WriteLine("  aoc 24 2");
      Console.WriteLine("... will run part 2 of the Dec 24 challenge with full data");
    }

    private static string RootFolderName([System.Runtime.CompilerServices.CallerFilePath] string thisFilePath = "")
    {
      var folder = new FileInfo(thisFilePath).Directory;
      if(folder == null) throw new Exception("This should not happen");
      folder = folder.Parent;
      if(folder == null) throw new Exception("This should not happen");
      return folder.FullName;
    }
  }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace AdventOfCode2022
{
  public class Dec07 : Day
  {
    private enum LineType { Cd, Ls, Dir, File }

    private record File
    {
      public string Name { get; init; }
      public int Size { get; init; }
    }

    private record Directory
    {
      public string Name { get; init; }
      public List<Directory> SubDirectories = new List<Directory>();
      public List<File> Files = new List<File>();
      public int TotalSize { get; set; }
    }

    private record State
    {
      public Stack<Directory> DirectoryStack = new Stack<Directory>();
      public Dictionary<string, Directory> AllDirectories = new Dictionary<string, Directory>();

      public State()
      {
        AllDirectories.Add("/", new Directory { Name = "/" });
      }
    }


    public Dec07(string inputFile) : base(inputFile) { }


    public string Part1()
    {
      var state = CalcFinalState(_lines);
      var result = state.AllDirectories.Values
        .Where(dir => dir.TotalSize <= 100000)
        .Select(dir => dir.TotalSize)
        .Sum();
      return $"The total size of all directories (<= 100,000 bytes) is {result}";
    }

    public string Part2()
    {
      var state = CalcFinalState(_lines);
      var rootDirTotalSize = state.AllDirectories["/"].TotalSize;
      var smallestDir = state.AllDirectories.Values
        .Where(dir => dir.TotalSize >= 30000000 - (70000000 - rootDirTotalSize))
        .OrderBy(dir => dir.TotalSize)
        .First();
      var s = $"The smallest dir that, when deleted, would free up enough space is {smallestDir.Name}";
      s += Environment.NewLine;
      s += $"The size of this directory is {smallestDir.TotalSize}";
      return s;
    }


    private State CalcFinalState(IEnumerable<string> _lines)
    {
      return _lines
      .Aggregate(
        new State(),
        (state, line) => ProcessLine(state, line),
        (state) =>
        {
          //Debug(state);
          CalculateTotalSizeByFolder(state);
          Debug(state);
          return state;
        });
    }

    private State ProcessLine(State state, string line)
    {
      var tokens = new List<string>();
      var type = ParseLine(line, tokens);
      switch(type) {
      case LineType.Cd:
        ProcessCd(state, tokens[0]);
        break;
      case LineType.Ls:
        break;
      case LineType.Dir:
        ProcessDir(state, tokens[0]);  
        break;
      case LineType.File:
        ProcessFile(state, tokens[1], int.Parse(tokens[0]));  
        break;
      }
      return state;
    }

    private void ProcessCd(State state, string dirName)
    {
      if(dirName == "..") {
        state.DirectoryStack.Pop();
        return;
      }
      if(dirName != "/") dirName += "/";
      var fullDirName = GetFullDirName(state, dirName);
      var dir = state.AllDirectories[fullDirName];
      state.DirectoryStack.Push(dir);
    }

    private void ProcessDir(State state, string dirName)
    {
      dirName += "/";
      var currentDir = state.DirectoryStack.Peek();
      var newDir = new Directory { Name = dirName };
      currentDir.SubDirectories.Add(newDir);
      var fullDirName = GetFullDirName(state, dirName);
      state.AllDirectories.Add(fullDirName, newDir);
    }

    private void ProcessFile(State state, string fileName, int fileSize)
    {
      var currentDir = state.DirectoryStack.Peek();
      currentDir.Files.Add(new File { Name = fileName, Size = fileSize });
    }

    private string GetFullDirName(State state, string dirName)
    {
      var fullDirName = string.Join("", state.DirectoryStack.Select(d => d.Name).Reverse()) + dirName;
      return fullDirName;
    }

    private LineType ParseLine(string line, List<string> tokens)
    {
      const string cdPattern = @"^\$ cd ([\/a-zA-Z0-9.]+)$";
      const string lsPattern = @"^\$ ls$";
      const string dirPattern = @"^dir ([\/a-zA-Z0-9.]+)$";
      const string filePattern = @"^(\d+)\s+([a-zA-Z0-9.]+)$";

      if(RegexTest(line, cdPattern, tokens)) return LineType.Cd;
      if(RegexTest(line, lsPattern, tokens)) return LineType.Ls;
      if(RegexTest(line, dirPattern, tokens)) return LineType.Dir;
      if(RegexTest(line, filePattern, tokens)) return LineType.File;

      throw new Exception("Unknown pattern, line: '{line}'");
    }

    private bool RegexTest(string line, string pattern, List<string> tokens)
    {
      var match = Regex.Match(line, pattern);
      if(match.Success) {
        for(var n = 1; n < match.Groups.Count; ++n) {
          var group = match.Groups[n];
          tokens.Add(group.Captures[0].Value);
        }
        return true;
      }
      return false;
    }

    private void Debug(State state)
    {
      PrintFileSystem(state.AllDirectories, "/", "/", 0);
    }

    private void CalculateTotalSizeByFolder(State state)
    {
      CalculateTotalSize(state.AllDirectories, "/", "/");
    }

    private void CalculateTotalSize(Dictionary<string, Directory> allDirs, string dirName, string fullDirName)
    {
      var dir = allDirs[fullDirName];
      foreach(var subDir in dir.SubDirectories) {
        CalculateTotalSize(allDirs, subDir.Name, fullDirName + subDir.Name);
        dir.TotalSize += subDir.TotalSize;
      }
      foreach(var file in dir.Files)
        dir.TotalSize += file.Size;
    }

    private void PrintFileSystem(Dictionary<string, Directory> allDirs, string dirName, string fullDirName, int level)
    {
      var padding = "  ".Repeat(level);
      var padding2 = "  ".Repeat(level + 1);
      var dir = allDirs[fullDirName];
      Console.WriteLine($"{padding}- {dirName} (dir, size={dir.TotalSize})");
      foreach(var subDir in dir.SubDirectories)
        PrintFileSystem(allDirs, subDir.Name, fullDirName + subDir.Name, ++level);
      foreach(var file in dir.Files)
        Console.WriteLine($"{padding2}- {file.Name} (file, size={file.Size})");
    }
  }
}

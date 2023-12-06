using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace AdventOfCode2022
{
  public class Dec05 : Day
  {
    private enum Mode { InitStacks, MutateStacks };

    private class State
    {
      public Mode Mode;
      public readonly List<string> InitInfo;
      public readonly List<Stack<char>> Stacks;

      public State()
      {
        Mode = Mode.InitStacks;
        InitInfo = new List<string>();
        Stacks = new List<Stack<char>>();
      }
    }

    public Dec05(string inputFile) : base(inputFile) { }

    public string Part1()
    {
      return PartN(MutateStacks);
    }

    public string Part2()
    {
      return PartN(MutateStacks2);
    }


    private string PartN(Action<List<Stack<char>>, string> mutateStacksFn)
    {
      var result = _lines
        .Aggregate(
          new State(),
          (state, s) =>
          {
            switch(state.Mode) {
            case Mode.InitStacks:
              if(s.Length == 0) {
                InitStacks(state.Stacks, RotateRight(Convert(state.InitInfo)));
                state.Mode = Mode.MutateStacks;
              }
              else {
                state.InitInfo.Add(s);
              }
              break;
            case Mode.MutateStacks:
              mutateStacksFn(state.Stacks, s);
              break;
            }
            return state;
          },
          state => state.Stacks
        )
        .Aggregate(
          new StringBuilder(),
          (sb, stack) => { if(stack.Count > 0) sb.Append(stack.Pop()); return sb; },
          sb => sb.ToString()
        );

      return $"The top crates on the stacks from left to right = {result}";
    }

    private void InitStacks(List<Stack<char>> stacks, char[,] matrix)
    {
      var rows = matrix.GetLength(0);
      var cols = matrix.GetLength(1);
      for(var row = 0; row < rows; ++row) {
        if(matrix[row, 0] == ' ') continue;
        var stack = new Stack<char>();
        stacks.Add(stack);
        for(var col = 1; col < cols; ++col) {
          var c = matrix[row, col];
          if(c == ' ') break;
          stack.Push(c);
        }
      }
    }

    private static char[,] RotateRight(char[,] matrix)
    {
      //
      // 0,0 0,1 0,2 0,3 -> 2,0 1,0 0,0
      // 1,0 1,1 1,2 1,3    2,1 1,1 0,1
      // 2,0 2,1 2,2 2,3    2,2 1,2 0,2
      //                    2,3 1,3 0,3
      //
      var rows = matrix.GetLength(0);
      var cols = matrix.GetLength(1);
      var result = new char[cols, rows];
      for(int row = 0; row < rows; ++row)
        for(int col = 0; col < cols; ++col)
          result[col, rows - row - 1] = matrix[row, col];
      return result;
    }

    private static char[,] Convert(List<string> initInfo)
    {
      var rows = initInfo.Count;
      var cols = initInfo[0].Length;
      var result = new char[rows, cols];
      for(var row = 0; row < rows; ++row)
        for(var col = 0; col < cols; ++col)
          result[row, col] = initInfo[row][col];
      return result;
    }

    private static void MutateStacks(List<Stack<char>> stacks, string s)
    {
      string pattern = @"^\s*move\s+(\d+)\s+from\s+(\d+)\s+to\s+(\d+)\s*$";
      var match = Regex.Match(s, pattern);
      var count = int.Parse(match.Groups[1].Captures[0].Value);
      var sourceStack = int.Parse(match.Groups[2].Captures[0].Value);
      var targetStack = int.Parse(match.Groups[3].Captures[0].Value);

      for(var n = 0; n < count; ++n)
        stacks[targetStack - 1].Push(stacks[sourceStack - 1].Pop());
    }

    private static void MutateStacks2(List<Stack<char>> stacks, string s)
    {
      string pattern = @"^\s*move\s+(\d+)\s+from\s+(\d+)\s+to\s+(\d+)\s*$";
      var match = Regex.Match(s, pattern);
      var count = int.Parse(match.Groups[1].Captures[0].Value);
      var sourceStack = int.Parse(match.Groups[2].Captures[0].Value);
      var targetStack = int.Parse(match.Groups[3].Captures[0].Value);

      var temp = new Stack<char>();
      for(var n = 0; n < count; ++n)
        temp.Push(stacks[sourceStack - 1].Pop());
      for(var n = 0; n < count; ++n)
        stacks[targetStack - 1].Push(temp.Pop());
    }

    private static string MatrixToString(char[,] matrix)
    {
      var rows = matrix.GetLength(0);
      var cols = matrix.GetLength(1);
      var sb = new StringBuilder();
      for(var row = 0; row < rows; ++row) {
        for(var col = 0; col < cols; ++col) {
          var c = matrix[row, col];
          sb.Append(c);
        }
        sb.Append(Environment.NewLine);
      }
      return sb.ToString();
    }
  }
}

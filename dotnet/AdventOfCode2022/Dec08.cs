using System.Linq;
using System.Collections.Generic;
using System;

namespace AdventOfCode2022
{
  public class Dec08 : Day
  {
    private class Tree
    {
      public int Value { get; init; }
      public int Row { get; init; }
      public int Col { get; init; }
    }

    private class State
    {
      public IList<IList<Tree>> TreeRows = new List<IList<Tree>>();
      public IList<IList<Tree>> TreeCols = new List<IList<Tree>>();
      public int Row = 0;
    }

    private class LineState
    {
      public State State;
      public int Col;
      public LineState(State state, int col) { State = state; Col = col; }
    }

    public Dec08(string inputFile) : base(inputFile) { }


    public string Part1()
    {
      var state = GenerateState(_lines);
      PrintState(state);
      Console.WriteLine();

      var numVisibleTrees = state.TreeRows
        .Select(trees => trees.Where(tree => IsVisible(tree, state)).Count())
        .Sum();

      return $"The number of visible trees = {numVisibleTrees}";
    }

    public string Part2()
    {
      var state = GenerateState(_lines);
      PrintForest(state);
      Console.WriteLine();

      var mostScenic = state.TreeRows
        .Select(
          trees => trees.Select(
            tree => (tree, CalcScenicScore(tree, state)))
            .OrderByDescending(tuple => tuple.Item2.Item1)
            .First())
        .OrderByDescending(tuple => tuple.Item2.Item1)
        .First();

      var tree = mostScenic.Item1;
      var scoreInfo = mostScenic.Item2;
      var score = scoreInfo.Item1;
      var info = scoreInfo.Item2;
      var scoreString = $"{score}:^{info.Item1}>{info.Item2}v{info.Item3}<{info.Item4}";

      return $"Maximum scenic score = {scoreString} for the tree [{tree.Row},{tree.Col}]";
    }

    private static State GenerateState(IEnumerable<string> lines)
    {
      return lines
        .Select(line => line.ToCharArray().Select(c => (int)(c - '0')))
        .Aggregate(
          new State(),
          (state, nums) => {
            if(state.TreeCols.Count == 0)
              foreach(var n in nums)
                state.TreeCols.Add(new List<Tree>());

            var trees = new List<Tree>();
            state.TreeRows.Add(trees);
            nums.Aggregate(
              new LineState(state, 0),
              (ls, n) => {
                var tree = new Tree {
                  Value = n,
                  Row = ls.State.Row,
                  Col = ls.Col
                };
                trees.Add(tree);
                state.TreeCols[ls.Col].Add(tree);
                ls.Col++;
                return ls;
              });
            state.Row++;
            return state;
          },
          state => state);
    }

    private bool IsVisible(Tree tree, State state)
    {
      if(tree.Row == 0 || tree.Row == state.TreeRows.Count - 1) return true;
      if(tree.Col == 0 || tree.Col == state.TreeCols.Count - 1) return true;

      var row = state.TreeRows[tree.Row];
      var col = state.TreeCols[tree.Col];

      var maxHeightWest = row.TakeWhile(t => t.Col < tree.Col).Select(t => t.Value).Max();
      var maxHeightNorth = col.TakeWhile(t => t.Row < tree.Row).Select(t => t.Value).Max();
      var maxHeightEast = row.SkipWhile(t => t.Col <= tree.Col).Select(t => t.Value).Max();
      var maxHeightSouth = col.SkipWhile(t => t.Row <= tree.Row).Select(t => t.Value).Max();

      if(tree.Value > maxHeightNorth) return true;
      if(tree.Value > maxHeightEast) return true;
      if(tree.Value > maxHeightSouth) return true;
      if(tree.Value > maxHeightWest) return true;

      return false;
    }

    private Tuple<int, Tuple<int, int, int, int>> CalcScenicScore(Tree tree, State state)
    {
      var row = state.TreeRows[tree.Row];
      var col = state.TreeCols[tree.Col];

      var toNorth = col.TakeWhile(t => t.Row < tree.Row).Reverse().ToList();
      var toEast = row.SkipWhile(t => t.Col <= tree.Col).ToList();
      var toSouth = col.SkipWhile(t => t.Row <= tree.Row).ToList();
      var toWest = row.TakeWhile(t => t.Col < tree.Col).Reverse().ToList();

      var scoreToNorth = CalcScenicScore(toNorth, tree);
      var scoreToEast = CalcScenicScore(toEast, tree);
      var scoreToSouth = CalcScenicScore(toSouth, tree);
      var scoreToWest = CalcScenicScore(toWest, tree);

      var score = scoreToNorth * scoreToEast * scoreToSouth * scoreToWest;
      return new Tuple<int, Tuple<int, int, int, int>>(
        score,
        new Tuple<int, int, int, int>(scoreToNorth, scoreToEast, scoreToSouth, scoreToWest));
    }

    private int CalcScenicScore(IList<Tree> trees, Tree tree)
    {
      var score = 0;
      for(var n = 0; n < trees.Count; ++n) {
        if(n > 0 && trees[n - 1].Value == tree.Value) break;
        ++score;
        if(trees[n].Value > tree.Value) break;
      }
      return score;
    }

    private static void PrintForest(State state)
    {
      foreach(var row in state.TreeRows) {
        foreach(var tree in row) {
          Console.Write($"[{tree.Row},{tree.Col}]:{tree.Value} ");
        }
        Console.WriteLine();
      }
    }

    private static void PrintState(State state)
    {
      Console.WriteLine();
    }
  }
}

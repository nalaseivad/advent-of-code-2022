using System;
using System.Collections.Generic;
using System.Linq;


namespace AdventOfCode2022
{
  public class Dec02 : Day
  {
    private enum Move { Rock, Paper, Scissors };
    private enum Outcome { Lose, Draw, Win };
    private class Round { public Move TheirMove; public Move MyMove; }
    private class RoundOutcome { public Move TheirMove; public Outcome Outcome; }


    public Dec02(string inputFile) : base(inputFile) { }


    public string Part1()
    {
      var score = _lines
        .Select(s => s.Split(" "))
        .Select(pair => pair.Select(s => char.Parse(s)))
        .Select(pair => new Round { TheirMove = LeftCharToMove(pair.First()), MyMove = RightCharToMove(pair.Last()) })
        .Select(round => CalcMyScore(round))
        .Sum();
      return $"Total score = {score}";
    }

    public string Part2()
    {
      var score = _lines
        .Select(s => s.Split(" "))
        .Select(pair => pair.Select(s => char.Parse(s)))
        .Select(pair => new RoundOutcome {
          TheirMove = LeftCharToMove(pair.First()),
          Outcome = CharToOutcome(pair.Last())
        })
        .Select(roundOutcome => new Round {
          TheirMove = roundOutcome.TheirMove,
          MyMove = CalcMyMove(roundOutcome)
        })
        .Select(moves => CalcMyScore(moves))
        .Sum();
      return $"Total score = {score}";
    }


    private int CalcMyScore(Round round)
    {
      int score = (int)round.MyMove + 1;
      int[,] map = new int[,] {
        { 3, 6, 0 },   // Rock v Rock, Rock v Paper, Rock v Scissors
        { 0, 3, 6 },   // Paper v Rock, Paper v Paper, Paper v Scissors
        { 6, 0, 3 }    // Scissors v Rock, Scissors v Paper, Scissors v Scissors
      };
      score += map[(int)round.TheirMove, (int)round.MyMove];
      return score;
    }

    private Move CalcMyMove(RoundOutcome roundOutcome)
    {
      int[,] map = new int[,] {
        { 2, 0, 1 },   // Rock + Lose = Scissors, Rock + Draw = Rock, Rock + Win = Paper
        { 0, 1, 2 },   // Paper + Lose = Rock, Paper + Draw = Paper, Paper + Win = Scissors
        { 1, 2, 0 }    // Scissors + Lose = Paper, Scissors + Draw = Scissors, Scissors + Win = Rock
      };
      return (Move)map[(int)roundOutcome.TheirMove, (int)roundOutcome.Outcome];
    }

    private Move CharToMove(char c, List<char> chars)
    {
      return (Move)chars.FindIndex(x => x == c);
    }

    private Move LeftCharToMove(char c)
    {
      return CharToMove(c, new List<char> { 'A', 'B', 'C' });
    }

    private Move RightCharToMove(char c)
    {
      return CharToMove(c, new List<char> { 'X', 'Y', 'Z' });
    }

    private Outcome CharToOutcome(char c)
    {
      return (Outcome)(new List<char> { 'X', 'Y', 'Z' }.FindIndex(x => x == c));
    }
  }
}

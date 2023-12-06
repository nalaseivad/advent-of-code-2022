using System;
using System.Collections.Generic;
using System.Linq;


namespace AdventOfCode2022
{
  public class Dec01 : Day
  {
    private class Elf { public int Index; public int Total; }


    public Dec01(string inputFile) : base(inputFile) { }


    public string Part1()
    {
      var elf = GetElves(1).First();
      return $"elf = {elf.Index}, Total = {elf.Total}";
    }

    public string Part2()
    {
      var elves = GetElves(3);
      return $"elves = [{String.Join(",", elves.Select(x => x.Index))}], Total = {elves.Select(x => x.Total).Sum()}";
    }


    private IEnumerable<Elf> GetElves(int count)
    {
      return _lines
        .Select(s => s == "" ? -1 : int.Parse(s))
        .Split(n => n == -1)
        .Select((list, index) => new Elf { Index = index, Total = list.Sum() })
        .OrderByDescending(x => x.Total)
        .Take(count);
    }
  }
}

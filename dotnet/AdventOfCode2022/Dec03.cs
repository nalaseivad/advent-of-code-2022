using System.Collections.Generic;
using System.Linq;


namespace AdventOfCode2022
{
  public class Dec03 : Day
  {
    public Dec03(string inputFile) : base(inputFile) { }


    public string Part1()
    {
      var result = _lines
        .Select(s => s.ToCharArray())                             // Split each line into a char array.
        .Select(a => {                                            // Split items into compartments.
          var n = a.Length / 2;
          return ( a.Take(n), a.Skip(n).Take(n) );
        })
        .Select(t => t.Item1.Intersect(t.Item2))                  // Intersect the compartments to find the common items
        .Select(a => a.First())                                   // There should only be one common item
        .Select(c => (c, c > 'Z' ? c - 'a' + 1 : c - 'A' + 27))   // Calc item priority and add into tuple with item.
        .Select(t => t.Item2)                                     // Reduce to just the priorities.
        .Sum();

      return $"Sum of priorities of commom items = {result}";
    }

    public string Part2()
    {
      var result = _lines
        .Select(s => s.ToCharArray())                             // Split each line into a char array.
        .Partition(3)                                             // Partition into groups of three lines.
        .Select(listOfLists => listOfLists                        // For each 3 line group do a 3 way intersection.
          .Skip(1)                                                //   Aggregate over the lists using a HashSet.
          .Aggregate(                                             //   Initialize the HashSet with the first list
            new HashSet<char>(listOfLists.First()),               //   but then skip that list in the aggregation.
            (h, e) => { h.IntersectWith(e); return h; }           //   This is what the Skip(1) is for.
          )
          .First()                                                // There should only be one common item (the badge)
        )
        .Select(c => (c, c > 'Z' ? c - 'a' + 1 : c - 'A' + 27))   // Calc badge priority and add into tuple with badge
        .Select(t => t.Item2)                                     // Reduce to just the priorities 
        .Sum();

      return $"Sum of priorities of badges of 3 elf groups = {result}";
    }
  }
}

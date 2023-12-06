using System.Linq;


namespace AdventOfCode2022
{
  public class Dec04 : Day
  {
    private class Range {
      public readonly int Start;
      public readonly int End;

      public Range(string s)
      {
        var pair = s.Split("-");
        Start = int.Parse(pair[0]);
        End = int.Parse(pair[1]);
      }
    }


    public Dec04(string inputFile) : base(inputFile) { }


    public string Part1()
    {
      var result = _lines
        .Select(s => { var pair = s.Split(","); return (pair[0], pair[1]); })
        .Select(t => ( new Range(t.Item1), new Range(t.Item2) ))
        .Where(t => RangesOverlapFully(t.Item1, t.Item2))
        .Count();

      return $"The number of assignment pairs where one range fully contains the other = {result}";
    }

    public string Part2()
    {
      var result = _lines
        .Select(s => { var pair = s.Split(","); return (pair[0], pair[1]); })
        .Select(t => (new Range(t.Item1), new Range(t.Item2)))
        .Where(t => RangesOverlap(t.Item1, t.Item2))
        .Count();

      return $"The number of assignment pairs where one range fully contains the other = {result}";
    }

    private static bool RangesOverlapFully(Range r1, Range r2)
    {
      // r1: [      ]
      // r2:   [  ]
      // or
      // r1:   [   ]
      // r2: [      ]
      return r1.Start >= r2.Start && r1.End <= r2.End || r2.Start >= r1.Start && r2.End <= r1.End;
    }

    private static bool RangesOverlap(Range r1, Range r2)
    {
      // r1: [   ]
      // r2:   [    ]
      // or
      // r1:   [   ]
      // r2: [    ]
      // but not ...
      // r1: [   ]
      // r2:       [    ]
      // or
      // r1:        [   ]
      // r2: [    ]
      // or
      // r1:        [   ]
      // r2: [    ]
      // or
      // r1:  [   ]
      // r2:         [    ]
      return r1.Start <= r2.End && r1.End >= r2.Start || r2.Start <= r1.End && r2.End >= r1.Start;
    }
  }
}

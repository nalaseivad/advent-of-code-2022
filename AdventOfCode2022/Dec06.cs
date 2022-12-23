using System.Linq;


namespace AdventOfCode2022
{
  public class Dec06 : Day
  {
    public Dec06(string inputFile) : base(inputFile) { }


    public string Part1()
    {
      return PartN(4);
    }

    public string Part2()
    {
      return PartN(14);
    }


    private string PartN(int count)
    {
      var result = _lines
        .First()
        .Partition(count, 1)
        .TakeWhile(chars => chars.Distinct().Count() != count)
        .Count() + count;
      return $"{result} characters need to be processed before the first start-of-packet marker is detected";
    }
  }
}

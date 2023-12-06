using System;
using System.Collections.Generic;
using System.IO;


namespace AdventOfCode2022
{
  public class Day
  {
    protected string _inputFile;
    protected IEnumerable<string> _lines;


    public Day(string inputFile)
    {
      _inputFile = inputFile;
      _lines = File.ReadLines(inputFile);
    }
  }
}

using System.Text;

namespace AdventOfCode2022
{
  public static class StringExtensions
  {
    //
    // Repeat a string N times
    //
    public static string Repeat(this string s, int count)
    {
      if(count == 0) return "";

      var builder = new StringBuilder(s.Length * count);
      for(var n = 0; n < count; ++n) builder.Append(s);
      return builder.ToString();
   }
  }
}
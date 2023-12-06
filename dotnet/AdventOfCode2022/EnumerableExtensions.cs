using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2022
{
  public static class EnumerableExtensions
  {
    //
    // Split a list into sublists based on a test for whether elements are considered separators.
    // e.g.
    //  ["a", "b", "", "c", "", "d", "e", "f", "", "", "g"].Split(x => x.Length == 0)
    //  =>
    //  [["a", "b"], ["d", "e", "f"], ["g"]]
    //
    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, Func<T, bool> separatorFn)
    {
      var list = new List<T>();
      foreach(var element in source) {
        if(separatorFn(element)) {
          yield return list;
          list.Clear();
          continue;
        }
        list.Add(element);
      }
      if(list.Count > 0) yield return list;
    }

    //
    // Return the mode (most common item) of a collection.
    // If there is more than one item with the same frequency then return them all.
    // e.g. [1, 2, 2, 3, 4, 4, 5].AllModes() => [2, 3]
    //
    public static IEnumerable<int> AllModes(this IEnumerable<int> collection)
    {
      var pairs = collection
        .GroupBy(value => value)
        .OrderByDescending(group => group.Count());
      int modeCount = pairs.First().Count();
      return pairs
        .Where(pair => pair.Count() == modeCount)
        .Select(pair => pair.Key)
        .ToList();
    }

    //
    // Partition a list into a list of sublists of a certain size.  The last list may be incomplete.
    // e.g. [1, 2, 3, 4, 5, 6, 7].Partition(2) => [[1, 2], [3, 4], [5, 6], [7]]
    //
    public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int bucketSize)
    {
      if(source == null) throw new ArgumentNullException(nameof(source));
      if(bucketSize <= 0) throw new ArgumentOutOfRangeException(nameof(bucketSize));

      return PartitionImpl(source, bucketSize, bucketSize);
    }

    //
    // Partition a list into a list of sublists with a specific offset.  The last list may be incomplete.
    // e.g. [1, 2, 3, 4, 5, 6].Partition(3, 2) => [[1, 2, 3], [3, 4, 5], [5, 6]]
    //
    public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int bucketSize, int offset)
    {
      if(source == null) throw new ArgumentNullException(nameof(source));
      if(bucketSize <= 0) throw new ArgumentOutOfRangeException(nameof(bucketSize));
      if(offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));

      return PartitionImpl(source, bucketSize, offset);
    }


    private static IEnumerable<IEnumerable<T>> PartitionImpl<T>(this IEnumerable<T> source, int bucketSize, int offset)
    {
      var buckets = new Queue<IList<T>>();
      var n = 0;
      foreach(var item in source) {
        if(n % offset == 0) buckets.Enqueue(new List<T>(bucketSize));
        foreach(var bucket in buckets) bucket.Add(item);
        if(buckets.Peek().Count == bucketSize) yield return buckets.Dequeue();
        ++n;
      }
      while(buckets.Count > 0) yield return buckets.Dequeue();
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq
{
   public class KeyEqualityComparer<T> : IEqualityComparer<T>
   {
      private readonly Func<T, T, bool> comparer;
      private readonly Func<T, object> keyExtractor;

      // Enable to only specify the key to compare with: y => y.CustomerID
      public KeyEqualityComparer(Func<T, object> keyExtractor) : this(keyExtractor, null) { }
      // Enable to specify how to tel if two objects are equal: (x, y) => y.CustomerID == x.CustomerID
      public KeyEqualityComparer(Func<T, T, bool> comparer) : this(null, comparer) { }

      public KeyEqualityComparer(Func<T, object> keyExtractor, Func<T, T, bool> comparer)
      {
         this.keyExtractor = keyExtractor;
         this.comparer = comparer;
      }

      public bool Equals(T x, T y)
      {
         if (comparer != null)
            return comparer(x, y);
         else
         {
            var valX = keyExtractor(x);
            if (valX is IEnumerable<object>) // The special case where we pass a list of keys
               return ((IEnumerable<object>)valX).SequenceEqual((IEnumerable<object>)keyExtractor(y));

            return valX.Equals(keyExtractor(y));
         }
      }

      public int GetHashCode(T obj)
      {
         if (keyExtractor == null)
            return obj.ToString().ToLower().GetHashCode();
         else
         {
            var val = keyExtractor(obj);
            if (val is IEnumerable<object>) // The special case where we pass a list of keys
               return (int)((IEnumerable<object>)val).Aggregate((x, y) => x.GetHashCode() ^ y.GetHashCode());

            return val.GetHashCode();
         }
      }
   }

   public class Comparer<T> : IComparer<T>
   {
      private readonly Func<T, T, int> _comparer;

      public Comparer(Func<T, T, int> comparer)
      {
         if (comparer == null)
            throw new ArgumentNullException("comparer");
         _comparer = comparer;
      }

      public int Compare(T x, T y)
      {
         return _comparer(x, y);
      }
   }

   public static class GeneralExtensions
   {
      public static IEnumerable<T> Distinct<T>(this IEnumerable<T> list, Func<T, object> keyExtractor)
      {
         return list.Distinct(new KeyEqualityComparer<T>(keyExtractor));
      }

      public static bool Contains<T>(this IEnumerable<T> list, T item, Func<T, object> keyExtractor)
      {
         return list.Contains(item, new KeyEqualityComparer<T>(keyExtractor));
      }

      public static IEnumerable<T> Except<T>(this IEnumerable<T> list, IEnumerable<T> except, Func<T, object> keyExtractor)
      {
         return list.Except(except, new KeyEqualityComparer<T>(keyExtractor));
      }

      public static IEnumerable<T> Intersect<T>(this IEnumerable<T> list, IEnumerable<T> toIntersect, Func<T, object> keyExtractor)
      {
         return list.Intersect(toIntersect, new KeyEqualityComparer<T>(keyExtractor));
      }

      public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> list, Func<T, TKey> keySelector, Func<TKey, TKey, int> comparer)
      {
         return list.OrderBy(keySelector, new Comparer<TKey>(comparer));
      }

      public static IOrderedEnumerable<T> OrderByDescending<T, TKey>(this IEnumerable<T> list, Func<T, TKey> keySelector, Func<TKey, TKey, int> comparer)
      {
         return list.OrderByDescending(keySelector, new Comparer<TKey>(comparer));
      }

      public static bool SequenceEqual<T>(this IEnumerable<T> list, IEnumerable<T> sequenceToEqual, Func<T, object> keyExtractor)
      {
         return list.SequenceEqual(sequenceToEqual, new KeyEqualityComparer<T>(keyExtractor));
      }

      public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, object> keyExtractor)
      {
         return first.Union(second, new KeyEqualityComparer<TSource>(keyExtractor));
      }

      public static IEnumerable<T> ToEnumerable<T>(this T o)
      {
         return new List<T>() { o };
      }

      public static bool IsNullOrEmpty(this string value)
      {
         return string.IsNullOrEmpty(value);
      }

      public static string FormatIt(this string val, params object[] args)
      {
         return string.Format(val, args);
      }
   }
}

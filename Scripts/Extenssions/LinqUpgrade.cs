using System;
using System.Collections.Generic;

namespace Extenssions {
	public static class LinqUpgrade {
		public static ICollection<TSource> Where<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate, int capacity) {
			if (source == null) throw new NullReferenceException();
			if (capacity < 0) throw new ArgumentOutOfRangeException($"Capacity {capacity} < 0!");
			
			var result = new TSource[capacity];
			var iteration = 0;

			foreach (var item in source) {
				if (predicate(item) == false) continue;

				result[iteration] = item;
				iteration++;
				if (iteration == capacity) break;
			}

			return result;
		}

		/// <summary>
		/// Check all items for null! This method faster than LINQ ToArray
		/// </summary>
		/// <param name="source"></param>
		/// <param name="capacity"></param>
		/// <typeparam name="TSource"></typeparam>
		/// <returns></returns>
		/// <exception cref="NullReferenceException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source, int capacity) {
			if (source == null) throw new NullReferenceException();
			if (capacity < 0) throw new ArgumentOutOfRangeException($"Capacity {capacity} < 0!");
			
			var result = new TSource[capacity];

			var iteration = 0;
			foreach (var item in source) {
				result[iteration++] = item;
			}

			return result;
		}
		
		/// <summary>
		/// This method faster than LINQ ToList
		/// </summary>
		/// <param name="source"></param>
		/// <param name="capacity"></param>
		/// <typeparam name="TSource"></typeparam>
		/// <returns></returns>
		/// <exception cref="NullReferenceException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source, int capacity) {
			if (source == null) throw new NullReferenceException();
			if (capacity < 0) throw new ArgumentOutOfRangeException($"Capacity {capacity} < 0!");
			
			var result = new List<TSource>(capacity);

			var iteration = 0;
			foreach (var item in source) {
				result[iteration++] = item;
			}

			if (result[^1] != null) return result;
			
			if (result[0] == null) {
				result.Clear();
			} else {
				for (var index = result.Count - 1; index >= 0; index--) {
					if (result[index] == null) result.RemoveAt(index);
				}
			}
			
			return result;
		}

		private static readonly Random random = new();
		public static TSource GetRandomItem<TSource>(this IList<TSource> source) {
			return source[random.Next(0, source.Count)];
		}
	}
}
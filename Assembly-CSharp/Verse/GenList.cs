using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Verse
{
	// Token: 0x0200007D RID: 125
	public static class GenList
	{
		// Token: 0x06000529 RID: 1321 RVA: 0x0001E473 File Offset: 0x0001C673
		public static int CountAllowNull<T>(this IList<T> list)
		{
			if (list == null)
			{
				return 0;
			}
			return list.Count;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001E480 File Offset: 0x0001C680
		public static bool NullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001E490 File Offset: 0x0001C690
		public static bool HasData<T>(this IList<T> list)
		{
			return list != null && list.Count >= 1;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001E4A4 File Offset: 0x0001C6A4
		public static List<T> ListFullCopy<T>(this List<T> source)
		{
			List<T> list = new List<T>(source.Count);
			for (int i = 0; i < source.Count; i++)
			{
				list.Add(source[i]);
			}
			return list;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001E4DC File Offset: 0x0001C6DC
		public static List<T> ListFullCopyOrNull<T>(this List<T> source)
		{
			if (source == null)
			{
				return null;
			}
			return source.ListFullCopy<T>();
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001E4EC File Offset: 0x0001C6EC
		public static void ClearValueLists<T, J>(this Dictionary<T, List<J>> source)
		{
			foreach (KeyValuePair<T, List<J>> keyValuePair in source)
			{
				keyValuePair.Value.Clear();
			}
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001E540 File Offset: 0x0001C740
		public static void ClearAndPoolValueLists<T, V>(this Dictionary<T, List<V>> source)
		{
			foreach (List<V> list in source.Values)
			{
				list.Clear();
				SimplePool<List<V>>.Return(list);
			}
			source.Clear();
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001E59C File Offset: 0x0001C79C
		public static void RemoveDuplicates<T>(this List<T> list, Func<T, T, bool> comparer = null) where T : class
		{
			if (list.Count <= 1)
			{
				return;
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				for (int j = 0; j < i; j++)
				{
					if ((comparer == null && list[i] == list[j]) || (comparer != null && comparer(list[i], list[j])))
					{
						list.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001E611 File Offset: 0x0001C811
		public static void TruncateToLength<T>(this List<T> list, int maxLength)
		{
			if (list.Count == 0 || list.Count <= maxLength)
			{
				return;
			}
			list.RemoveRange(maxLength, list.Count - maxLength);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001E634 File Offset: 0x0001C834
		public static void Shuffle<T>(this IList<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int num = Rand.RangeInclusive(0, i);
				int index = num;
				int index2 = i;
				T value = list[i];
				T value2 = list[num];
				list[index] = value;
				list[index2] = value2;
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001E688 File Offset: 0x0001C888
		public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison)
		{
			int count = list.Count;
			for (int i = 1; i < count; i++)
			{
				T t = list[i];
				int num = i - 1;
				while (num >= 0 && comparison(list[num], t) > 0)
				{
					list[num + 1] = list[num];
					num--;
				}
				list[num + 1] = t;
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001E6E9 File Offset: 0x0001C8E9
		public static bool NotNullAndContains<T>(this IList<T> list, T element)
		{
			return !list.NullOrEmpty<T>() && list.Contains(element);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001E6FC File Offset: 0x0001C8FC
		public static bool NotNullAndContainsAnyElement<T>(this IList<T> list, IList<T> other)
		{
			if (list.NullOrEmpty<T>() || other.NullOrEmpty<T>())
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (other.Contains(list[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001E740 File Offset: 0x0001C940
		public static bool RemoveAll_IfNotAll<T, ContextType>(this List<T> list, ContextType context, Predicate<ContextType, T> predicate)
		{
			int num = list.Count - 1;
			while (num >= 0 && !predicate(context, list[num]))
			{
				num--;
			}
			if (num < 0)
			{
				return false;
			}
			T item = list[num];
			list.RemoveRange(num, list.Count - num);
			list.RemoveAll(context, predicate, true);
			list.Add(item);
			return true;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001E7A0 File Offset: 0x0001C9A0
		public static int RemoveAll<T, ContextType>(this List<T> list, ContextType context, Predicate<ContextType, T> predicate, bool negatePredicate = false)
		{
			bool flag = !negatePredicate;
			int num = 0;
			while (num < list.Count && predicate(context, list[num]) != flag)
			{
				num++;
			}
			if (num == list.Count)
			{
				return 0;
			}
			for (int i = num + 1; i < list.Count; i++)
			{
				if (predicate(context, list[i]) != flag)
				{
					list[num++] = list[i];
				}
			}
			int num2 = list.Count - num;
			list.RemoveRange(num, num2);
			return num2;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001E828 File Offset: 0x0001CA28
		public static void Swap<T>(this IList<T> list, int a, int b)
		{
			T value = list[b];
			T value2 = list[a];
			list[a] = value;
			list[b] = value2;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001E85D File Offset: 0x0001CA5D
		public static T GetLast<T>(this IList<T> source)
		{
			if (source.Count == 0)
			{
				throw new IndexOutOfRangeException("Attempted to get the last element of a list with zero length.");
			}
			return source[source.Count - 1];
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001E880 File Offset: 0x0001CA80
		public static Span<T> AsSpan<T>(this List<T> list)
		{
			return new Span<T>(list.UnsafeGetBackingArray<T>(), 0, list.Count);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001E894 File Offset: 0x0001CA94
		public static Span<T> AsSpan<T>(this List<T> list, int length)
		{
			return new Span<T>(list.UnsafeGetBackingArray<T>(), 0, length);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001E8A3 File Offset: 0x0001CAA3
		public static ReadOnlySpan<T> AsReadOnlySpan<T>(this List<T> list)
		{
			return new ReadOnlySpan<T>(list.UnsafeGetBackingArray<T>(), 0, list.Count);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001E8B7 File Offset: 0x0001CAB7
		public static void UnsafeSetCount<T>(this List<T> list, int count)
		{
			FieldInfo field = typeof(List<T>).GetField("_size", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field == null)
			{
				throw new InvalidOperationException("Failed to find the backing field for List<T> size.");
			}
			field.SetValue(list, count);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001E8EF File Offset: 0x0001CAEF
		private unsafe static T[] UnsafeGetBackingArray<T>(this List<T> list)
		{
			return UnsafeUtility.As<List<T>, StrongBox<T[]>>(ref list)->Value;
		}
	}
}

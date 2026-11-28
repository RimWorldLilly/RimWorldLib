using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000078 RID: 120
	public static class GenCollection
	{
		// Token: 0x060004A1 RID: 1185 RVA: 0x0001B9D8 File Offset: 0x00019BD8
		public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> enumerable)
		{
			foreach (T item in enumerable)
			{
				hashSet.Add(item);
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001BA24 File Offset: 0x00019C24
		public static bool NullOrEmpty<T>(this HashSet<T> hashSet)
		{
			return hashSet == null || !hashSet.Any<T>();
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001BA34 File Offset: 0x00019C34
		public static bool Empty<T>(this Queue<T> queue)
		{
			return queue.Count == 0;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001BA40 File Offset: 0x00019C40
		public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dest, IDictionary<TKey, TValue> source)
		{
			foreach (KeyValuePair<TKey, TValue> keyValuePair in source)
			{
				dest.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001BA98 File Offset: 0x00019C98
		public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, ICollection<TKey> remove)
		{
			foreach (TKey key in remove)
			{
				if (dict.ContainsKey(key))
				{
					dict.Remove(key);
				}
			}
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001BAEC File Offset: 0x00019CEC
		public static void SetOrAdd<K, V>(this Dictionary<K, V> dict, K key, V value)
		{
			dict[key] = value;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001BAF6 File Offset: 0x00019CF6
		public static void AddDistinct<K, V>(this Dictionary<K, V> dict, K key, V value)
		{
			dict.TryAdd(key, value);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001BB04 File Offset: 0x00019D04
		public static void Increment<K>(this Dictionary<K, int> dict, K key)
		{
			if (!dict.TryAdd(key, 1))
			{
				int num = dict[key];
				dict[key] = num + 1;
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001BB30 File Offset: 0x00019D30
		public static bool SharesElementWith<T>(this IEnumerable<T> source, IEnumerable<T> other)
		{
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				IList<T> list2 = other as IList<T>;
				if (list2 != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						for (int j = 0; j < list2.Count; j++)
						{
							if (EqualityComparer<T>.Default.Equals(list[i], list2[j]))
							{
								return true;
							}
						}
					}
					return false;
				}
			}
			return source.Any(new Func<T, bool>(other.Contains<T>));
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001BBA3 File Offset: 0x00019DA3
		public static IEnumerable<T> InRandomOrder<T>(this IEnumerable<T> source, IList<T> workingList = null)
		{
			GenCollection.<InRandomOrder>d__9<T> <InRandomOrder>d__ = new GenCollection.<InRandomOrder>d__9<T>(-2);
			<InRandomOrder>d__.<>3__source = source;
			<InRandomOrder>d__.<>3__workingList = workingList;
			return <InRandomOrder>d__;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001BBBC File Offset: 0x00019DBC
		public static T RandomElement<T>(this HashSet<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (source.Count == 0)
			{
				Log.Warning("Getting random element from empty hashset.");
				return default(T);
			}
			HashSet<T>.Enumerator enumerator = source.GetEnumerator();
			int num = Rand.Range(0, source.Count);
			T result = default(T);
			while (enumerator.MoveNext())
			{
				if (num == 0)
				{
					result = enumerator.Current;
					break;
				}
				num--;
			}
			enumerator.Dispose();
			return result;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001BC34 File Offset: 0x00019E34
		public static T RandomElement<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			ICollection<T> collection = source as ICollection<T>;
			if (collection != null)
			{
				if (collection.Count == 0)
				{
					Log.Warning("Getting random element from empty collection.");
					return default(T);
				}
				return source.ElementAt(Rand.Range(0, collection.Count));
			}
			else
			{
				List<T> list = source.ToList<T>();
				if (list.Count == 0)
				{
					Log.Warning("Getting random element from empty collection.");
					return default(T);
				}
				return list[Rand.Range(0, list.Count)];
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001BCC0 File Offset: 0x00019EC0
		public static T RandomElementWithFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			T result;
			if (source.TryRandomElement(out result))
			{
				return result;
			}
			return fallback;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001BCDC File Offset: 0x00019EDC
		public static bool TryRandomElement<T>(this IEnumerable<T> source, Predicate<T> predicate, out T result)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				if (list.Count == 0)
				{
					result = default(T);
					return false;
				}
			}
			else
			{
				list = source.ToList<T>();
				if (!list.Any<T>())
				{
					result = default(T);
					return false;
				}
			}
			if (!list.Any((T x) => predicate(x)))
			{
				result = default(T);
				return false;
			}
			result = (from x in list
			where predicate(x)
			select x).RandomElement<T>();
			return true;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001BD74 File Offset: 0x00019F74
		public static bool TryRandomElement<T>(this HashSet<T> source, out T result)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (source.Count == 0)
			{
				result = default(T);
				return false;
			}
			HashSet<T>.Enumerator enumerator = source.GetEnumerator();
			int num = Rand.Range(0, source.Count);
			result = default(T);
			while (enumerator.MoveNext())
			{
				if (num == 0)
				{
					result = enumerator.Current;
					break;
				}
				num--;
			}
			enumerator.Dispose();
			return true;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001BDE4 File Offset: 0x00019FE4
		public static bool TryRandomElement<T>(this IEnumerable<T> source, out T result)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			ICollection<T> collection = source as ICollection<T>;
			if (collection != null)
			{
				if (collection.Count == 0)
				{
					result = default(T);
					return false;
				}
			}
			else
			{
				collection = source.ToList<T>();
				if (!collection.Any<T>())
				{
					result = default(T);
					return false;
				}
			}
			result = collection.RandomElement<T>();
			return true;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001BE40 File Offset: 0x0001A040
		public static T RandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector)
		{
			float num = 0f;
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0f)
					{
						string str = "Negative weight in selector: ";
						string str2 = num2.ToString();
						string str3 = " from ";
						T t = list[i];
						Log.Error(str + str2 + str3 + ((t != null) ? t.ToString() : null));
						num2 = 0f;
					}
					num += num2;
				}
				if (list.Count == 1 && num > 0f)
				{
					return list[0];
				}
			}
			else
			{
				int num3 = 0;
				foreach (T t2 in source)
				{
					num3++;
					float num4 = weightSelector(t2);
					if (num4 < 0f)
					{
						string str4 = "Negative weight in selector: ";
						string str5 = num4.ToString();
						string str6 = " from ";
						T t = t2;
						Log.Error(str4 + str5 + str6 + ((t != null) ? t.ToString() : null));
						num4 = 0f;
					}
					num += num4;
				}
				if (num3 == 1 && num > 0f)
				{
					return source.First<T>();
				}
			}
			if (num <= 0f)
			{
				Log.Error("RandomElementByWeight with totalWeight=" + num.ToString() + " - use TryRandomElementByWeight.");
				T t = default(T);
				return t;
			}
			float num5 = Rand.Value * num;
			float num6 = 0f;
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					float num7 = weightSelector(list[j]);
					if (num7 > 0f)
					{
						num6 += num7;
						if (num6 >= num5)
						{
							return list[j];
						}
					}
				}
			}
			else
			{
				foreach (T t3 in source)
				{
					float num8 = weightSelector(t3);
					if (num8 > 0f)
					{
						num6 += num8;
						if (num6 >= num5)
						{
							return t3;
						}
					}
				}
			}
			return default(T);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0001C08C File Offset: 0x0001A28C
		public static T RandomElementByWeightWithFallback<T>(this IEnumerable<T> source, Func<T, float> weightSelector, T fallback = default(T))
		{
			T result;
			if (source.TryRandomElementByWeight(weightSelector, out result))
			{
				return result;
			}
			return fallback;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001C0A8 File Offset: 0x0001A2A8
		public static bool TryRandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector, out T result)
		{
			IList<T> list = source as IList<T>;
			if (list != null)
			{
				float num = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					float num2 = weightSelector(list[i]);
					if (num2 < 0f)
					{
						string str = "Negative weight in selector: ";
						string str2 = num2.ToString();
						string str3 = " from ";
						T t = list[i];
						Log.Error(str + str2 + str3 + ((t != null) ? t.ToString() : null));
						num2 = 0f;
					}
					num += num2;
				}
				if (list.Count == 1 && num > 0f)
				{
					result = list[0];
					return true;
				}
				if (num == 0f)
				{
					result = default(T);
					return false;
				}
				num *= Rand.Value;
				for (int j = 0; j < list.Count; j++)
				{
					float num3 = weightSelector(list[j]);
					if (num3 > 0f)
					{
						num -= num3;
						if (num <= 0f)
						{
							result = list[j];
							return true;
						}
					}
				}
			}
			IEnumerator<T> enumerator = source.GetEnumerator();
			result = default(T);
			float num4 = 0f;
			while (num4 == 0f && enumerator.MoveNext())
			{
				result = enumerator.Current;
				num4 = weightSelector(result);
				if (num4 < 0f)
				{
					string str4 = "Negative weight in selector: ";
					string str5 = num4.ToString();
					string str6 = " from ";
					T t = result;
					Log.Error(str4 + str5 + str6 + ((t != null) ? t.ToString() : null));
					num4 = 0f;
				}
			}
			if (num4 == 0f)
			{
				result = default(T);
				return false;
			}
			while (enumerator.MoveNext())
			{
				T t2 = enumerator.Current;
				float num5 = weightSelector(t2);
				if (num5 < 0f)
				{
					string str7 = "Negative weight in selector: ";
					string str8 = num5.ToString();
					string str9 = " from ";
					T t = t2;
					Log.Error(str7 + str8 + str9 + ((t != null) ? t.ToString() : null));
					num5 = 0f;
				}
				if (Rand.Range(0f, num4 + num5) >= num4)
				{
					result = t2;
				}
				num4 += num5;
			}
			return true;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001C2EC File Offset: 0x0001A4EC
		public static T RandomElementByWeightWithDefault<T>(this IEnumerable<T> source, Func<T, float> weightSelector, float defaultValueWeight)
		{
			if (defaultValueWeight < 0f)
			{
				Log.Error("Negative default value weight.");
				defaultValueWeight = 0f;
			}
			float num = 0f;
			foreach (T t in source)
			{
				float num2 = weightSelector(t);
				if (num2 < 0f)
				{
					string str = "Negative weight in selector: ";
					string str2 = num2.ToString();
					string str3 = " from ";
					T t2 = t;
					Log.Error(str + str2 + str3 + ((t2 != null) ? t2.ToString() : null));
					num2 = 0f;
				}
				num += num2;
			}
			float num3 = defaultValueWeight + num;
			if (num3 <= 0f)
			{
				Log.Error("RandomElementByWeightWithDefault with totalWeight=" + num3.ToString());
				return default(T);
			}
			if (Rand.Value < defaultValueWeight / num3 || num == 0f)
			{
				return default(T);
			}
			return source.RandomElementByWeight(weightSelector);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001C3F4 File Offset: 0x0001A5F4
		public static T FirstOrFallback<T>(this IEnumerable<T> source, T fallback = default(T))
		{
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return fallback;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001C43C File Offset: 0x0001A63C
		public static T FirstOrFallback<T>(this IEnumerable<T> source, Func<T, bool> predicate, T fallback = default(T))
		{
			return source.Where(predicate).FirstOrFallback(fallback);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001C44B File Offset: 0x0001A64B
		public static TSource MaxBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return GenCollection.MaxBy<TSource, TKey>(source, selector, Comparer<TKey>.Default);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001C45C File Offset: 0x0001A65C
		public static TSource MaxBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource tsource = enumerator.Current;
				TKey y = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, y) > 0)
					{
						tsource = tsource2;
						y = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001C508 File Offset: 0x0001A708
		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, TSource fallback = default(TSource))
		{
			return source.MaxByWithFallback(selector, Comparer<TKey>.Default, fallback);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001C518 File Offset: 0x0001A718
		public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, TSource fallback = default(TSource))
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					result = fallback;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) > 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					result = tsource;
				}
			}
			return result;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001C5C0 File Offset: 0x0001A7C0
		public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMaxBy(selector, Comparer<TKey>.Default, out value);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001C5D0 File Offset: 0x0001A7D0
		public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			bool result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					value = default(TSource);
					result = false;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) > 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					value = tsource;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001C684 File Offset: 0x0001A884
		public static TSource MinBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return GenCollection.MinBy<TSource, TKey>(source, selector, Comparer<TKey>.Default);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0001C694 File Offset: 0x0001A894
		public static TSource MinBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			TSource result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("Sequence contains no elements");
				}
				TSource tsource = enumerator.Current;
				TKey y = selector(tsource);
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					TKey tkey = selector(tsource2);
					if (comparer.Compare(tkey, y) < 0)
					{
						tsource = tsource2;
						y = tkey;
					}
				}
				result = tsource;
			}
			return result;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001C740 File Offset: 0x0001A940
		public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
		{
			return source.TryMinBy(selector, Comparer<TKey>.Default, out value);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001C750 File Offset: 0x0001A950
		public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			bool result;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					value = default(TSource);
					result = false;
				}
				else
				{
					TSource tsource = enumerator.Current;
					TKey y = selector(tsource);
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						TKey tkey = selector(tsource2);
						if (comparer.Compare(tkey, y) < 0)
						{
							tsource = tsource2;
							y = tkey;
						}
					}
					value = tsource;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001C804 File Offset: 0x0001AA04
		public static void SortBy<T, TSortBy>(this List<T> list, Func<T, TSortBy> selector) where TSortBy : IComparable<TSortBy>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				return tsortBy.CompareTo(selector(b));
			});
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0001C83C File Offset: 0x0001AA3C
		public static void SortBy<T, TSortBy, TThenBy>(this List<T> list, Func<T, TSortBy> selector, Func<T, TThenBy> thenBySelector) where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				TSortBy other = selector(b);
				if (!tsortBy.Equals(other))
				{
					return tsortBy.CompareTo(other);
				}
				TThenBy tthenBy = thenBySelector(a);
				return tthenBy.CompareTo(thenBySelector(b));
			});
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001C87C File Offset: 0x0001AA7C
		public static void SortBy<T, TSortBy, TThenBy, TThenBy2>(this List<T> list, Func<T, TSortBy> selector, Func<T, TThenBy> thenBySelector, Func<T, TThenBy2> thenBy2Selector) where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>, IEquatable<TThenBy> where TThenBy2 : IComparable<TThenBy2>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortBy tsortBy = selector(a);
				TSortBy other = selector(b);
				if (!tsortBy.Equals(other))
				{
					return tsortBy.CompareTo(other);
				}
				TThenBy tthenBy = thenBySelector(a);
				TThenBy other2 = thenBySelector(b);
				if (!tthenBy.Equals(other2))
				{
					return tthenBy.CompareTo(other2);
				}
				TThenBy2 tthenBy2 = thenBy2Selector(a);
				return tthenBy2.CompareTo(thenBy2Selector(b));
			});
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001C8C0 File Offset: 0x0001AAC0
		public static void SortByColor<T>(this List<T> colorDefs, Func<T, Color> getColor)
		{
			colorDefs.SortBy(delegate(T x)
			{
				float num;
				float a;
				float num2;
				Color.RGBToHSV(getColor(x), out num, out a, out num2);
				if (!Mathf.Approximately(a, 0f))
				{
					return (float)Mathf.RoundToInt(num * 100f);
				}
				return -1f;
			}, delegate(T x)
			{
				float num;
				float num2;
				float num3;
				Color.RGBToHSV(getColor(x), out num, out num2, out num3);
				return Mathf.RoundToInt(num3 * 100f);
			});
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001C8F8 File Offset: 0x0001AAF8
		public static void SortByDescending<T, TSortByDescending>(this List<T> list, Func<T, TSortByDescending> selector) where TSortByDescending : IComparable<TSortByDescending>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortByDescending tsortByDescending = selector(b);
				return tsortByDescending.CompareTo(selector(a));
			});
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001C930 File Offset: 0x0001AB30
		public static void SortByDescending<T, TSortByDescending, TThenByDescending>(this List<T> list, Func<T, TSortByDescending> selector, Func<T, TThenByDescending> thenByDescendingSelector) where TSortByDescending : IComparable<TSortByDescending>, IEquatable<TSortByDescending> where TThenByDescending : IComparable<TThenByDescending>
		{
			if (list.Count <= 1)
			{
				return;
			}
			list.Sort(delegate(T a, T b)
			{
				TSortByDescending other = selector(a);
				TSortByDescending other2 = selector(b);
				if (!other.Equals(other2))
				{
					return other2.CompareTo(other);
				}
				TThenByDescending tthenByDescending = thenByDescendingSelector(b);
				return tthenByDescending.CompareTo(thenByDescendingSelector(a));
			});
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001C970 File Offset: 0x0001AB70
		public static void SortStable<T>(this IList<T> list, Func<T, T, int> comparator)
		{
			if (list.Count <= 1)
			{
				return;
			}
			List<Pair<T, int>> list2;
			bool flag;
			if (GenCollection.SortStableTempList<T>.working)
			{
				list2 = new List<Pair<T, int>>();
				flag = false;
			}
			else
			{
				list2 = GenCollection.SortStableTempList<T>.list;
				GenCollection.SortStableTempList<T>.working = true;
				flag = true;
			}
			try
			{
				list2.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(new Pair<T, int>(list[i], i));
				}
				list2.Sort(delegate(Pair<T, int> lhs, Pair<T, int> rhs)
				{
					int num = comparator(lhs.First, rhs.First);
					if (num != 0)
					{
						return num;
					}
					return lhs.Second.CompareTo(rhs.Second);
				});
				list.Clear();
				for (int j = 0; j < list2.Count; j++)
				{
					list.Add(list2[j].First);
				}
				list2.Clear();
			}
			finally
			{
				if (flag)
				{
					GenCollection.SortStableTempList<T>.working = false;
				}
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001CA44 File Offset: 0x0001AC44
		public static IComparer<T> ThenBy<T>(this IComparer<T> first, IComparer<T> second)
		{
			return new GenCollection.ComparerChain<T>(first, second);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001CA4D File Offset: 0x0001AC4D
		public static IComparer<T> Descending<T>(this IComparer<T> cmp)
		{
			return new GenCollection.DescendingComparer<T>(cmp);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001CA55 File Offset: 0x0001AC55
		public static IComparer<T> CompareBy<T, TComparable>(Func<T, TComparable> selector) where TComparable : IComparable<TComparable>
		{
			return Comparer<T>.Create(delegate(T a, T b)
			{
				TComparable tcomparable = selector(a);
				return tcomparable.CompareTo(selector(b));
			});
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001CA74 File Offset: 0x0001AC74
		public static int RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			List<TKey> list = null;
			int result;
			try
			{
				foreach (KeyValuePair<TKey, TValue> obj in dictionary)
				{
					if (predicate(obj))
					{
						if (list == null)
						{
							list = SimplePool<List<TKey>>.Get();
						}
						list.Add(obj.Key);
					}
				}
				if (list != null)
				{
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						dictionary.Remove(list[i]);
						i++;
					}
					result = list.Count;
				}
				else
				{
					result = 0;
				}
			}
			finally
			{
				if (list != null)
				{
					list.Clear();
					SimplePool<List<TKey>>.Return(list);
				}
			}
			return result;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0001CB30 File Offset: 0x0001AD30
		public static void RemoveAll<T>(this List<T> list, Func<T, int, bool> predicate)
		{
			int num = 0;
			int count = list.Count;
			while (num < count && !predicate(list[num], num))
			{
				num++;
			}
			if (num >= count)
			{
				return;
			}
			int i = num + 1;
			while (i < count)
			{
				while (i < count && predicate(list[i], i))
				{
					i++;
				}
				if (i < count)
				{
					list[num++] = list[i++];
				}
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001CBA1 File Offset: 0x0001ADA1
		public static void RemoveLast<T>(this List<T> list)
		{
			list.RemoveAt(list.Count - 1);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001CBB1 File Offset: 0x0001ADB1
		public static T Pop<T>(this List<T> list)
		{
			T result = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return result;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001CBCF File Offset: 0x0001ADCF
		public static T PopFront<T>(this List<T> list)
		{
			T result = list[0];
			list.RemoveAt(0);
			return result;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001CBDF File Offset: 0x0001ADDF
		public static bool Any<T>(this List<T> list, Predicate<T> predicate)
		{
			return list.FindIndex(predicate) != -1;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001CBEE File Offset: 0x0001ADEE
		public static bool Any<T>(this List<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001CBF9 File Offset: 0x0001ADF9
		public static bool Empty<T>(this List<T> list)
		{
			return list.Count == 0;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001CC04 File Offset: 0x0001AE04
		public static bool Any<T>(this HashSet<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001CC0F File Offset: 0x0001AE0F
		public static bool Any<T>(this Stack<T> list)
		{
			return list.Count != 0;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001CC1C File Offset: 0x0001AE1C
		public static void AddRange<T>(this HashSet<T> set, List<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				set.Add(list[i]);
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001CC48 File Offset: 0x0001AE48
		public static void AddRange<T>(this HashSet<T> set, HashSet<T> other)
		{
			foreach (T item in other)
			{
				set.Add(item);
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001CC98 File Offset: 0x0001AE98
		public static void AddRangeNoAlloc<T>(this List<T> self, IEnumerator<T> other) where T : class
		{
			while (other.MoveNext())
			{
				T item = other.Current;
				self.Add(item);
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001CCB0 File Offset: 0x0001AEB0
		public static void AddRangeFast<T>(this List<T> self, IEnumerable<T> other) where T : class
		{
			using (IEnumerator<T> enumerator = other.GetEnumerator())
			{
				self.AddRangeNoAlloc(enumerator);
			}
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001CCE8 File Offset: 0x0001AEE8
		public static void AddRangeWhereFast<T>(this List<T> self, IEnumerable<T> other, Predicate<T> condition) where T : class
		{
			foreach (T t in other)
			{
				if (condition(t))
				{
					self.Add(t);
				}
			}
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001CD3C File Offset: 0x0001AF3C
		public static int Count_EnumerableBase(IEnumerable e)
		{
			if (e == null)
			{
				return 0;
			}
			ICollection collection = e as ICollection;
			if (collection != null)
			{
				return collection.Count;
			}
			int num = 0;
			foreach (object obj in e)
			{
				num++;
			}
			return num;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001CDA4 File Offset: 0x0001AFA4
		public static T FirstOrDefault<T>(this List<T> list, Predicate<T> predicate)
		{
			foreach (T t in list)
			{
				if (predicate(t))
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001CE04 File Offset: 0x0001B004
		public static object FirstOrDefault_EnumerableBase(IEnumerable e)
		{
			if (e == null)
			{
				return null;
			}
			IList list = e as IList;
			if (list == null)
			{
				using (IEnumerator enumerator = e.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
				return null;
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001CE74 File Offset: 0x0001B074
		public static float AverageWeighted<T>(this IEnumerable<T> list, Func<T, float> weight, Func<T, float> value)
		{
			float num = 0f;
			float num2 = 0f;
			foreach (T arg in list)
			{
				float num3 = weight(arg);
				num += num3;
				num2 += value(arg) * num3;
			}
			return num2 / num;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001CEE0 File Offset: 0x0001B0E0
		public static void ExecuteEnumerable(this IEnumerable enumerable)
		{
			foreach (object obj in enumerable)
			{
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001CF28 File Offset: 0x0001B128
		public static IEnumerable<T> OrElseEmptyEnumerable<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return Enumerable.Empty<T>();
			}
			return enumerable;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001CF34 File Offset: 0x0001B134
		public static bool EnumerableNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return true;
			}
			ICollection collection = enumerable as ICollection;
			if (collection != null)
			{
				return collection.Count == 0;
			}
			return !enumerable.Any<T>();
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001CF64 File Offset: 0x0001B164
		public static int EnumerableCount(this IEnumerable enumerable)
		{
			if (enumerable == null)
			{
				return 0;
			}
			ICollection collection = enumerable as ICollection;
			if (collection != null)
			{
				return collection.Count;
			}
			int num = 0;
			foreach (object obj in enumerable)
			{
				num++;
			}
			return num;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001CFCC File Offset: 0x0001B1CC
		public static int Count<T>(this List<T> list, Predicate<T> predicate)
		{
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (predicate(list[i]))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001D000 File Offset: 0x0001B200
		public static float Percent<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			int num = 0;
			int num2 = 0;
			foreach (T arg in source)
			{
				num2++;
				if (predicate(arg))
				{
					num++;
				}
			}
			if (num2 == 0)
			{
				throw new InvalidOperationException("Sequence contains no elements");
			}
			return (float)num / (float)num2;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001D06C File Offset: 0x0001B26C
		public static int FirstIndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
		{
			int num = 0;
			foreach (T arg in enumerable)
			{
				if (predicate(arg))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001D0C4 File Offset: 0x0001B2C4
		public static V TryGetValue<T, V>(this IReadOnlyDictionary<T, V> dict, T key, V fallback = default(V))
		{
			V result;
			if (key == null || !dict.TryGetValue(key, out result))
			{
				result = fallback;
			}
			return result;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001D0E7 File Offset: 0x0001B2E7
		public static IReadOnlyDictionary<T, V> Slice<T, V>(this IReadOnlyDictionary<T, V> dict, IEnumerable<T> keys)
		{
			return new GenCollection.DictionarySlice<T, V>(dict, keys);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001D0F0 File Offset: 0x0001B2F0
		public static IReadOnlyDictionary<T, V> Slice<T, V>(this IReadOnlyDictionary<T, V> dict, params T[] keys)
		{
			return new GenCollection.DictionarySlice<T, V>(dict, keys);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001D0F9 File Offset: 0x0001B2F9
		public static IEnumerable<Pair<T, V>> Cross<T, V>(this IEnumerable<T> lhs, IEnumerable<V> rhs)
		{
			GenCollection.<Cross>d__75<T, V> <Cross>d__ = new GenCollection.<Cross>d__75<T, V>(-2);
			<Cross>d__.<>3__lhs = lhs;
			<Cross>d__.<>3__rhs = rhs;
			return <Cross>d__;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001D110 File Offset: 0x0001B310
		public static IEnumerable<T> Concat<T>(this IEnumerable<T> lhs, T rhs)
		{
			GenCollection.<Concat>d__76<T> <Concat>d__ = new GenCollection.<Concat>d__76<T>(-2);
			<Concat>d__.<>3__lhs = lhs;
			<Concat>d__.<>3__rhs = rhs;
			return <Concat>d__;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001D128 File Offset: 0x0001B328
		public static void RemoveWhere<T>(this IList<T> list, Func<T, bool> predicate)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (predicate(list[i]))
				{
					list.RemoveAt(i);
				}
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001D160 File Offset: 0x0001B360
		public static bool ContainsAny<T>(this IList<T> list, Func<T, bool> predicate)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (predicate(list[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001D190 File Offset: 0x0001B390
		public static IEnumerable<TSource> ConcatIfNotNull<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}
			if (second == null)
			{
				return first;
			}
			return first.Concat(second);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001D1AC File Offset: 0x0001B3AC
		public static LocalTargetInfo FirstValid(this List<LocalTargetInfo> source)
		{
			if (source == null)
			{
				return LocalTargetInfo.Invalid;
			}
			for (int i = 0; i < source.Count; i++)
			{
				if (source[i].IsValid)
				{
					return source[i];
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001D1F1 File Offset: 0x0001B3F1
		public static IEnumerable<T> Except<T>(this IEnumerable<T> lhs, T rhs) where T : class
		{
			GenCollection.<Except>d__81<T> <Except>d__ = new GenCollection.<Except>d__81<T>(-2);
			<Except>d__.<>3__lhs = lhs;
			<Except>d__.<>3__rhs = rhs;
			return <Except>d__;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001D208 File Offset: 0x0001B408
		public static IEnumerable<ValueTuple<T, T>> Pairwise<T>(this IEnumerable<T> enumerable) where T : class
		{
			GenCollection.<Pairwise>d__82<T> <Pairwise>d__ = new GenCollection.<Pairwise>d__82<T>(-2);
			<Pairwise>d__.<>3__enumerable = enumerable;
			return <Pairwise>d__;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001D218 File Offset: 0x0001B418
		public static bool ListsEqual<T>(List<T> a, List<T> b) where T : class
		{
			if (a == b)
			{
				return true;
			}
			if (a.NullOrEmpty<T>() && b.NullOrEmpty<T>())
			{
				return true;
			}
			if (a.NullOrEmpty<T>() || b.NullOrEmpty<T>())
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < a.Count; i++)
			{
				if (!@default.Equals(a[i], b[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001D290 File Offset: 0x0001B490
		public static bool DictsEqual<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> a, IReadOnlyDictionary<TKey, TValue> b)
		{
			int num = (a != null) ? a.Count : 0;
			int num2 = (b != null) ? b.Count : 0;
			if (num != num2)
			{
				return false;
			}
			if (num == 0)
			{
				return true;
			}
			EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in a)
			{
				TValue y;
				if (!b.TryGetValue(keyValuePair.Key, out y) || !@default.Equals(keyValuePair.Value, y))
				{
					return false;
				}
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair2 in b)
			{
				TValue y2;
				if (!a.TryGetValue(keyValuePair2.Key, out y2) || !@default.Equals(keyValuePair2.Value, y2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001D380 File Offset: 0x0001B580
		public static int DictHashCode<Key, Value>(IReadOnlyDictionary<Key, Value> dict)
		{
			int num = 0;
			foreach (KeyValuePair<Key, Value> keyValuePair in (dict ?? Enumerable.Empty<KeyValuePair<Key, Value>>()))
			{
				num ^= HashCode.Combine<Key, Value>(keyValuePair.Key, keyValuePair.Value);
			}
			return num;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001D3E8 File Offset: 0x0001B5E8
		public static void Deconstruct<Key, Value>(this KeyValuePair<Key, Value> tuple, out Key key, out Value value)
		{
			key = tuple.Key;
			value = tuple.Value;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001D404 File Offset: 0x0001B604
		public static bool SetsEqual<T>(this List<T> a, List<T> b)
		{
			if (a == b)
			{
				return true;
			}
			if (a.NullOrEmpty<T>() && b.NullOrEmpty<T>())
			{
				return true;
			}
			if (a.NullOrEmpty<T>() || b.NullOrEmpty<T>())
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (!b.Contains(a[i]))
				{
					return false;
				}
			}
			for (int j = 0; j < b.Count; j++)
			{
				if (!a.Contains(b[j]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001D480 File Offset: 0x0001B680
		public static IEnumerable<T> TakeRandom<T>(this List<T> list, int count)
		{
			GenCollection.<TakeRandom>d__88<T> <TakeRandom>d__ = new GenCollection.<TakeRandom>d__88<T>(-2);
			<TakeRandom>d__.<>3__list = list;
			<TakeRandom>d__.<>3__count = count;
			return <TakeRandom>d__;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001D498 File Offset: 0x0001B698
		public static List<T> TakeRandomDistinct<T>(this List<T> list, int count)
		{
			List<T> list2 = new List<T>();
			foreach (T item in list.Distinct<T>().InRandomOrder(null))
			{
				list2.Add(item);
				if (list2.Count == count)
				{
					return list2;
				}
			}
			return list2;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001D504 File Offset: 0x0001B704
		public static List<T> TakeRandomDistinct<T>(this IEnumerable<T> list, int count)
		{
			List<T> list2 = new List<T>();
			foreach (T item in list.Distinct<T>().InRandomOrder(null))
			{
				list2.Add(item);
				if (list2.Count == count)
				{
					return list2;
				}
			}
			return list2;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001D570 File Offset: 0x0001B770
		public static void AddDistinct<T>(this List<T> list, T obj)
		{
			list.AddUnique(obj);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001D579 File Offset: 0x0001B779
		public static void AddUnique<T>(this List<T> list, T obj)
		{
			if (!list.Contains(obj))
			{
				list.Add(obj);
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001D58C File Offset: 0x0001B78C
		public static void AddRangeUnique<T>(this List<T> list, List<T> other)
		{
			for (int i = 0; i < other.Count; i++)
			{
				if (!list.Contains(other[i]))
				{
					list.Add(other[i]);
				}
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001D5C8 File Offset: 0x0001B7C8
		public static void AddRangeUnique<T>(this List<T> list, IEnumerable<T> other)
		{
			foreach (T item in other)
			{
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001D61C File Offset: 0x0001B81C
		public static int Replace<T>(this IList<T> list, T replace, T with) where T : class
		{
			if (list == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == replace)
				{
					list[i] = with;
					num++;
				}
			}
			return num;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001D662 File Offset: 0x0001B862
		public static Pair<K, List<E>> ConvertIGroupingToPair<K, E>(IGrouping<K, E> g)
		{
			return new Pair<K, List<E>>(g.Key, g.ToList<E>());
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001D678 File Offset: 0x0001B878
		public static int GetCountGreaterOrEqualInSortedList(List<int> list, int val)
		{
			int num = list.BinarySearch(val);
			if (num >= 0)
			{
				return list.Count - num;
			}
			int num2 = ~num;
			return list.Count - num2;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001D6A8 File Offset: 0x0001B8A8
		public static void InsertIntoSortedList<T>(this List<T> list, T val, IComparer<T> cmp)
		{
			if (list.Count == 0)
			{
				list.Add(val);
				return;
			}
			int num = list.BinarySearch(val, cmp);
			if (num >= 0)
			{
				list.Insert(num, val);
				return;
			}
			list.Insert(~num, val);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001D6E4 File Offset: 0x0001B8E4
		public static void RemoveBatchUnordered<T>(this List<T> list, List<int> indices)
		{
			if (indices.Count == 0)
			{
				return;
			}
			int num = list.Count - 1;
			foreach (int num2 in indices)
			{
				if (num == num2)
				{
					num--;
				}
				else
				{
					if (num <= 0)
					{
						break;
					}
					list[num2] = list[num];
					num--;
				}
			}
			for (int i = list.Count - 1; i > num; i--)
			{
				list.RemoveAt(i);
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001D77C File Offset: 0x0001B97C
		public static ValueType GetWithFallback<KeyType, ValueType>(this Dictionary<KeyType, ValueType> dictionary, KeyType key, ValueType fallback = default(ValueType))
		{
			ValueType result;
			if (dictionary.TryGetValue(key, out result))
			{
				return result;
			}
			return fallback;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001D798 File Offset: 0x0001B998
		public static void ClearNullAndDestroyed<T>(this List<T> list) where T : Thing
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i].DestroyedOrNull())
				{
					list.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001D7D2 File Offset: 0x0001B9D2
		public static bool TryPop<T>(this Stack<T> stack, out T element)
		{
			if (stack.Count == 0)
			{
				element = default(T);
				return false;
			}
			element = stack.Pop();
			return true;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001D7F2 File Offset: 0x0001B9F2
		public static bool TryDequeue<T>(this Queue<T> queue, out T element)
		{
			if (queue.Count == 0)
			{
				element = default(T);
				return false;
			}
			element = queue.Dequeue();
			return true;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001D814 File Offset: 0x0001BA14
		public static void CopyToList<T>(this IEnumerable<T> source, List<T> target, bool clear = true)
		{
			if (clear)
			{
				target.Clear();
			}
			ICollection<T> collection = source as ICollection<T>;
			if (collection != null)
			{
				int num = (clear ? 0 : target.Count) + collection.Count;
				if (target.Capacity < num)
				{
					target.Capacity = num;
				}
			}
			foreach (T item in source)
			{
				target.Add(item);
			}
		}

		// Token: 0x020024A4 RID: 9380
		private static class SortStableTempList<T>
		{
			// Token: 0x04009600 RID: 38400
			public static List<Pair<T, int>> list = new List<Pair<T, int>>();

			// Token: 0x04009601 RID: 38401
			public static bool working;
		}

		// Token: 0x020024A5 RID: 9381
		private class ComparerChain<T> : IComparer<T>
		{
			// Token: 0x0600EAD2 RID: 60114 RVA: 0x0056E863 File Offset: 0x0056CA63
			public ComparerChain(IComparer<T> first, IComparer<T> second)
			{
				this.first = first;
				this.second = second;
			}

			// Token: 0x0600EAD3 RID: 60115 RVA: 0x0056E87C File Offset: 0x0056CA7C
			public int Compare(T x, T y)
			{
				int num = this.first.Compare(x, y);
				if (num != 0)
				{
					return num;
				}
				return this.second.Compare(x, y);
			}

			// Token: 0x04009602 RID: 38402
			private readonly IComparer<T> first;

			// Token: 0x04009603 RID: 38403
			private readonly IComparer<T> second;
		}

		// Token: 0x020024A6 RID: 9382
		private class DescendingComparer<T> : IComparer<T>
		{
			// Token: 0x0600EAD4 RID: 60116 RVA: 0x0056E8A9 File Offset: 0x0056CAA9
			public DescendingComparer(IComparer<T> cmp)
			{
				this.cmp = cmp;
			}

			// Token: 0x0600EAD5 RID: 60117 RVA: 0x0056E8B8 File Offset: 0x0056CAB8
			public int Compare(T x, T y)
			{
				return -this.cmp.Compare(x, y);
			}

			// Token: 0x04009604 RID: 38404
			private readonly IComparer<T> cmp;
		}

		// Token: 0x020024A7 RID: 9383
		public class DictionarySlice<T, V> : IReadOnlyDictionary<T, V>, IEnumerable<KeyValuePair<T, V>>, IEnumerable, IReadOnlyCollection<KeyValuePair<T, V>>
		{
			// Token: 0x1700298E RID: 10638
			public V this[T key]
			{
				get
				{
					if (key == null)
					{
						throw new ArgumentNullException();
					}
					if (!this.keys.Contains(key))
					{
						throw new KeyNotFoundException(key.ToStringSafe<T>());
					}
					return this.underlying[key];
				}
			}

			// Token: 0x1700298F RID: 10639
			// (get) Token: 0x0600EAD7 RID: 60119 RVA: 0x0056E8FE File Offset: 0x0056CAFE
			public IEnumerable<V> Values
			{
				get
				{
					GenCollection.DictionarySlice<T, V>.<get_Values>d__3 <get_Values>d__ = new GenCollection.DictionarySlice<T, V>.<get_Values>d__3(-2);
					<get_Values>d__.<>4__this = this;
					return <get_Values>d__;
				}
			}

			// Token: 0x17002990 RID: 10640
			// (get) Token: 0x0600EAD8 RID: 60120 RVA: 0x0056E90E File Offset: 0x0056CB0E
			public IEnumerable<T> Keys
			{
				get
				{
					return this.keys;
				}
			}

			// Token: 0x17002991 RID: 10641
			// (get) Token: 0x0600EAD9 RID: 60121 RVA: 0x0056E916 File Offset: 0x0056CB16
			public int Count
			{
				get
				{
					return this.keys.Count;
				}
			}

			// Token: 0x0600EADA RID: 60122 RVA: 0x0056E923 File Offset: 0x0056CB23
			public bool ContainsKey(T key)
			{
				return key != null && this.keys.Contains(key);
			}

			// Token: 0x0600EADB RID: 60123 RVA: 0x0056E93B File Offset: 0x0056CB3B
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x0600EADC RID: 60124 RVA: 0x0056E943 File Offset: 0x0056CB43
			public DictionarySlice(IReadOnlyDictionary<T, V> dict, IEnumerable<T> keys)
			{
				this.underlying = dict;
				this.keys = new HashSet<T>(keys);
			}

			// Token: 0x0600EADD RID: 60125 RVA: 0x0056E95E File Offset: 0x0056CB5E
			public IEnumerator<KeyValuePair<T, V>> GetEnumerator()
			{
				GenCollection.DictionarySlice<T, V>.<GetEnumerator>d__11 <GetEnumerator>d__ = new GenCollection.DictionarySlice<T, V>.<GetEnumerator>d__11(0);
				<GetEnumerator>d__.<>4__this = this;
				return <GetEnumerator>d__;
			}

			// Token: 0x0600EADE RID: 60126 RVA: 0x0056E96D File Offset: 0x0056CB6D
			public bool TryGetValue(T key, out V value)
			{
				if (!this.keys.Contains(key))
				{
					value = default(V);
					return false;
				}
				return this.underlying.TryGetValue(key, out value);
			}

			// Token: 0x04009605 RID: 38405
			private HashSet<T> keys;

			// Token: 0x04009606 RID: 38406
			private IReadOnlyDictionary<T, V> underlying;
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Verse
{
	// Token: 0x0200010A RID: 266
	public static class ShortHashGiver
	{
		// Token: 0x06000941 RID: 2369 RVA: 0x00035508 File Offset: 0x00033708
		public static void GiveAllShortHashes()
		{
			ShortHashGiver.takenHashesPerDeftype.Clear();
			Parallel.ForEach<ValueTuple<Type, HashSet<ushort>>>(GenDefDatabase.AllDefTypesWithDatabases().Select(delegate(Type defType)
			{
				HashSet<ushort> hashSet;
				if (!ShortHashGiver.takenHashesPerDeftype.TryGetValue(defType, out hashSet))
				{
					hashSet = new HashSet<ushort>();
					ShortHashGiver.takenHashesPerDeftype.Add(defType, hashSet);
				}
				return new ValueTuple<Type, HashSet<ushort>>(defType, hashSet);
			}).ToArray<ValueTuple<Type, HashSet<ushort>>>(), delegate([TupleElementNames(new string[]
			{
				"defType",
				"takenHashes"
			})] ValueTuple<Type, HashSet<ushort>> pair)
			{
				Type item = pair.Item1;
				HashSet<ushort> item2 = pair.Item2;
				Type type = typeof(DefDatabase<>).MakeGenericType(new Type[]
				{
					item
				});
				IEnumerable enumerable = (IEnumerable)type.GetProperty("AllDefs").GetGetMethod().Invoke(null, null);
				List<Def> list = new List<Def>();
				foreach (object obj in enumerable)
				{
					Def item3 = (Def)obj;
					list.Add(item3);
				}
				list.SortBy((Def d) => d.defName);
				for (int i = 0; i < list.Count; i++)
				{
					ShortHashGiver.GiveShortHash(list[i], item, item2);
				}
				type.GetMethod("InitializeShortHashDictionary").Invoke(null, null);
			});
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00035574 File Offset: 0x00033774
		private static void GiveShortHash(Def def, Type defType, HashSet<ushort> takenHashes)
		{
			if (def.shortHash != 0)
			{
				Log.Error(((def != null) ? def.ToString() : null) + " already has short hash.");
				return;
			}
			ushort num = (ushort)(GenText.StableStringHash(def.defName) % 65535);
			int num2 = 0;
			while (num == 0 || takenHashes.Contains(num))
			{
				num += 1;
				num2++;
				if (num2 > 5000)
				{
					Log.Message("Short hashes are saturated. There are probably too many Defs.");
				}
			}
			def.shortHash = num;
			takenHashes.Add(num);
		}

		// Token: 0x0400044B RID: 1099
		private static Dictionary<Type, HashSet<ushort>> takenHashesPerDeftype = new Dictionary<Type, HashSet<ushort>>();
	}
}

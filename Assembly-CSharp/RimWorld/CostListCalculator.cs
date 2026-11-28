using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E54 RID: 7764
	public static class CostListCalculator
	{
		// Token: 0x0600C97C RID: 51580 RVA: 0x004A3128 File Offset: 0x004A1328
		public static void Reset()
		{
			CostListCalculator.cachedCosts.Clear();
		}

		// Token: 0x0600C97D RID: 51581 RVA: 0x004A3134 File Offset: 0x004A1334
		public static List<ThingDefCountClass> CostListAdjusted(this Thing thing)
		{
			return thing.def.CostListAdjusted(thing.Stuff, true);
		}

		// Token: 0x0600C97E RID: 51582 RVA: 0x004A3148 File Offset: 0x004A1348
		public static List<ThingDefCountClass> CostListAdjusted(this BuildableDef entDef, ThingDef stuff, bool errorOnNullStuff = true)
		{
			if (CostListCalculator.cachedDifficulty != Find.Storyteller.difficulty)
			{
				CostListCalculator.Reset();
				CostListCalculator.cachedDifficulty = Find.Storyteller.difficulty;
			}
			CostListCalculator.CostListPair key = new CostListCalculator.CostListPair(entDef, stuff);
			List<ThingDefCountClass> list;
			if (!CostListCalculator.cachedCosts.TryGetValue(key, out list))
			{
				list = new List<ThingDefCountClass>();
				int num = 0;
				if (entDef.MadeFromStuff)
				{
					if (errorOnNullStuff && stuff == null)
					{
						Log.Error("Cannot get AdjustedCostList for " + ((entDef != null) ? entDef.ToString() : null) + " with null Stuff.");
						if (GenStuff.DefaultStuffFor(entDef) == null)
						{
							return null;
						}
						return entDef.CostListAdjusted(GenStuff.DefaultStuffFor(entDef), true);
					}
					else if (stuff != null)
					{
						num = Mathf.RoundToInt((float)entDef.CostStuffCount / stuff.VolumePerUnit);
						if (num < 1)
						{
							num = 1;
						}
					}
					else
					{
						num = entDef.CostStuffCount;
					}
				}
				else if (stuff != null)
				{
					Log.Error(string.Concat(new string[]
					{
						"Got AdjustedCostList for ",
						(entDef != null) ? entDef.ToString() : null,
						" with stuff ",
						(stuff != null) ? stuff.ToString() : null,
						" but is not MadeFromStuff."
					}));
				}
				bool flag = false;
				if (entDef.CostList != null)
				{
					for (int i = 0; i < entDef.CostList.Count; i++)
					{
						ThingDefCountClass thingDefCountClass = entDef.CostList[i];
						if (thingDefCountClass.thingDef == stuff)
						{
							list.Add(new ThingDefCountClass(thingDefCountClass.thingDef, thingDefCountClass.count + num));
							flag = true;
						}
						else
						{
							list.Add(thingDefCountClass);
						}
					}
				}
				if (!flag && num > 0)
				{
					list.Add(new ThingDefCountClass(stuff, num));
				}
				CostListCalculator.cachedCosts.Add(key, list);
			}
			return list;
		}

		// Token: 0x040072B3 RID: 29363
		private static Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>> cachedCosts = new Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>>(CostListCalculator.FastCostListPairComparer.Instance);

		// Token: 0x040072B4 RID: 29364
		private static Difficulty cachedDifficulty = null;

		// Token: 0x02003BA3 RID: 15267
		private struct CostListPair : IEquatable<CostListCalculator.CostListPair>
		{
			// Token: 0x0601561B RID: 87579 RVA: 0x006C42BB File Offset: 0x006C24BB
			public CostListPair(BuildableDef buildable, ThingDef stuff)
			{
				this.buildable = buildable;
				this.stuff = stuff;
			}

			// Token: 0x0601561C RID: 87580 RVA: 0x006C42CB File Offset: 0x006C24CB
			public override int GetHashCode()
			{
				return Gen.HashCombine<ThingDef>(Gen.HashCombine<BuildableDef>(0, this.buildable), this.stuff);
			}

			// Token: 0x0601561D RID: 87581 RVA: 0x006C42E4 File Offset: 0x006C24E4
			public override bool Equals(object obj)
			{
				return obj is CostListCalculator.CostListPair && this.Equals((CostListCalculator.CostListPair)obj);
			}

			// Token: 0x0601561E RID: 87582 RVA: 0x006C42FC File Offset: 0x006C24FC
			public bool Equals(CostListCalculator.CostListPair other)
			{
				return this == other;
			}

			// Token: 0x0601561F RID: 87583 RVA: 0x006C430A File Offset: 0x006C250A
			public static bool operator ==(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return lhs.buildable == rhs.buildable && lhs.stuff == rhs.stuff;
			}

			// Token: 0x06015620 RID: 87584 RVA: 0x006C432A File Offset: 0x006C252A
			public static bool operator !=(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0400EC5B RID: 60507
			public BuildableDef buildable;

			// Token: 0x0400EC5C RID: 60508
			public ThingDef stuff;
		}

		// Token: 0x02003BA4 RID: 15268
		private class FastCostListPairComparer : IEqualityComparer<CostListCalculator.CostListPair>
		{
			// Token: 0x06015621 RID: 87585 RVA: 0x006C4336 File Offset: 0x006C2536
			public bool Equals(CostListCalculator.CostListPair x, CostListCalculator.CostListPair y)
			{
				return x == y;
			}

			// Token: 0x06015622 RID: 87586 RVA: 0x006C433F File Offset: 0x006C253F
			public int GetHashCode(CostListCalculator.CostListPair obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x0400EC5D RID: 60509
			public static readonly CostListCalculator.FastCostListPairComparer Instance = new CostListCalculator.FastCostListPairComparer();
		}
	}
}

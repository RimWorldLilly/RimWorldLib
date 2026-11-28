using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F1F RID: 7967
	public static class GenStuff
	{
		// Token: 0x17002525 RID: 9509
		// (get) Token: 0x0600CB3C RID: 52028 RVA: 0x004AC338 File Offset: 0x004AA538
		private static List<ThingDef> StuffDefs
		{
			get
			{
				if (GenStuff.cachedStuffDefs == null)
				{
					GenStuff.cachedStuffDefs = new List<ThingDef>();
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
					{
						if (thingDef.IsStuff)
						{
							GenStuff.cachedStuffDefs.Add(thingDef);
						}
					}
				}
				return GenStuff.cachedStuffDefs;
			}
		}

		// Token: 0x0600CB3D RID: 52029 RVA: 0x004AC3AC File Offset: 0x004AA5AC
		public static ThingDef DefaultStuffFor(BuildableDef bd)
		{
			if (!bd.MadeFromStuff)
			{
				return null;
			}
			ThingDef thingDef = bd as ThingDef;
			if (thingDef != null)
			{
				if (thingDef.defaultStuff != null)
				{
					return thingDef.defaultStuff;
				}
				if (thingDef.IsMeleeWeapon)
				{
					if (ThingDefOf.Steel.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Steel;
					}
					if (ThingDefOf.Plasteel.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Plasteel;
					}
				}
				if (thingDef.IsApparel)
				{
					if (ThingDefOf.Cloth.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Cloth;
					}
					if (ThingDefOf.Leather_Plain.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Leather_Plain;
					}
					if (ThingDefOf.Steel.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Steel;
					}
				}
			}
			if (ThingDefOf.WoodLog.stuffProps.CanMake(bd))
			{
				return ThingDefOf.WoodLog;
			}
			if (ThingDefOf.Steel.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Steel;
			}
			if (ThingDefOf.Plasteel.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Plasteel;
			}
			if (ThingDefOf.BlocksGranite.stuffProps.CanMake(bd))
			{
				return ThingDefOf.BlocksGranite;
			}
			if (ThingDefOf.Cloth.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Cloth;
			}
			if (ThingDefOf.Leather_Plain.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Leather_Plain;
			}
			return GenStuff.AllowedStuffsFor(bd, TechLevel.Undefined, false).First<ThingDef>();
		}

		// Token: 0x0600CB3E RID: 52030 RVA: 0x004AC504 File Offset: 0x004AA704
		public static ThingDef RandomStuffFor(ThingDef td)
		{
			if (!td.MadeFromStuff)
			{
				return null;
			}
			return GenStuff.AllowedStuffsFor(td, TechLevel.Undefined, true).RandomElement<ThingDef>();
		}

		// Token: 0x0600CB3F RID: 52031 RVA: 0x004AC520 File Offset: 0x004AA720
		public static ThingDef RandomStuffByCommonalityFor(ThingDef td, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				return null;
			}
			ThingDef result;
			if (!GenStuff.TryRandomStuffByCommonalityFor(td, out result, maxTechLevel))
			{
				result = GenStuff.DefaultStuffFor(td);
			}
			return result;
		}

		// Token: 0x0600CB40 RID: 52032 RVA: 0x004AC54A File Offset: 0x004AA74A
		public static IEnumerable<ThingDef> AllowedStuffsFor(BuildableDef td, TechLevel maxTechLevel = TechLevel.Undefined, bool checkAllowedInStuffGeneration = false)
		{
			GenStuff.<AllowedStuffsFor>d__6 <AllowedStuffsFor>d__ = new GenStuff.<AllowedStuffsFor>d__6(-2);
			<AllowedStuffsFor>d__.<>3__td = td;
			<AllowedStuffsFor>d__.<>3__maxTechLevel = maxTechLevel;
			<AllowedStuffsFor>d__.<>3__checkAllowedInStuffGeneration = checkAllowedInStuffGeneration;
			return <AllowedStuffsFor>d__;
		}

		// Token: 0x0600CB41 RID: 52033 RVA: 0x004AC568 File Offset: 0x004AA768
		public static IEnumerable<ThingDef> AllowedStuffs(List<StuffCategoryDef> categories, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			GenStuff.<AllowedStuffs>d__7 <AllowedStuffs>d__ = new GenStuff.<AllowedStuffs>d__7(-2);
			<AllowedStuffs>d__.<>3__categories = categories;
			<AllowedStuffs>d__.<>3__maxTechLevel = maxTechLevel;
			return <AllowedStuffs>d__;
		}

		// Token: 0x0600CB42 RID: 52034 RVA: 0x004AC57F File Offset: 0x004AA77F
		public static bool TryRandomStuffByCommonalityFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				stuff = null;
				return true;
			}
			return GenStuff.AllowedStuffsFor(td, maxTechLevel, true).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff);
		}

		// Token: 0x0600CB43 RID: 52035 RVA: 0x004AC5BC File Offset: 0x004AA7BC
		public static bool TryRandomStuffFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined, Predicate<ThingDef> validator = null)
		{
			if (!td.MadeFromStuff)
			{
				stuff = null;
				return true;
			}
			if (validator != null)
			{
				GenStuff.allowedStuffTmp.Clear();
				using (IEnumerator<ThingDef> enumerator = GenStuff.AllowedStuffsFor(td, maxTechLevel, true).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef thingDef = enumerator.Current;
						if (validator(thingDef))
						{
							GenStuff.allowedStuffTmp.Add(thingDef);
						}
					}
					goto IL_75;
				}
			}
			GenStuff.allowedStuffTmp.Clear();
			GenStuff.allowedStuffTmp.AddRange(GenStuff.AllowedStuffsFor(td, maxTechLevel, false));
			IL_75:
			return GenStuff.allowedStuffTmp.TryRandomElement(out stuff);
		}

		// Token: 0x0600CB44 RID: 52036 RVA: 0x004AC65C File Offset: 0x004AA85C
		public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, Faction faction, Predicate<ThingDef> validator = null)
		{
			return GenStuff.RandomStuffInexpensiveFor(thingDef, (faction != null) ? faction.def.techLevel : TechLevel.Undefined, validator);
		}

		// Token: 0x0600CB45 RID: 52037 RVA: 0x004AC678 File Offset: 0x004AA878
		public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, TechLevel maxTechLevel = TechLevel.Undefined, Predicate<ThingDef> validator = null)
		{
			if (!thingDef.MadeFromStuff)
			{
				return null;
			}
			IEnumerable<ThingDef> enumerable = GenStuff.AllowedStuffsFor(thingDef, maxTechLevel, true);
			if (validator != null)
			{
				enumerable = from x in enumerable
				where validator(x)
				select x;
			}
			float cheapestPrice = -1f;
			foreach (ThingDef thingDef2 in enumerable)
			{
				float num = thingDef2.BaseMarketValue / thingDef2.VolumePerUnit;
				if (cheapestPrice == -1f || num < cheapestPrice)
				{
					cheapestPrice = num;
				}
			}
			enumerable = from x in enumerable
			where x.BaseMarketValue / x.VolumePerUnit <= cheapestPrice * 4f
			select x;
			ThingDef result;
			if (enumerable.TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600CB46 RID: 52038 RVA: 0x004AC770 File Offset: 0x004AA970
		public static void ResetStaticData()
		{
			GenStuff.cachedStuffDefs = null;
		}

		// Token: 0x0400819C RID: 33180
		private static List<ThingDef> cachedStuffDefs;

		// Token: 0x0400819D RID: 33181
		private static List<ThingDef> allowedStuffTmp = new List<ThingDef>();
	}
}

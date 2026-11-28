using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A8 RID: 6568
	public static class ThingStyleHelper
	{
		// Token: 0x0600A4E7 RID: 42215 RVA: 0x003AE708 File Offset: 0x003AC908
		public static bool CanBeStyled(this ThingDef thingDef)
		{
			return thingDef.CompDefFor<CompStyleable>() != null;
		}

		// Token: 0x0600A4E8 RID: 42216 RVA: 0x003AE714 File Offset: 0x003AC914
		public static bool GetEverSeenByPlayer(this Thing thing)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			bool? flag;
			if (thingWithComps == null)
			{
				flag = null;
			}
			else
			{
				CompStyleable compStyleable = thingWithComps.compStyleable;
				flag = ((compStyleable != null) ? new bool?(compStyleable.everSeenByPlayer) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x0600A4E9 RID: 42217 RVA: 0x003AE75C File Offset: 0x003AC95C
		public static ThingStyleDef GetStyleDef(this Thing thing)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingWithComps == null)
			{
				return null;
			}
			CompStyleable compStyleable = thingWithComps.compStyleable;
			if (compStyleable == null)
			{
				return null;
			}
			return compStyleable.styleDef;
		}

		// Token: 0x0600A4EA RID: 42218 RVA: 0x003AE77A File Offset: 0x003AC97A
		public static Precept_ThingStyle GetStyleSourcePrecept(this Thing thing)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingWithComps == null)
			{
				return null;
			}
			CompStyleable compStyleable = thingWithComps.compStyleable;
			if (compStyleable == null)
			{
				return null;
			}
			return compStyleable.SourcePrecept;
		}

		// Token: 0x0600A4EB RID: 42219 RVA: 0x003AE798 File Offset: 0x003AC998
		public static void SetEverSeenByPlayer(this Thing thing, bool everSeenByPlayer)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			CompStyleable compStyleable = (thingWithComps != null) ? thingWithComps.compStyleable : null;
			if (compStyleable != null)
			{
				compStyleable.everSeenByPlayer = everSeenByPlayer;
			}
			if (everSeenByPlayer && thing.StyleSourcePrecept is Precept_Relic)
			{
				thing.StyleSourcePrecept.ideo.Notify_RelicSeenByPlayer(thing);
			}
		}

		// Token: 0x0600A4EC RID: 42220 RVA: 0x003AE7E4 File Offset: 0x003AC9E4
		public static void SetStyleDef(this Thing thing, ThingStyleDef styleDef)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			CompStyleable compStyleable = (thingWithComps != null) ? thingWithComps.compStyleable : null;
			if (compStyleable == null)
			{
				if (styleDef != null)
				{
					Log.Warning("Tried setting ThingStyleDef to a thing without CompStyleable (" + thing.def.defName + ")!");
				}
				return;
			}
			compStyleable.styleDef = styleDef;
			compStyleable.cachedStyleCategoryDef = null;
		}

		// Token: 0x0600A4ED RID: 42221 RVA: 0x003AE838 File Offset: 0x003ACA38
		public static void SetStyleSourcePrecept(this Thing thing, Precept_ThingStyle precept)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			CompStyleable compStyleable = (thingWithComps != null) ? thingWithComps.compStyleable : null;
			if (compStyleable == null)
			{
				if (precept != null)
				{
					Log.Warning("Tried setting StyleSourcePrecept to a thing without CompStyleable (" + thing.def.defName + ")!");
				}
				return;
			}
			compStyleable.SourcePrecept = precept;
		}
	}
}

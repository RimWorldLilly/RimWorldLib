using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002080 RID: 8320
	public static class GeneResourceDrainUtility
	{
		// Token: 0x0600D1D3 RID: 53715 RVA: 0x004D16BE File Offset: 0x004CF8BE
		public static void TickResourceDrainInterval(IGeneResourceDrain drain, int delta)
		{
			if (drain.CanOffset)
			{
				if (drain.Resource == null)
				{
					return;
				}
				GeneResourceDrainUtility.OffsetResource(drain, -drain.ResourceLossPerDay * (float)delta / 60000f);
			}
		}

		// Token: 0x0600D1D4 RID: 53716 RVA: 0x004D16E8 File Offset: 0x004CF8E8
		public static void PostResourceOffset(IGeneResourceDrain drain, float oldValue)
		{
			if (oldValue > 0f && drain.Resource.Value <= 0f)
			{
				Pawn pawn = drain.Pawn;
				if (!pawn.health.hediffSet.HasHediff(HediffDefOf.HemogenCraving, false))
				{
					pawn.health.AddHediff(HediffDefOf.HemogenCraving, null, null, null);
				}
			}
		}

		// Token: 0x0600D1D5 RID: 53717 RVA: 0x004D174C File Offset: 0x004CF94C
		public static void OffsetResource(IGeneResourceDrain drain, float amnt)
		{
			if (drain.Resource == null)
			{
				return;
			}
			float value = drain.Resource.Value;
			drain.Resource.Value += amnt;
			GeneResourceDrainUtility.PostResourceOffset(drain, value);
		}

		// Token: 0x0600D1D6 RID: 53718 RVA: 0x004D1788 File Offset: 0x004CF988
		public static IEnumerable<Gizmo> GetResourceDrainGizmos(IGeneResourceDrain drain)
		{
			GeneResourceDrainUtility.<GetResourceDrainGizmos>d__3 <GetResourceDrainGizmos>d__ = new GeneResourceDrainUtility.<GetResourceDrainGizmos>d__3(-2);
			<GetResourceDrainGizmos>d__.<>3__drain = drain;
			return <GetResourceDrainGizmos>d__;
		}
	}
}

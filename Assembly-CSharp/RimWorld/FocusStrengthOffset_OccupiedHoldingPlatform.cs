using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001907 RID: 6407
	public class FocusStrengthOffset_OccupiedHoldingPlatform : FocusStrengthOffset
	{
		// Token: 0x0600A09B RID: 41115 RVA: 0x0039740C File Offset: 0x0039560C
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				return "StatsReport_Occupied".Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x0600A09C RID: 41116 RVA: 0x0039745B File Offset: 0x0039565B
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_Occupied".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x0600A09D RID: 41117 RVA: 0x0039748B File Offset: 0x0039568B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x0600A09E RID: 41118 RVA: 0x00397494 File Offset: 0x00395694
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			if (!ModLister.CheckAnomaly("FocusStrengthOffset_OccupiedHoldingPlatform"))
			{
				return false;
			}
			Building_HoldingPlatform building_HoldingPlatform = parent as Building_HoldingPlatform;
			return building_HoldingPlatform != null && building_HoldingPlatform.Occupied;
		}
	}
}

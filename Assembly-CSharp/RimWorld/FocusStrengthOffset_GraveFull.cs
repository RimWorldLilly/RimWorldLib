using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018FE RID: 6398
	public class FocusStrengthOffset_GraveFull : FocusStrengthOffset
	{
		// Token: 0x0600A067 RID: 41063 RVA: 0x00396768 File Offset: 0x00394968
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				Building_Grave building_Grave = parent as Building_Grave;
				return "StatsReport_GraveFull".Translate(building_Grave.Corpse.InnerPawn.LabelShortCap) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x0600A068 RID: 41064 RVA: 0x003967D3 File Offset: 0x003949D3
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_GraveFullAbstract".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x0600A069 RID: 41065 RVA: 0x00396803 File Offset: 0x00394A03
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x0600A06A RID: 41066 RVA: 0x0039680C File Offset: 0x00394A0C
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			if (parent.Spawned)
			{
				Building_Grave building_Grave = parent as Building_Grave;
				if (building_Grave != null && building_Grave.HasCorpse)
				{
					return building_Grave.Corpse.InnerPawn.RaceProps.Humanlike;
				}
			}
			return false;
		}
	}
}

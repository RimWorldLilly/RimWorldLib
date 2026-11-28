using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020018FF RID: 6399
	public class FocusStrengthOffset_GraveCorpseRelationship : FocusStrengthOffset
	{
		// Token: 0x17001BA7 RID: 7079
		// (get) Token: 0x0600A06C RID: 41068 RVA: 0x00396852 File Offset: 0x00394A52
		public override bool DependsOnPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A06D RID: 41069 RVA: 0x00396855 File Offset: 0x00394A55
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_GraveCorpseRelatedAbstract".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x0600A06E RID: 41070 RVA: 0x00396885 File Offset: 0x00394A85
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x0600A06F RID: 41071 RVA: 0x00396890 File Offset: 0x00394A90
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			Building_Grave building_Grave = parent as Building_Grave;
			return parent.Spawned && building_Grave != null && building_Grave.HasCorpse && building_Grave.Corpse.InnerPawn.RaceProps.Humanlike && building_Grave.Corpse.InnerPawn.relations.PotentiallyRelatedPawns.Contains(user);
		}
	}
}

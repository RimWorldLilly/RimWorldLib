using System;

namespace Verse
{
	// Token: 0x020004B1 RID: 1201
	public class Hediff_Inhumanized : Hediff
	{
		// Token: 0x06002141 RID: 8513 RVA: 0x000C607E File Offset: 0x000C427E
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModsConfig.AnomalyActive)
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
			Find.StudyManager.UpdateStudiableCache(this.pawn, this.pawn.MapHeld);
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x000C60BB File Offset: 0x000C42BB
		public override void PostRemoved()
		{
			base.PostRemoved();
			Find.StudyManager.UpdateStudiableCache(this.pawn, this.pawn.MapHeld);
		}
	}
}

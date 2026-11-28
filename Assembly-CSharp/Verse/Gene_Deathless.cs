using System;

namespace Verse
{
	// Token: 0x020003E4 RID: 996
	public class Gene_Deathless : Gene
	{
		// Token: 0x06001D52 RID: 7506 RVA: 0x000B2F8C File Offset: 0x000B118C
		public override void PostRemove()
		{
			base.PostRemove();
			this.pawn.health.CheckForStateChange(null, null);
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x000B2FB9 File Offset: 0x000B11B9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSkillReductionTick, "lastSkillReductionTick", -99999, false);
		}

		// Token: 0x04001772 RID: 6002
		public int lastSkillReductionTick = -99999;
	}
}

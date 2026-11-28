using System;

namespace Verse
{
	// Token: 0x020004BE RID: 1214
	public class Hediff_PorcupineQuill : HediffWithComps
	{
		// Token: 0x060021D5 RID: 8661 RVA: 0x000C8E68 File Offset: 0x000C7068
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (this.pawn != null && this.pawn.RaceProps != null && !this.pawn.RaceProps.IsFlesh)
			{
				this.pawn.health.RemoveHediff(this);
			}
		}
	}
}

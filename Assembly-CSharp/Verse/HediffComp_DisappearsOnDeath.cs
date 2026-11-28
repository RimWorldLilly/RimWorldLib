using System;

namespace Verse
{
	// Token: 0x02000425 RID: 1061
	public class HediffComp_DisappearsOnDeath : HediffComp
	{
		// Token: 0x06001F06 RID: 7942 RVA: 0x000BD522 File Offset: 0x000BB722
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			base.Notify_PawnDied(dinfo, culprit);
			base.Pawn.health.RemoveHediff(this.parent);
		}
	}
}

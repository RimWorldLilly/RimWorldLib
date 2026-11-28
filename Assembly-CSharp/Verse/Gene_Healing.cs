using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003DE RID: 990
	public class Gene_Healing : Gene
	{
		// Token: 0x06001D2B RID: 7467 RVA: 0x000B24BF File Offset: 0x000B06BF
		public override void PostAdd()
		{
			base.PostAdd();
			this.ResetInterval();
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x000B24CD File Offset: 0x000B06CD
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.ticksToHeal -= delta;
			if (this.ticksToHeal <= 0)
			{
				HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(this.pawn, this.LabelCap);
				this.ResetInterval();
			}
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x000B2504 File Offset: 0x000B0704
		private void ResetInterval()
		{
			this.ticksToHeal = Gene_Healing.HealingIntervalTicksRange.RandomInRange;
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x000B2524 File Offset: 0x000B0724
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Gene_Healing.<GetGizmos>d__5 <GetGizmos>d__ = new Gene_Healing.<GetGizmos>d__5(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x000B2534 File Offset: 0x000B0734
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		// Token: 0x0400176C RID: 5996
		private int ticksToHeal;

		// Token: 0x0400176D RID: 5997
		private static readonly IntRange HealingIntervalTicksRange = new IntRange(900000, 1800000);
	}
}

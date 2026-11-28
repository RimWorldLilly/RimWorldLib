using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DF9 RID: 7673
	public class TransferableComparer_HitPointsPercentage : TransferableComparer
	{
		// Token: 0x0600C783 RID: 51075 RVA: 0x00494564 File Offset: 0x00492764
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x0600C784 RID: 51076 RVA: 0x00494588 File Offset: 0x00492788
		private float GetValueFor(Transferable t)
		{
			Thing anyThing = t.AnyThing;
			Pawn pawn = anyThing as Pawn;
			if (pawn != null)
			{
				return pawn.health.summaryHealth.SummaryHealthPercent;
			}
			if (!anyThing.def.useHitPoints || !anyThing.def.healthAffectsPrice)
			{
				return 1f;
			}
			return (float)anyThing.HitPoints / (float)anyThing.MaxHitPoints;
		}
	}
}

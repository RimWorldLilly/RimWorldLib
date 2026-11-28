using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000886 RID: 2182
	public class MentalState_SadisticRageTantrum : MentalState_TantrumRandom
	{
		// Token: 0x060043FC RID: 17404 RVA: 0x001AB0E3 File Offset: 0x001A92E3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hits, "hits", 0, false);
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x001AB0FD File Offset: 0x001A92FD
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x001AB11F File Offset: 0x001A931F
		protected override Predicate<Thing> GetCustomValidator()
		{
			return (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(this.pawn, x);
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x001AB12D File Offset: 0x001A932D
		public override void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
			base.Notify_AttackedTarget(hitTarget);
			if (this.target != null && hitTarget.Thing == this.target)
			{
				this.hits++;
				if (this.hits >= 7)
				{
					base.RecoverFromState();
				}
			}
		}

		// Token: 0x04002C31 RID: 11313
		private int hits;

		// Token: 0x04002C32 RID: 11314
		public const int MaxHits = 7;
	}
}

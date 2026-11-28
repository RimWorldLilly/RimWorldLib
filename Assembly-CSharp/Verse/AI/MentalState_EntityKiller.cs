using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000883 RID: 2179
	public class MentalState_EntityKiller : MentalState_SlaughterThing
	{
		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x060043F3 RID: 17395 RVA: 0x001AAFC0 File Offset: 0x001A91C0
		protected override bool SlaughterTargetAvailable
		{
			get
			{
				return AnomalyUtility.FindEntityOnPlatform(this.pawn.Map, EntityQueryType.ForSlaughter) != null;
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x060043F4 RID: 17396 RVA: 0x001AAFD6 File Offset: 0x001A91D6
		protected override int MinTicksBetweenSlaughter
		{
			get
			{
				return 625;
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x060043F5 RID: 17397 RVA: 0x001AAFDD File Offset: 0x001A91DD
		protected override int MaxThingsSlaughtered
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x001AAFE0 File Offset: 0x001A91E0
		public override void Notify_SlaughteredTarget()
		{
			base.Notify_SlaughteredTarget();
			if (this.pawn.MentalState == this && Rand.Chance(0.5f))
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x04002C2F RID: 11311
		private const float ChanceToRecoverAfterKill = 0.5f;
	}
}

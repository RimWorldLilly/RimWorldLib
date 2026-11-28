using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000875 RID: 2165
	public class MentalState_HumanityBreak : MentalState_WanderOwnRoom
	{
		// Token: 0x060043B3 RID: 17331 RVA: 0x001A9F5C File Offset: 0x001A815C
		public override void PostStart(string reason)
		{
			this.forceRecoverAfterTicks = MentalState_HumanityBreak.RecoveryDurationTicksRange.RandomInRange;
			this.ticksToRamble = MentalState_HumanityBreak.RambleTicksRange.RandomInRange;
			base.PostStart(reason);
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x001A9F98 File Offset: 0x001A8198
		public override void MentalStateTick(int delta)
		{
			if (this.pawn.Awake() && !this.pawn.Suspended)
			{
				if (this.pawn.IsHashIntervalTick(30, delta))
				{
					this.age += 30;
					if (this.age >= this.forceRecoverAfterTicks)
					{
						base.RecoverFromState();
						return;
					}
				}
				if (this.pawn.Spawned)
				{
					this.ticksToRamble--;
					if (this.ticksToRamble <= 0)
					{
						this.ticksToRamble = MentalState_HumanityBreak.RambleTicksRange.RandomInRange;
						SocialInteractionUtility.ImitateInteractionWithNoPawn(this.pawn, InteractionDefOf.InhumanRambling);
					}
				}
			}
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x001AA03D File Offset: 0x001A823D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToRamble, "ticksToRamble", 0, false);
		}

		// Token: 0x04002C10 RID: 11280
		private int ticksToRamble;

		// Token: 0x04002C11 RID: 11281
		private static readonly IntRange RecoveryDurationTicksRange = new IntRange(10000, 20000);

		// Token: 0x04002C12 RID: 11282
		private static readonly IntRange RambleTicksRange = new IntRange(1200, 6000);
	}
}

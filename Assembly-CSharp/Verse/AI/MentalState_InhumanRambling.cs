using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000876 RID: 2166
	public class MentalState_InhumanRambling : MentalState_WanderSad
	{
		// Token: 0x060043B8 RID: 17336 RVA: 0x001AA089 File Offset: 0x001A8289
		public override void PostStart(string reason)
		{
			this.ticksToRamble = this.def.ticksBetweenRamblingRange.RandomInRange;
			base.PostStart(reason);
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x001AA0A8 File Offset: 0x001A82A8
		public override void MentalStateTick(int delta)
		{
			if (this.pawn.Awake() && !this.pawn.Suspended && this.pawn.Spawned)
			{
				this.ticksToRamble--;
				if (this.ticksToRamble <= 0)
				{
					this.ticksToRamble = this.def.ticksBetweenRamblingRange.RandomInRange;
					MentalState_InhumanRambling.DoInhumanRambling(this.pawn);
				}
			}
			base.MentalStateTick(delta);
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x001AA11C File Offset: 0x001A831C
		public static void DoInhumanRambling(Pawn pawn)
		{
			SocialInteractionUtility.ImitateInteractionWithNoPawn(pawn, InteractionDefOf.InhumanRambling);
			GenClamor.DoClamor(pawn, 9.9f, delegate(Thing source, Pawn hearer)
			{
				if (hearer != source && hearer.needs.mood != null)
				{
					hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HeardInhumanRambling, pawn, null);
				}
			});
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x001AA162 File Offset: 0x001A8362
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToRamble, "ticksToRamble", 0, false);
		}

		// Token: 0x04002C13 RID: 11283
		private int ticksToRamble;

		// Token: 0x04002C14 RID: 11284
		private const float RambleEffectRadius = 9.9f;
	}
}

using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000868 RID: 2152
	public class MentalState_PanicFleeFire : MentalState
	{
		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06004382 RID: 17282 RVA: 0x001A9899 File Offset: 0x001A7A99
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x001A989C File Offset: 0x001A7A9C
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x001A98A0 File Offset: 0x001A7AA0
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if (this.pawn.IsHashIntervalTick(30, delta))
			{
				if (this.lastFireSeenTick < 0 || ThoughtWorker_Pyrophobia.NearFire(this.pawn))
				{
					this.lastFireSeenTick = Find.TickManager.TicksGame;
				}
				if (this.lastFireSeenTick >= 0 && Find.TickManager.TicksGame >= this.lastFireSeenTick + this.def.minTicksBeforeRecovery)
				{
					base.RecoverFromState();
				}
			}
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x001A9917 File Offset: 0x001A7B17
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastFireSeenTick, "lastFireSeenTick", -1, false);
		}

		// Token: 0x04002C08 RID: 11272
		private int lastFireSeenTick = -1;
	}
}

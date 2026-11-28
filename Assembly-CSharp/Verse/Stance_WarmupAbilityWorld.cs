using System;

namespace Verse
{
	// Token: 0x020004ED RID: 1261
	public class Stance_WarmupAbilityWorld : Stance_Warmup
	{
		// Token: 0x06002571 RID: 9585 RVA: 0x000E3B70 File Offset: 0x000E1D70
		public Stance_WarmupAbilityWorld()
		{
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x000E3B78 File Offset: 0x000E1D78
		public Stance_WarmupAbilityWorld(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x000E3B83 File Offset: 0x000E1D83
		protected override void Expire()
		{
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			if (this.stanceTracker.curStance == this)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
			}
		}
	}
}

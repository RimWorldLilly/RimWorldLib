using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000887 RID: 2183
	public abstract class MentalState_Tantrum : MentalState
	{
		// Token: 0x06004402 RID: 17410 RVA: 0x001AB180 File Offset: 0x001A9380
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.hitTargetAtLeastOnce, "hitTargetAtLeastOnce", false, false);
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x001AB1AB File Offset: 0x001A93AB
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x06004404 RID: 17412 RVA: 0x001AB1AE File Offset: 0x001A93AE
		public override void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
			base.Notify_AttackedTarget(hitTarget);
			if (this.target != null && hitTarget.Thing == this.target)
			{
				this.hitTargetAtLeastOnce = true;
			}
		}

		// Token: 0x04002C33 RID: 11315
		public Thing target;

		// Token: 0x04002C34 RID: 11316
		protected bool hitTargetAtLeastOnce;
	}
}

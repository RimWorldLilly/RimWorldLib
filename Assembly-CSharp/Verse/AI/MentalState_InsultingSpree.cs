using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000860 RID: 2144
	public abstract class MentalState_InsultingSpree : MentalState
	{
		// Token: 0x06004347 RID: 17223 RVA: 0x001A8BCE File Offset: 0x001A6DCE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.insultedTargetAtLeastOnce, "insultedTargetAtLeastOnce", false, false);
			Scribe_Values.Look<int>(ref this.lastInsultTicks, "lastInsultTicks", 0, false);
		}

		// Token: 0x06004348 RID: 17224 RVA: 0x001A8C0B File Offset: 0x001A6E0B
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04002BF3 RID: 11251
		public Pawn target;

		// Token: 0x04002BF4 RID: 11252
		public bool insultedTargetAtLeastOnce;

		// Token: 0x04002BF5 RID: 11253
		public int lastInsultTicks = -999999;
	}
}

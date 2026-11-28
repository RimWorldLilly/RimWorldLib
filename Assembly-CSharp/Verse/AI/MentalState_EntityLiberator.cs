using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000878 RID: 2168
	public class MentalState_EntityLiberator : MentalState
	{
		// Token: 0x060043BF RID: 17343 RVA: 0x001AA18F File Offset: 0x001A838F
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x060043C0 RID: 17344 RVA: 0x001AA192 File Offset: 0x001A8392
		public override void Notify_ReleasedTarget()
		{
			base.RecoverFromState();
		}
	}
}

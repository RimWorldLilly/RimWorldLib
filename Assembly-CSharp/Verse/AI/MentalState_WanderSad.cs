using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200086E RID: 2158
	public class MentalState_WanderSad : MentalState
	{
		// Token: 0x06004396 RID: 17302 RVA: 0x001A9A87 File Offset: 0x001A7C87
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

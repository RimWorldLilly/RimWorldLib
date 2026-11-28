using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200086D RID: 2157
	public class MentalState_WanderConfused : MentalState
	{
		// Token: 0x06004394 RID: 17300 RVA: 0x001A9A7C File Offset: 0x001A7C7C
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

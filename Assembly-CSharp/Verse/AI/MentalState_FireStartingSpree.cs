using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000870 RID: 2160
	public class MentalState_FireStartingSpree : MentalState
	{
		// Token: 0x0600439A RID: 17306 RVA: 0x001A9A9D File Offset: 0x001A7C9D
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

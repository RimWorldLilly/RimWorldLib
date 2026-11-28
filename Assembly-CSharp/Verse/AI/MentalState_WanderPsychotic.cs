using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200086F RID: 2159
	public class MentalState_WanderPsychotic : MentalState
	{
		// Token: 0x06004398 RID: 17304 RVA: 0x001A9A92 File Offset: 0x001A7C92
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

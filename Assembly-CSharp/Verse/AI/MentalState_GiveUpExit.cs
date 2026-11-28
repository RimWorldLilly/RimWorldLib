using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200086A RID: 2154
	public class MentalState_GiveUpExit : MentalState
	{
		// Token: 0x06004389 RID: 17289 RVA: 0x001A994B File Offset: 0x001A7B4B
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

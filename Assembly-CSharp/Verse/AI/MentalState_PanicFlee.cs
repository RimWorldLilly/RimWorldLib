using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000867 RID: 2151
	public class MentalState_PanicFlee : MentalState
	{
		// Token: 0x06004380 RID: 17280 RVA: 0x001A988E File Offset: 0x001A7A8E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

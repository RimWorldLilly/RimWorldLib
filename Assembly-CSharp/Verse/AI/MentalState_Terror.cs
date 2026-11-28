using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000869 RID: 2153
	public class MentalState_Terror : MentalState
	{
		// Token: 0x06004387 RID: 17287 RVA: 0x001A9940 File Offset: 0x001A7B40
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

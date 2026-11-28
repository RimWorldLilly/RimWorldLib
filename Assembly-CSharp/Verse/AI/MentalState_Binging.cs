using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000865 RID: 2149
	public class MentalState_Binging : MentalState
	{
		// Token: 0x06004375 RID: 17269 RVA: 0x001A957E File Offset: 0x001A777E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

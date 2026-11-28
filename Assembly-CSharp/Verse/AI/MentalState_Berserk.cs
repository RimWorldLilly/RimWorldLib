using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000864 RID: 2148
	public class MentalState_Berserk : MentalState
	{
		// Token: 0x06004371 RID: 17265 RVA: 0x001A956D File Offset: 0x001A776D
		public override bool ForceHostileTo(Thing t)
		{
			return true;
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x001A9570 File Offset: 0x001A7770
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x001A9573 File Offset: 0x001A7773
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

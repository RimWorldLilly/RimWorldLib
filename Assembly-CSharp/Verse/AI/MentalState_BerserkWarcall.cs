using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000874 RID: 2164
	public class MentalState_BerserkWarcall : MentalState
	{
		// Token: 0x060043AF RID: 17327 RVA: 0x001A9F20 File Offset: 0x001A8120
		public override bool ForceHostileTo(Thing t)
		{
			if (this.sourceFaction == null)
			{
				return t.HostileTo(this.pawn);
			}
			return t.HostileTo(this.sourceFaction);
		}

		// Token: 0x060043B0 RID: 17328 RVA: 0x001A9F43 File Offset: 0x001A8143
		public override bool ForceHostileTo(Faction f)
		{
			return f.HostileTo(this.sourceFaction);
		}

		// Token: 0x060043B1 RID: 17329 RVA: 0x001A9F51 File Offset: 0x001A8151
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200086C RID: 2156
	public class MentalState_WanderOwnRoom : MentalState
	{
		// Token: 0x06004390 RID: 17296 RVA: 0x001A99F0 File Offset: 0x001A7BF0
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			if (this.pawn.ownership.OwnedBed != null)
			{
				this.target = this.pawn.ownership.OwnedBed.Position;
				return;
			}
			this.target = this.pawn.Position;
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x001A9A44 File Offset: 0x001A7C44
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x001A9A71 File Offset: 0x001A7C71
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04002C09 RID: 11273
		public IntVec3 target;
	}
}

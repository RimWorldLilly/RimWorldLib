using System;

namespace Verse.AI
{
	// Token: 0x0200087D RID: 2173
	public class MentalState_CorpseObsession : MentalState
	{
		// Token: 0x060043D2 RID: 17362 RVA: 0x001AA832 File Offset: 0x001A8A32
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Corpse>(ref this.corpse, "corpse", false);
			Scribe_Values.Look<bool>(ref this.alreadyHauledCorpse, "alreadyHauledCorpse", false, false);
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x001AA860 File Offset: 0x001A8A60
		public override void MentalStateTick(int delta)
		{
			if (this.alreadyHauledCorpse)
			{
				base.MentalStateTick(delta);
				return;
			}
			bool flag = false;
			if (this.pawn.IsHashIntervalTick(500, delta) && !CorpseObsessionMentalStateUtility.IsCorpseValid(this.corpse, this.pawn, false))
			{
				this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
				if (this.corpse == null)
				{
					base.RecoverFromState();
					flag = true;
				}
			}
			if (!flag)
			{
				base.MentalStateTick(delta);
			}
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x001AA8D1 File Offset: 0x001A8AD1
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x001AA8EB File Offset: 0x001A8AEB
		public void Notify_CorpseHauled()
		{
			this.alreadyHauledCorpse = true;
		}

		// Token: 0x04002C20 RID: 11296
		public Corpse corpse;

		// Token: 0x04002C21 RID: 11297
		public bool alreadyHauledCorpse;

		// Token: 0x04002C22 RID: 11298
		private const int AnyCorpseStillValidCheckInterval = 500;
	}
}

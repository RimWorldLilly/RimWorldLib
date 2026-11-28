using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ADA RID: 2778
	public class JobGiver_AIFollowOverseer : JobGiver_AIFollowPawn
	{
		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x06005332 RID: 21298 RVA: 0x001EF896 File Offset: 0x001EDA96
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06005333 RID: 21299 RVA: 0x001EF89D File Offset: 0x001EDA9D
		protected override Pawn GetFollowee(Pawn pawn)
		{
			return pawn.GetOverseer();
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x001EF8A5 File Offset: 0x001EDAA5
		protected override float GetRadius(Pawn pawn)
		{
			return 5f;
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x001EF8AC File Offset: 0x001EDAAC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.GetOverseer() == null)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}
	}
}

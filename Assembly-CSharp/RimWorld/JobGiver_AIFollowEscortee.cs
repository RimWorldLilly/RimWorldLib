using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD8 RID: 2776
	public class JobGiver_AIFollowEscortee : JobGiver_AIFollowPawn
	{
		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x0600532A RID: 21290 RVA: 0x001EF800 File Offset: 0x001EDA00
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 120;
			}
		}

		// Token: 0x0600532B RID: 21291 RVA: 0x001EF804 File Offset: 0x001EDA04
		protected override Pawn GetFollowee(Pawn pawn)
		{
			return (Pawn)pawn.mindState.duty.focus.Thing;
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x001EF820 File Offset: 0x001EDA20
		protected override float GetRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}

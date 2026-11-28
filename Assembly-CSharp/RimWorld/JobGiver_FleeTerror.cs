using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B3F RID: 2879
	public class JobGiver_FleeTerror : ThinkNode_JobGiver
	{
		// Token: 0x06005473 RID: 21619 RVA: 0x001F6208 File Offset: 0x001F4408
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState mentalState = pawn.MentalState;
			Pawn pawn2 = (mentalState != null) ? mentalState.causedByPawn : null;
			if (pawn2 == null)
			{
				return null;
			}
			return FleeUtility.FleeJob(pawn, pawn2, Mathf.CeilToInt(this.fleeDistRange.RandomInRange));
		}

		// Token: 0x04003280 RID: 12928
		private FloatRange fleeDistRange = new FloatRange(50f, 75f);
	}
}

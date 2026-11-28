using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D50 RID: 3408
	public class ThinkNode_ConditionalShouldFollowMaster : ThinkNode_Conditional
	{
		// Token: 0x06005F1E RID: 24350 RVA: 0x0022B9D3 File Offset: 0x00229BD3
		protected override bool Satisfied(Pawn pawn)
		{
			return ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn);
		}

		// Token: 0x06005F1F RID: 24351 RVA: 0x0022B9DC File Offset: 0x00229BDC
		public static bool ShouldFollowMaster(Pawn pawn)
		{
			if (!pawn.Spawned || pawn.playerSettings == null)
			{
				return false;
			}
			Pawn respectedMaster = pawn.playerSettings.RespectedMaster;
			if (respectedMaster == null)
			{
				return false;
			}
			if (respectedMaster.Spawned)
			{
				if (pawn.playerSettings.followDrafted && respectedMaster.Drafted && pawn.CanReach(respectedMaster, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
				if (pawn.playerSettings.followFieldwork && respectedMaster.mindState.lastJobTag == JobTag.Fieldwork && pawn.CanReach(respectedMaster, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			else
			{
				Pawn carriedBy = respectedMaster.CarriedBy;
				if (carriedBy != null && carriedBy.HostileTo(respectedMaster) && pawn.CanReach(carriedBy, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}

using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CFC RID: 3324
	public class LordToil_StealCover : LordToil_DoOpportunisticTaskOrCover
	{
		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x06005DB6 RID: 23990 RVA: 0x0022549F File Offset: 0x0022369F
		protected override DutyDef DutyDef
		{
			get
			{
				return DutyDefOf.Steal;
			}
		}

		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x06005DB7 RID: 23991 RVA: 0x002254A6 File Offset: 0x002236A6
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x06005DB8 RID: 23992 RVA: 0x002254A9 File Offset: 0x002236A9
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005DB9 RID: 23993 RVA: 0x002254AC File Offset: 0x002236AC
		protected override bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets)
		{
			if (pawn.mindState.duty != null && pawn.mindState.duty.def == this.DutyDef && pawn.carryTracker.CarriedThing != null)
			{
				target = pawn.carryTracker.CarriedThing;
				return true;
			}
			return StealAIUtility.TryFindBestItemToSteal(pawn.Position, pawn.Map, 7f, out target, pawn, alreadyTakenTargets);
		}
	}
}

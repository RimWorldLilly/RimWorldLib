using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CE5 RID: 3301
	public class LordToil_KidnapCover : LordToil_DoOpportunisticTaskOrCover
	{
		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x06005D43 RID: 23875 RVA: 0x0022345B File Offset: 0x0022165B
		protected override DutyDef DutyDef
		{
			get
			{
				return DutyDefOf.Kidnap;
			}
		}

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x06005D44 RID: 23876 RVA: 0x00223462 File Offset: 0x00221662
		public override bool ForceHighStoryDanger
		{
			get
			{
				return this.cover;
			}
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x06005D45 RID: 23877 RVA: 0x0022346A File Offset: 0x0022166A
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005D46 RID: 23878 RVA: 0x00223470 File Offset: 0x00221670
		protected override bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets)
		{
			if (pawn.mindState.duty != null && pawn.mindState.duty.def == this.DutyDef && pawn.carryTracker.CarriedThing is Pawn)
			{
				target = pawn.carryTracker.CarriedThing;
				return true;
			}
			Pawn pawn2;
			bool result = KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 8f, out pawn2, alreadyTakenTargets);
			target = pawn2;
			return result;
		}
	}
}

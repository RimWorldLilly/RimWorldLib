using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D00 RID: 3328
	public class LordToil_VoidAwakeningWander : LordToil
	{
		// Token: 0x06005DC7 RID: 24007 RVA: 0x002258E0 File Offset: 0x00223AE0
		public override void UpdateAllDuties()
		{
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				this.UpdateDefendPoint(pawn);
			}
		}

		// Token: 0x06005DC8 RID: 24008 RVA: 0x00225938 File Offset: 0x00223B38
		public override void LordToilTick()
		{
			for (int i = this.lord.ownedPawns.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsHashIntervalTick(1800))
				{
					this.UpdateDefendPoint(pawn);
				}
				if (pawn.mindState.enemyTarget != null)
				{
					this.lord.RemovePawn(pawn);
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x06005DC9 RID: 24009 RVA: 0x002259B0 File Offset: 0x00223BB0
		private void UpdateDefendPoint(Pawn pawn)
		{
			if (pawn.mindState == null)
			{
				return;
			}
			IntVec3 c;
			if (!CellFinder.TryFindRandomReachableNearbyCell(pawn.Position, pawn.Map, 50f, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), null, null, out c, 999999))
			{
				return;
			}
			pawn.mindState.duty = new PawnDuty(DutyDefOf.VoidAwakeningWander, c, -1f);
			pawn.mindState.duty.wanderRadius = new float?(12f);
		}

		// Token: 0x04003538 RID: 13624
		private const float WanderRadius = 12f;

		// Token: 0x04003539 RID: 13625
		private const float NextDefendPointRadius = 50f;

		// Token: 0x0400353A RID: 13626
		private const int SwitchDefendPointInterval = 1800;
	}
}

using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200154E RID: 5454
	public class AutoUndrafter : IExposable
	{
		// Token: 0x06008543 RID: 34115 RVA: 0x00300B89 File Offset: 0x002FED89
		public AutoUndrafter(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008544 RID: 34116 RVA: 0x00300B98 File Offset: 0x002FED98
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastNonWaitingTick, "lastNonWaitingTick", 0, false);
		}

		// Token: 0x06008545 RID: 34117 RVA: 0x00300BAC File Offset: 0x002FEDAC
		public void AutoUndraftTickInterval(int delta)
		{
			if (GenTicks.IsTickIntervalDelta(100, delta) || !this.pawn.Drafted)
			{
				return;
			}
			if ((this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.def != JobDefOf.Wait_Combat) || this.AnyHostilePreventingAutoUndraft())
			{
				this.lastNonWaitingTick = Find.TickManager.TicksGame;
			}
			if (this.ShouldAutoUndraft())
			{
				this.pawn.drafter.Drafted = false;
			}
		}

		// Token: 0x06008546 RID: 34118 RVA: 0x00300C30 File Offset: 0x002FEE30
		public void Notify_Drafted()
		{
			this.lastNonWaitingTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06008547 RID: 34119 RVA: 0x00300C42 File Offset: 0x002FEE42
		private bool ShouldAutoUndraft()
		{
			return !this.pawn.IsColonyMech && Find.TickManager.TicksGame - this.lastNonWaitingTick >= 10000 && !this.AnyHostilePreventingAutoUndraft();
		}

		// Token: 0x06008548 RID: 34120 RVA: 0x00300C78 File Offset: 0x002FEE78
		private bool AnyHostilePreventingAutoUndraft()
		{
			List<IAttackTarget> potentialTargetsFor = this.pawn.Map.attackTargetsCache.GetPotentialTargetsFor(this.pawn);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				if (GenHostility.IsActiveThreatToPlayer(potentialTargetsFor[i], false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005206 RID: 20998
		private Pawn pawn;

		// Token: 0x04005207 RID: 20999
		private int lastNonWaitingTick;

		// Token: 0x04005208 RID: 21000
		private const int UndraftDelay = 10000;
	}
}

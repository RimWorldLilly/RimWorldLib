using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CF9 RID: 3321
	public class LordToil_SpawnHateChantersDelayed : LordToil
	{
		// Token: 0x06005DAB RID: 23979 RVA: 0x00225125 File Offset: 0x00223325
		public LordToil_SpawnHateChantersDelayed(List<Pawn> pawns, string readySignal, int batchSize, float spawnInterval)
		{
			this.pawns = pawns;
			this.readySignal = readySignal;
			this.batchSize = batchSize;
			this.spawnInterval = spawnInterval;
		}

		// Token: 0x06005DAC RID: 23980 RVA: 0x0022514C File Offset: 0x0022334C
		public override void Init()
		{
			base.Init();
			foreach (Pawn pawn in this.pawns)
			{
				if (((pawn != null) ? pawn.mindState : null) != null)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.Idle);
					pawn.mindState.Active = false;
				}
			}
			this.spawnRequest = base.Map.deferredSpawner.GetRequestByLord(this.lord);
			if (this.spawnRequest == null)
			{
				this.spawnRequest = new SpawnRequest(this.pawns.Cast<Thing>().ToList<Thing>(), this.batchSize, this.spawnInterval, this.lord);
				base.Map.deferredSpawner.AddRequest(this.spawnRequest, true);
			}
			else
			{
				this.spawnedCount = this.spawnRequest.spawnedThings.Count;
			}
			this.spawnRequested = true;
		}

		// Token: 0x06005DAD RID: 23981 RVA: 0x00225254 File Offset: 0x00223454
		public override void UpdateAllDuties()
		{
		}

		// Token: 0x06005DAE RID: 23982 RVA: 0x00225258 File Offset: 0x00223458
		public override void LordToilTick()
		{
			base.LordToilTick();
			if (!this.spawnRequested)
			{
				this.spawnRequest = base.Map.deferredSpawner.GetRequestByLord(this.lord);
				base.Map.deferredSpawner.AddRequest(this.spawnRequest, true);
				this.spawnRequested = true;
			}
			int count = this.spawnRequest.spawnedThings.Count;
			if (count > this.spawnedCount)
			{
				for (int i = this.spawnedCount; i < count; i++)
				{
					Hediff_DeathRefusal firstHediff = (this.spawnRequest.spawnedThings[i] as Pawn).health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
					if (firstHediff != null)
					{
						firstHediff.AIEnabled = false;
					}
				}
				this.spawnedCount = count;
			}
			if (this.spawnRequest.done)
			{
				foreach (Thing thing in this.spawnRequest.spawnedThings)
				{
					Pawn pawn = (Pawn)thing;
					if (pawn.mindState != null)
					{
						pawn.mindState.Active = true;
						pawn.mindState.duty = new PawnDuty(DutyDefOf.Idle);
					}
				}
				Find.SignalManager.SendSignal(new Signal(this.readySignal, false));
			}
		}

		// Token: 0x0400352A RID: 13610
		private readonly string readySignal;

		// Token: 0x0400352B RID: 13611
		private readonly int batchSize;

		// Token: 0x0400352C RID: 13612
		private readonly float spawnInterval;

		// Token: 0x0400352D RID: 13613
		private readonly List<Pawn> pawns;

		// Token: 0x0400352E RID: 13614
		private int spawnedCount;

		// Token: 0x0400352F RID: 13615
		private bool spawnRequested;

		// Token: 0x04003530 RID: 13616
		private SpawnRequest spawnRequest;
	}
}

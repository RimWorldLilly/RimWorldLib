using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D26 RID: 3366
	public class Trigger_KidnapVictimPresent : Trigger
	{
		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x06005E61 RID: 24161 RVA: 0x00227E7D File Offset: 0x0022607D
		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

		// Token: 0x06005E62 RID: 24162 RVA: 0x00227E8A File Offset: 0x0022608A
		public Trigger_KidnapVictimPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x06005E63 RID: 24163 RVA: 0x00227EA0 File Offset: 0x002260A0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 120 == 0)
			{
				if (this.data == null || !(this.data is TriggerData_PawnCycleInd))
				{
					BackCompatibility.TriggerDataPawnCycleIndNull(this);
				}
				if (Find.TickManager.TicksGame - lord.lastPawnHarmTick > 300)
				{
					TriggerData_PawnCycleInd data = this.Data;
					data.pawnCycleInd++;
					if (data.pawnCycleInd >= lord.ownedPawns.Count)
					{
						data.pawnCycleInd = 0;
					}
					if (lord.ownedPawns.Any<Pawn>())
					{
						Pawn pawn = lord.ownedPawns[data.pawnCycleInd];
						Pawn pawn2;
						if (pawn.Spawned && !pawn.Downed && pawn.MentalStateDef == null && KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 8f, out pawn2, null) && !GenAI.InDangerousCombat(pawn))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0400357B RID: 13691
		private const int CheckInterval = 120;

		// Token: 0x0400357C RID: 13692
		private const int MinTicksSinceDamage = 300;
	}
}

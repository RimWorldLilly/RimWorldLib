using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D28 RID: 3368
	public class Trigger_WoundedGuestPresent : Trigger
	{
		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x06005E66 RID: 24166 RVA: 0x00227F9E File Offset: 0x0022619E
		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

		// Token: 0x06005E67 RID: 24167 RVA: 0x00227FAB File Offset: 0x002261AB
		public Trigger_WoundedGuestPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x06005E68 RID: 24168 RVA: 0x00227FC0 File Offset: 0x002261C0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 800 == 0)
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
					if (pawn.Spawned && !pawn.Downed && !pawn.InMentalState && KidnapAIUtility.ReachableWoundedGuest(pawn) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0400357E RID: 13694
		private const int CheckInterval = 800;
	}
}

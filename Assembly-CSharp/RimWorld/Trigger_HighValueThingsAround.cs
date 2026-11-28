using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D25 RID: 3365
	public class Trigger_HighValueThingsAround : Trigger
	{
		// Token: 0x06005E5F RID: 24159 RVA: 0x00227E18 File Offset: 0x00226018
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 120 == 0)
			{
				if (TutorSystem.TutorialMode)
				{
					return false;
				}
				if (Find.TickManager.TicksGame - lord.lastPawnHarmTick > 300)
				{
					float num = StealAIUtility.TotalMarketValueAround(lord.ownedPawns);
					float num2 = StealAIUtility.StartStealingMarketValueThreshold(lord);
					return num > num2;
				}
			}
			return false;
		}

		// Token: 0x04003579 RID: 13689
		private const int CheckInterval = 120;

		// Token: 0x0400357A RID: 13690
		private const int MinTicksSinceDamage = 300;
	}
}

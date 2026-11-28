using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000987 RID: 2439
	public class Trigger_ImportantTraderCaravanPeopleLost : Trigger
	{
		// Token: 0x060049BA RID: 18874 RVA: 0x001CB480 File Offset: 0x001C9680
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost && (signal.condition == PawnLostCondition.Incapped || signal.condition == PawnLostCondition.MadePrisoner || signal.condition == PawnLostCondition.Killed))
			{
				if (signal.Pawn.GetTraderCaravanRole() == TraderCaravanRole.Trader || signal.Pawn.RaceProps.packAnimal)
				{
					return true;
				}
				if (lord.numPawnsLostViolently > 0 && (float)lord.numPawnsLostViolently / (float)lord.numPawnsEverGained >= 0.5f)
				{
					return true;
				}
			}
			return false;
		}
	}
}

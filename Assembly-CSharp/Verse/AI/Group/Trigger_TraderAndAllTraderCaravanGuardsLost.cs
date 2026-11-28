using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000988 RID: 2440
	public class Trigger_TraderAndAllTraderCaravanGuardsLost : Trigger
	{
		// Token: 0x060049BC RID: 18876 RVA: 0x001CB500 File Offset: 0x001C9700
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					TraderCaravanRole traderCaravanRole = lord.ownedPawns[i].GetTraderCaravanRole();
					if (traderCaravanRole == TraderCaravanRole.Trader || traderCaravanRole == TraderCaravanRole.Guard)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
	}
}

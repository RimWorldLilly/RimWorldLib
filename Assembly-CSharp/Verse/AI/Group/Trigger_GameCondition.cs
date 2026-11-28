using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x0200098B RID: 2443
	public class Trigger_GameCondition : Trigger
	{
		// Token: 0x060049C2 RID: 18882 RVA: 0x001CB6D4 File Offset: 0x001C98D4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 199 == 0)
			{
				foreach (GameConditionDef def in this.gameConditions)
				{
					if (lord.Map.GameConditionManager.ConditionIsActive(def))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x04002F4E RID: 12110
		public List<GameConditionDef> gameConditions;
	}
}

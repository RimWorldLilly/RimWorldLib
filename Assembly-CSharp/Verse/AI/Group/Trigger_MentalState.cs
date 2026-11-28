using System;

namespace Verse.AI.Group
{
	// Token: 0x0200097D RID: 2429
	public class Trigger_MentalState : Trigger
	{
		// Token: 0x060049A6 RID: 18854 RVA: 0x001CAFEC File Offset: 0x001C91EC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].InMentalState)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}

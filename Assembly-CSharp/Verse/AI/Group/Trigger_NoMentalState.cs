using System;

namespace Verse.AI.Group
{
	// Token: 0x0200097E RID: 2430
	public class Trigger_NoMentalState : Trigger
	{
		// Token: 0x060049A8 RID: 18856 RVA: 0x001CB038 File Offset: 0x001C9238
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].InMentalState)
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

using System;

namespace Verse.AI.Group
{
	// Token: 0x02000979 RID: 2425
	public class Trigger_PawnCanReachMapEdge : Trigger
	{
		// Token: 0x0600499C RID: 18844 RVA: 0x001CAE04 File Offset: 0x001C9004
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 193 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed && !pawn.CanReachMapEdge())
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

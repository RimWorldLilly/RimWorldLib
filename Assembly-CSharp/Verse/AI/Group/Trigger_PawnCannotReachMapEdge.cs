using System;

namespace Verse.AI.Group
{
	// Token: 0x02000978 RID: 2424
	public class Trigger_PawnCannotReachMapEdge : Trigger
	{
		// Token: 0x0600499A RID: 18842 RVA: 0x001CAD8C File Offset: 0x001C8F8C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 197 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed && !pawn.CanReachMapEdge())
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}

using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D24 RID: 3364
	public class Trigger_GameEnding : Trigger
	{
		// Token: 0x06005E5D RID: 24157 RVA: 0x00227D7E File Offset: 0x00225F7E
		public Trigger_GameEnding(int triggerAfterTicks = 2500)
		{
			this.triggerAfterTicks = triggerAfterTicks;
		}

		// Token: 0x06005E5E RID: 24158 RVA: 0x00227D9C File Offset: 0x00225F9C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				if (this.requireActivePawns && !lord.AnyActivePawn)
				{
					return false;
				}
				if (Find.GameEnder.gameEnding)
				{
					if (this.gameEndingSinceTick == -1)
					{
						this.gameEndingSinceTick = Find.TickManager.TicksGame;
					}
				}
				else
				{
					this.gameEndingSinceTick = -1;
				}
				if (this.gameEndingSinceTick > 0 && Find.TickManager.TicksGame - this.gameEndingSinceTick > this.triggerAfterTicks)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003576 RID: 13686
		private int triggerAfterTicks;

		// Token: 0x04003577 RID: 13687
		private bool requireActivePawns = true;

		// Token: 0x04003578 RID: 13688
		private int gameEndingSinceTick = -1;
	}
}

using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x0200097C RID: 2428
	public class Trigger_PawnKilled : Trigger
	{
		// Token: 0x060049A3 RID: 18851 RVA: 0x001CAF74 File Offset: 0x001C9174
		public Trigger_PawnKilled()
		{
		}

		// Token: 0x060049A4 RID: 18852 RVA: 0x001CAF7C File Offset: 0x001C917C
		public Trigger_PawnKilled(List<Pawn> exceptions)
		{
			this.exceptions = exceptions;
		}

		// Token: 0x060049A5 RID: 18853 RVA: 0x001CAF8C File Offset: 0x001C918C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && ((signal.condition == PawnLostCondition.Killed || signal.condition == PawnLostCondition.Incapped) && signal.Pawn.Dead) && (this.exceptions.NullOrEmpty<Pawn>() || !this.exceptions.Contains(signal.Pawn));
		}

		// Token: 0x04002F43 RID: 12099
		private List<Pawn> exceptions;
	}
}

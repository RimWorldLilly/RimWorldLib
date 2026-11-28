using System;

namespace Verse.AI.Group
{
	// Token: 0x0200097A RID: 2426
	public class Trigger_PawnLost : Trigger
	{
		// Token: 0x0600499E RID: 18846 RVA: 0x001CAE7C File Offset: 0x001C907C
		public Trigger_PawnLost(PawnLostCondition condition = PawnLostCondition.Undefined, Pawn pawn = null)
		{
			this.condition = condition;
			this.pawn = pawn;
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x001CAE92 File Offset: 0x001C9092
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (this.condition == PawnLostCondition.Undefined || signal.condition == this.condition) && (this.pawn == null || this.pawn == signal.Pawn);
		}

		// Token: 0x04002F3F RID: 12095
		private Pawn pawn;

		// Token: 0x04002F40 RID: 12096
		private PawnLostCondition condition;
	}
}

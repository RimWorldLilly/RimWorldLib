using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D63 RID: 3427
	public class ThinkNode_ConditionalCloseToDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x06005F47 RID: 24391 RVA: 0x0022BDE5 File Offset: 0x00229FE5
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCloseToDutyTarget thinkNode_ConditionalCloseToDutyTarget = (ThinkNode_ConditionalCloseToDutyTarget)base.DeepCopy(resolve);
			thinkNode_ConditionalCloseToDutyTarget.maxDistToDutyTarget = this.maxDistToDutyTarget;
			return thinkNode_ConditionalCloseToDutyTarget;
		}

		// Token: 0x06005F48 RID: 24392 RVA: 0x0022BE00 File Offset: 0x0022A000
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty.focus.IsValid && pawn.Position.InHorDistOf(pawn.mindState.duty.focus.Cell, this.maxDistToDutyTarget);
		}

		// Token: 0x040035BF RID: 13759
		private float maxDistToDutyTarget = 10f;
	}
}

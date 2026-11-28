using System;

namespace Verse.AI
{
	// Token: 0x020008D5 RID: 2261
	public class ThinkNode_ConditionalHasDuty : ThinkNode_Conditional
	{
		// Token: 0x060045D1 RID: 17873 RVA: 0x001B7696 File Offset: 0x001B5896
		protected override bool Satisfied(Pawn pawn)
		{
			Pawn_MindState mindState = pawn.mindState;
			DutyDef dutyDef;
			if (mindState == null)
			{
				dutyDef = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				dutyDef = ((duty != null) ? duty.def : null);
			}
			return dutyDef == this.dutyDef;
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x001B76BE File Offset: 0x001B58BE
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalHasDuty thinkNode_ConditionalHasDuty = (ThinkNode_ConditionalHasDuty)base.DeepCopy(resolve);
			thinkNode_ConditionalHasDuty.dutyDef = this.dutyDef;
			return thinkNode_ConditionalHasDuty;
		}

		// Token: 0x04002D58 RID: 11608
		public DutyDef dutyDef;
	}
}

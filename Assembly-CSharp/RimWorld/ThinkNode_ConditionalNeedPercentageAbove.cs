using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D5E RID: 3422
	public class ThinkNode_ConditionalNeedPercentageAbove : ThinkNode_Conditional
	{
		// Token: 0x06005F3C RID: 24380 RVA: 0x0022BCA1 File Offset: 0x00229EA1
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalNeedPercentageAbove thinkNode_ConditionalNeedPercentageAbove = (ThinkNode_ConditionalNeedPercentageAbove)base.DeepCopy(resolve);
			thinkNode_ConditionalNeedPercentageAbove.need = this.need;
			thinkNode_ConditionalNeedPercentageAbove.threshold = this.threshold;
			return thinkNode_ConditionalNeedPercentageAbove;
		}

		// Token: 0x06005F3D RID: 24381 RVA: 0x0022BCC8 File Offset: 0x00229EC8
		protected override bool Satisfied(Pawn pawn)
		{
			Need need;
			if (!pawn.needs.TryGetNeed(this.need, out need))
			{
				return false;
			}
			if (need is Need_Seeker)
			{
				return need.CurInstantLevelPercentage > this.threshold;
			}
			return need.CurLevelPercentage > this.threshold;
		}

		// Token: 0x040035BD RID: 13757
		private NeedDef need;

		// Token: 0x040035BE RID: 13758
		private float threshold;
	}
}

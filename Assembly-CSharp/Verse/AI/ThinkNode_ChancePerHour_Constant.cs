using System;

namespace Verse.AI
{
	// Token: 0x020008B8 RID: 2232
	public class ThinkNode_ChancePerHour_Constant : ThinkNode_ChancePerHour
	{
		// Token: 0x0600458C RID: 17804 RVA: 0x001B7073 File Offset: 0x001B5273
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ChancePerHour_Constant thinkNode_ChancePerHour_Constant = (ThinkNode_ChancePerHour_Constant)base.DeepCopy(resolve);
			thinkNode_ChancePerHour_Constant.mtbHours = this.mtbHours;
			thinkNode_ChancePerHour_Constant.mtbDays = this.mtbDays;
			return thinkNode_ChancePerHour_Constant;
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x001B7099 File Offset: 0x001B5299
		protected override float MtbHours(Pawn Pawn)
		{
			if (this.mtbDays > 0f)
			{
				return this.mtbDays * 24f;
			}
			return this.mtbHours;
		}

		// Token: 0x04002D4C RID: 11596
		private float mtbHours = -1f;

		// Token: 0x04002D4D RID: 11597
		private float mtbDays = -1f;
	}
}

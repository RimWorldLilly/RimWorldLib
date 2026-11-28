using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D83 RID: 3459
	public class ThinkNode_ConditionalHashIntervalTick : ThinkNode_Conditional
	{
		// Token: 0x06005F8D RID: 24461 RVA: 0x0022C851 File Offset: 0x0022AA51
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsHashIntervalTick(this.interval);
		}

		// Token: 0x06005F8E RID: 24462 RVA: 0x0022C85F File Offset: 0x0022AA5F
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalHashIntervalTick thinkNode_ConditionalHashIntervalTick = (ThinkNode_ConditionalHashIntervalTick)base.DeepCopy(resolve);
			thinkNode_ConditionalHashIntervalTick.interval = this.interval;
			return thinkNode_ConditionalHashIntervalTick;
		}

		// Token: 0x040035CC RID: 13772
		public int interval;
	}
}

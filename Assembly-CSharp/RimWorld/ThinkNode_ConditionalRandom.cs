using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D68 RID: 3432
	public class ThinkNode_ConditionalRandom : ThinkNode_Conditional
	{
		// Token: 0x06005F52 RID: 24402 RVA: 0x0022C009 File Offset: 0x0022A209
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalRandom thinkNode_ConditionalRandom = (ThinkNode_ConditionalRandom)base.DeepCopy(resolve);
			thinkNode_ConditionalRandom.chance = this.chance;
			return thinkNode_ConditionalRandom;
		}

		// Token: 0x06005F53 RID: 24403 RVA: 0x0022C023 File Offset: 0x0022A223
		protected override bool Satisfied(Pawn pawn)
		{
			return Rand.Value < this.chance;
		}

		// Token: 0x040035C0 RID: 13760
		public float chance = 0.5f;
	}
}

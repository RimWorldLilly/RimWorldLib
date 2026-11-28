using System;

namespace Verse.AI
{
	// Token: 0x020008C5 RID: 2245
	public class ThinkNode_ConditionalCapableOfWorkTag : ThinkNode_Conditional
	{
		// Token: 0x060045AC RID: 17836 RVA: 0x001B733E File Offset: 0x001B553E
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCapableOfWorkTag thinkNode_ConditionalCapableOfWorkTag = (ThinkNode_ConditionalCapableOfWorkTag)base.DeepCopy(resolve);
			thinkNode_ConditionalCapableOfWorkTag.workTags = this.workTags;
			return thinkNode_ConditionalCapableOfWorkTag;
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x001B7358 File Offset: 0x001B5558
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.WorkTagIsDisabled(this.workTags);
		}

		// Token: 0x04002D53 RID: 11603
		public WorkTags workTags;
	}
}

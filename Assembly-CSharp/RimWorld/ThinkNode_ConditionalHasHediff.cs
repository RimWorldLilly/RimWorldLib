using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D77 RID: 3447
	public class ThinkNode_ConditionalHasHediff : ThinkNode_Conditional
	{
		// Token: 0x06005F72 RID: 24434 RVA: 0x0022C4EF File Offset: 0x0022A6EF
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalHasHediff thinkNode_ConditionalHasHediff = (ThinkNode_ConditionalHasHediff)base.DeepCopy(resolve);
			thinkNode_ConditionalHasHediff.hediff = this.hediff;
			thinkNode_ConditionalHasHediff.severityRange = this.severityRange;
			return thinkNode_ConditionalHasHediff;
		}

		// Token: 0x06005F73 RID: 24435 RVA: 0x0022C518 File Offset: 0x0022A718
		protected override bool Satisfied(Pawn pawn)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.hediff, false);
			return firstHediffOfDef != null && firstHediffOfDef.Severity >= this.severityRange.RandomInRange;
		}

		// Token: 0x040035C3 RID: 13763
		public HediffDef hediff;

		// Token: 0x040035C4 RID: 13764
		public FloatRange severityRange = FloatRange.Zero;
	}
}

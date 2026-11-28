using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D6F RID: 3439
	public class ThinkNode_ConditionalPawnKind : ThinkNode_Conditional
	{
		// Token: 0x06005F61 RID: 24417 RVA: 0x0022C1D0 File Offset: 0x0022A3D0
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalPawnKind thinkNode_ConditionalPawnKind = (ThinkNode_ConditionalPawnKind)base.DeepCopy(resolve);
			thinkNode_ConditionalPawnKind.pawnKind = this.pawnKind;
			return thinkNode_ConditionalPawnKind;
		}

		// Token: 0x06005F62 RID: 24418 RVA: 0x0022C1EA File Offset: 0x0022A3EA
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.kindDef == this.pawnKind;
		}

		// Token: 0x040035C1 RID: 13761
		public PawnKindDef pawnKind;
	}
}

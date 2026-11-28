using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D3C RID: 3388
	public class ThinkNode_ConditionalIsCrawling : ThinkNode_Conditional
	{
		// Token: 0x06005EF5 RID: 24309 RVA: 0x0022B6E4 File Offset: 0x002298E4
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Crawling;
		}
	}
}

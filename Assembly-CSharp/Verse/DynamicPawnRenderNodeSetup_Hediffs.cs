using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x020003A2 RID: 930
	public class DynamicPawnRenderNodeSetup_Hediffs : DynamicPawnRenderNodeSetup
	{
		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001C22 RID: 7202 RVA: 0x000AE320 File Offset: 0x000AC520
		public override bool HumanlikeOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x000AE323 File Offset: 0x000AC523
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		public override IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
		{
			DynamicPawnRenderNodeSetup_Hediffs.<GetDynamicNodes>d__2 <GetDynamicNodes>d__ = new DynamicPawnRenderNodeSetup_Hediffs.<GetDynamicNodes>d__2(-2);
			<GetDynamicNodes>d__.<>3__pawn = pawn;
			<GetDynamicNodes>d__.<>3__tree = tree;
			return <GetDynamicNodes>d__;
		}
	}
}

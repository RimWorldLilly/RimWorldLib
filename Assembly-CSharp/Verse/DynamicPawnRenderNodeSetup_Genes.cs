using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x020003A1 RID: 929
	public class DynamicPawnRenderNodeSetup_Genes : DynamicPawnRenderNodeSetup
	{
		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001C1F RID: 7199 RVA: 0x000AE2FE File Offset: 0x000AC4FE
		public override bool HumanlikeOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x000AE301 File Offset: 0x000AC501
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		public override IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
		{
			DynamicPawnRenderNodeSetup_Genes.<GetDynamicNodes>d__2 <GetDynamicNodes>d__ = new DynamicPawnRenderNodeSetup_Genes.<GetDynamicNodes>d__2(-2);
			<GetDynamicNodes>d__.<>3__pawn = pawn;
			<GetDynamicNodes>d__.<>3__tree = tree;
			return <GetDynamicNodes>d__;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x020003A4 RID: 932
	public class DynamicPawnRenderNodeSetup_Traits : DynamicPawnRenderNodeSetup
	{
		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001C28 RID: 7208 RVA: 0x000AE364 File Offset: 0x000AC564
		public override bool HumanlikeOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x000AE367 File Offset: 0x000AC567
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		public override IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
		{
			DynamicPawnRenderNodeSetup_Traits.<GetDynamicNodes>d__2 <GetDynamicNodes>d__ = new DynamicPawnRenderNodeSetup_Traits.<GetDynamicNodes>d__2(-2);
			<GetDynamicNodes>d__.<>3__pawn = pawn;
			<GetDynamicNodes>d__.<>3__tree = tree;
			return <GetDynamicNodes>d__;
		}
	}
}

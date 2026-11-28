using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x020003A3 RID: 931
	public class DynamicPawnRenderNodeSetup_Mutants : DynamicPawnRenderNodeSetup
	{
		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001C25 RID: 7205 RVA: 0x000AE342 File Offset: 0x000AC542
		public override bool HumanlikeOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x000AE345 File Offset: 0x000AC545
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		public override IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
		{
			DynamicPawnRenderNodeSetup_Mutants.<GetDynamicNodes>d__2 <GetDynamicNodes>d__ = new DynamicPawnRenderNodeSetup_Mutants.<GetDynamicNodes>d__2(-2);
			<GetDynamicNodes>d__.<>3__pawn = pawn;
			<GetDynamicNodes>d__.<>3__tree = tree;
			return <GetDynamicNodes>d__;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x0200039F RID: 927
	public abstract class DynamicPawnRenderNodeSetup
	{
		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001C16 RID: 7190 RVA: 0x000AE287 File Offset: 0x000AC487
		public virtual List<Type> SetupAfter { get; }

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001C17 RID: 7191
		public abstract bool HumanlikeOnly { get; }

		// Token: 0x06001C18 RID: 7192
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		public abstract IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> GetDynamicNodes(Pawn pawn, PawnRenderTree tree);
	}
}

using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020003B7 RID: 951
	public class PawnRenderNodeProperties_Eye : PawnRenderNodeProperties
	{
		// Token: 0x06001C88 RID: 7304 RVA: 0x000AFCDC File Offset: 0x000ADEDC
		public PawnRenderNodeProperties_Eye()
		{
			this.visibleFacing = new List<Rot4>
			{
				Rot4.East,
				Rot4.South,
				Rot4.West
			};
			this.workerClass = typeof(PawnRenderNodeWorker_Eye);
			this.nodeClass = typeof(PawnRenderNode_AttachmentHead);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x000AFD3B File Offset: 0x000ADF3B
		public override void ResolveReferences()
		{
			this.skipFlag = RenderSkipFlagDefOf.Eyes;
		}
	}
}

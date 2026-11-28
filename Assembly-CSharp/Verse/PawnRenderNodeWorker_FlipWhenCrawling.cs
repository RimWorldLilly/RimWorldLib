using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D5 RID: 981
	public class PawnRenderNodeWorker_FlipWhenCrawling : PawnRenderNodeWorker
	{
		// Token: 0x06001CD8 RID: 7384 RVA: 0x000B0CBB File Offset: 0x000AEEBB
		protected override Material GetMaterial(PawnRenderNode node, PawnDrawParms parms)
		{
			if (parms.flipHead)
			{
				parms.facing = parms.facing.Opposite;
			}
			return base.GetMaterial(node, parms);
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x000B0CE0 File Offset: 0x000AEEE0
		public override float LayerFor(PawnRenderNode node, PawnDrawParms parms)
		{
			if (parms.flipHead)
			{
				parms.facing = parms.facing.Opposite;
			}
			return base.LayerFor(node, parms);
		}
	}
}

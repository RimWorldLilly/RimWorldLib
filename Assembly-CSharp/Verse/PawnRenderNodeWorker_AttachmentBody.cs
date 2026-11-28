using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C1 RID: 961
	public class PawnRenderNodeWorker_AttachmentBody : PawnRenderNodeWorker_Body
	{
		// Token: 0x06001C9A RID: 7322 RVA: 0x000B0134 File Offset: 0x000AE334
		public override Vector3 ScaleFor(PawnRenderNode node, PawnDrawParms parms)
		{
			Vector3 a = base.ScaleFor(node, parms);
			Vector2 bodyGraphicScale = parms.pawn.story.bodyType.bodyGraphicScale;
			return a * ((bodyGraphicScale.x + bodyGraphicScale.y) / 2f);
		}
	}
}

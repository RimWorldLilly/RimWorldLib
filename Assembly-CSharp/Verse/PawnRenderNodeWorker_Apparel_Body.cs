using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C0 RID: 960
	public class PawnRenderNodeWorker_Apparel_Body : PawnRenderNodeWorker_Body
	{
		// Token: 0x06001C95 RID: 7317 RVA: 0x000AFF8A File Offset: 0x000AE18A
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.flags.FlagSet(PawnRenderFlags.Clothes);
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000AFFAC File Offset: 0x000AE1AC
		public override Vector3 OffsetFor(PawnRenderNode n, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 result = base.OffsetFor(n, parms, out pivot);
			PawnRenderNode_Apparel pawnRenderNode_Apparel = (PawnRenderNode_Apparel)n;
			if (pawnRenderNode_Apparel.apparel.def.apparel.wornGraphicData != null && pawnRenderNode_Apparel.apparel.RenderAsPack())
			{
				Vector2 vector = pawnRenderNode_Apparel.apparel.def.apparel.wornGraphicData.BeltOffsetAt(parms.facing, parms.pawn.story.bodyType);
				result.x += vector.x;
				result.z += vector.y;
			}
			return result;
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x000B0044 File Offset: 0x000AE244
		public override Vector3 ScaleFor(PawnRenderNode n, PawnDrawParms parms)
		{
			Vector3 result = base.ScaleFor(n, parms);
			PawnRenderNode_Apparel pawnRenderNode_Apparel = (PawnRenderNode_Apparel)n;
			if (pawnRenderNode_Apparel.apparel.def.apparel.wornGraphicData != null && pawnRenderNode_Apparel.apparel.RenderAsPack())
			{
				Vector2 vector = pawnRenderNode_Apparel.apparel.def.apparel.wornGraphicData.BeltScaleAt(parms.facing, parms.pawn.story.bodyType);
				result.x *= vector.x;
				result.z *= vector.y;
			}
			return result;
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x000B00DC File Offset: 0x000AE2DC
		public override float LayerFor(PawnRenderNode n, PawnDrawParms parms)
		{
			if (parms.flipHead && n.Props.oppositeFacingLayerWhenFlipped)
			{
				PawnDrawParms parms2 = parms;
				parms2.facing = parms.facing.Opposite;
				parms2.flipHead = false;
				return base.LayerFor(n, parms2);
			}
			return base.LayerFor(n, parms);
		}
	}
}

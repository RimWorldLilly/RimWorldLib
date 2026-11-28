using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D3 RID: 979
	public class PawnRenderNodeWorker_Eye : PawnRenderNodeWorker_FlipWhenCrawling
	{
		// Token: 0x06001CD2 RID: 7378 RVA: 0x000B0B1C File Offset: 0x000AED1C
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 vector = base.OffsetFor(node, parms, out pivot);
			BodyTypeDef.WoundAnchor anchor;
			if (this.TryGetWoundAnchor(node.Props.anchorTag, parms, out anchor))
			{
				Vector3 b;
				float num;
				PawnDrawUtility.CalcAnchorData(parms.pawn, anchor, parms.facing, out b, out num);
				vector += b;
			}
			return vector;
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x000B0B68 File Offset: 0x000AED68
		public override Vector3 ScaleFor(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.ScaleFor(node, parms) * parms.pawn.ageTracker.CurLifeStage.eyeSizeFactor.GetValueOrDefault(1f);
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x000B0B98 File Offset: 0x000AED98
		protected bool TryGetWoundAnchor(string anchorTag, PawnDrawParms parms, out BodyTypeDef.WoundAnchor anchor)
		{
			anchor = null;
			if (anchorTag.NullOrEmpty())
			{
				return false;
			}
			List<BodyTypeDef.WoundAnchor> woundAnchors = parms.pawn.story.bodyType.woundAnchors;
			for (int i = 0; i < woundAnchors.Count; i++)
			{
				BodyTypeDef.WoundAnchor woundAnchor = woundAnchors[i];
				if (woundAnchor.tag == anchorTag && woundAnchor.rotation == parms.facing && (parms.facing == Rot4.South || woundAnchor.narrowCrown.GetValueOrDefault() == parms.pawn.story.headType.narrow))
				{
					anchor = woundAnchor;
					return true;
				}
			}
			return false;
		}
	}
}

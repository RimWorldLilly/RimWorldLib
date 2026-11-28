using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D4 RID: 980
	public class PawnRenderNodeWorker_HediffEye : PawnRenderNodeWorker_Eye
	{
		// Token: 0x06001CD6 RID: 7382 RVA: 0x000B0C60 File Offset: 0x000AEE60
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 vector = base.OffsetFor(node, parms, out pivot);
			BodyPartRecord bodyPart = node.bodyPart;
			BodyTypeDef.WoundAnchor anchor;
			if (base.TryGetWoundAnchor((bodyPart != null) ? bodyPart.woundAnchorTag : null, parms, out anchor))
			{
				Vector3 b;
				float num;
				PawnDrawUtility.CalcAnchorData(parms.pawn, anchor, parms.facing, out b, out num);
				vector += b;
			}
			return vector;
		}
	}
}

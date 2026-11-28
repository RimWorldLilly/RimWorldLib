using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D8 RID: 984
	public class PawnRenderNodeWorker_Spastic : PawnRenderNodeWorker
	{
		// Token: 0x06001CE1 RID: 7393 RVA: 0x000B0F2C File Offset: 0x000AF12C
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 vector = base.OffsetFor(node, parms, out pivot);
			PawnRenderNode_Spastic pawnRenderNode_Spastic = node as PawnRenderNode_Spastic;
			PawnRenderNode_Spastic.SpasmData spasmData;
			float t;
			if (pawnRenderNode_Spastic != null && pawnRenderNode_Spastic.CheckAndDoSpasm(parms, out spasmData, out t))
			{
				vector += Vector3.Lerp(spasmData.offsetStart, spasmData.offsetTarget, t);
			}
			return vector;
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x000B0F74 File Offset: 0x000AF174
		public override Quaternion RotationFor(PawnRenderNode node, PawnDrawParms parms)
		{
			Quaternion quaternion = base.RotationFor(node, parms);
			PawnRenderNode_Spastic pawnRenderNode_Spastic = node as PawnRenderNode_Spastic;
			if (pawnRenderNode_Spastic == null)
			{
				return quaternion;
			}
			float num = 0f;
			PawnRenderNodeProperties_Spastic pawnRenderNodeProperties_Spastic = node.Props as PawnRenderNodeProperties_Spastic;
			if (pawnRenderNodeProperties_Spastic != null && pawnRenderNodeProperties_Spastic.rotateFacing)
			{
				num += parms.facing.AsAngle;
			}
			PawnRenderNode_Spastic.SpasmData spasmData;
			float t;
			if (pawnRenderNode_Spastic.CheckAndDoSpasm(parms, out spasmData, out t))
			{
				num += Mathf.Lerp(spasmData.rotationStart, spasmData.rotationTarget, t);
			}
			return quaternion * num.ToQuat();
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x000B0FF4 File Offset: 0x000AF1F4
		public override Vector3 ScaleFor(PawnRenderNode node, PawnDrawParms parms)
		{
			Vector3 vector = base.ScaleFor(node, parms);
			PawnRenderNode_Spastic pawnRenderNode_Spastic = node as PawnRenderNode_Spastic;
			PawnRenderNode_Spastic.SpasmData spasmData;
			float t;
			if (pawnRenderNode_Spastic != null && pawnRenderNode_Spastic.CheckAndDoSpasm(parms, out spasmData, out t))
			{
				vector *= Mathf.Lerp(spasmData.scaleStart, spasmData.scaleTarget, t);
				vector.y = 1f;
			}
			return vector;
		}
	}
}

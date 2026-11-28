using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DB RID: 987
	public class PawnRenderSubWorker
	{
		// Token: 0x06001CFA RID: 7418 RVA: 0x000B19AC File Offset: 0x000AFBAC
		public virtual bool CanDrawNowSub(PawnRenderNode node, PawnDrawParms parms)
		{
			return true;
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x000B19AF File Offset: 0x000AFBAF
		public virtual void EditMaterialPropertyBlock(PawnRenderNode node, Material material, PawnDrawParms parms, ref MaterialPropertyBlock block)
		{
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x000B19B1 File Offset: 0x000AFBB1
		public virtual void EditMaterial(PawnRenderNode node, PawnDrawParms parms, ref Material material)
		{
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x000B19B3 File Offset: 0x000AFBB3
		public virtual void TransformOffset(PawnRenderNode node, PawnDrawParms parms, ref Vector3 offset, ref Vector3 pivot)
		{
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x000B19B5 File Offset: 0x000AFBB5
		public virtual void TransformLayer(PawnRenderNode node, PawnDrawParms parms, ref float layer)
		{
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x000B19B7 File Offset: 0x000AFBB7
		public virtual void TransformRotation(PawnRenderNode node, PawnDrawParms parms, ref Quaternion rotation)
		{
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x000B19B9 File Offset: 0x000AFBB9
		public virtual void TransformScale(PawnRenderNode node, PawnDrawParms parms, ref Vector3 scale)
		{
		}
	}
}

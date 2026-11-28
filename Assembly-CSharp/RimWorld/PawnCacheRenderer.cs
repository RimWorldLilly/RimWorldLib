using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F3 RID: 5363
	public class PawnCacheRenderer : MonoBehaviour
	{
		// Token: 0x06008386 RID: 33670 RVA: 0x002F3B10 File Offset: 0x002F1D10
		public void RenderPawn(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom, float angle, Rot4 rotation, bool renderHead = true, bool renderHeadgear = true, bool renderClothes = true, bool portrait = false, Vector3 positionOffset = default(Vector3), IReadOnlyDictionary<Apparel, Color> overrideApparelColor = null, Color? overrideHairColor = null, bool stylingStation = false)
		{
			this.rotation = rotation;
			this.renderHead = renderHead;
			this.renderHeadgear = renderHeadgear;
			this.renderClothes = renderClothes;
			this.portrait = portrait;
			this.angle = angle;
			this.positionOffset = positionOffset;
			this.overrideApparelColor = overrideApparelColor;
			this.overrideHairColor = overrideHairColor;
			this.stylingStation = stylingStation;
			Camera pawnCacheCamera = Find.PawnCacheCamera;
			Vector3 position = pawnCacheCamera.transform.position;
			float orthographicSize = pawnCacheCamera.orthographicSize;
			pawnCacheCamera.transform.position += cameraOffset;
			pawnCacheCamera.orthographicSize = 1f / cameraZoom;
			this.pawn = pawn;
			pawnCacheCamera.SetTargetBuffers(renderTexture.colorBuffer, renderTexture.depthBuffer);
			pawnCacheCamera.Render();
			this.pawn = null;
			pawnCacheCamera.transform.position = position;
			pawnCacheCamera.orthographicSize = orthographicSize;
			pawnCacheCamera.targetTexture = null;
		}

		// Token: 0x06008387 RID: 33671 RVA: 0x002F3BEC File Offset: 0x002F1DEC
		public void OnPostRender()
		{
			this.pawn.Drawer.renderer.RenderCache(this.rotation, this.angle, this.positionOffset, this.renderHead, this.portrait, this.renderHeadgear, this.renderClothes, this.overrideApparelColor, this.overrideHairColor, this.stylingStation);
		}

		// Token: 0x0400513B RID: 20795
		private Pawn pawn;

		// Token: 0x0400513C RID: 20796
		private Rot4 rotation;

		// Token: 0x0400513D RID: 20797
		private bool renderHead;

		// Token: 0x0400513E RID: 20798
		private bool renderHeadgear;

		// Token: 0x0400513F RID: 20799
		private bool renderClothes;

		// Token: 0x04005140 RID: 20800
		private bool portrait;

		// Token: 0x04005141 RID: 20801
		private float angle;

		// Token: 0x04005142 RID: 20802
		private Vector3 positionOffset;

		// Token: 0x04005143 RID: 20803
		private IReadOnlyDictionary<Apparel, Color> overrideApparelColor;

		// Token: 0x04005144 RID: 20804
		private Color? overrideHairColor;

		// Token: 0x04005145 RID: 20805
		private bool stylingStation;
	}
}

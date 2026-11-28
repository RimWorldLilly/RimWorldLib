using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003CB RID: 971
	public abstract class PawnRenderNodeWorker_Overlay : PawnRenderNodeWorker
	{
		// Token: 0x06001CB4 RID: 7348
		protected abstract PawnOverlayDrawer OverlayDrawer(Pawn pawn);

		// Token: 0x06001CB5 RID: 7349 RVA: 0x000B0793 File Offset: 0x000AE993
		public override bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
		{
			return false;
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x000B0798 File Offset: 0x000AE998
		public override void AppendDrawRequests(PawnRenderNode node, PawnDrawParms parms, List<PawnGraphicDrawRequest> requests)
		{
			Mesh mesh = node.GetMesh(parms);
			if (mesh != null)
			{
				requests.Add(new PawnGraphicDrawRequest(node, mesh, null));
			}
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x000B07C4 File Offset: 0x000AE9C4
		public override void PostDraw(PawnRenderNode node, PawnDrawParms parms, Mesh mesh, Matrix4x4 matrix)
		{
			PawnOverlayDrawer pawnOverlayDrawer = this.OverlayDrawer(parms.pawn);
			if (pawnOverlayDrawer == null)
			{
				return;
			}
			pawnOverlayDrawer.RenderPawnOverlay(matrix, mesh, node.Props.overlayLayer, parms, node.Props.overlayOverApparel);
		}
	}
}

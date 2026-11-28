using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D0 RID: 976
	public class PawnRenderNodeWorker_OverlayStatus : PawnRenderNodeWorker_Overlay
	{
		// Token: 0x06001CC7 RID: 7367 RVA: 0x000B092E File Offset: 0x000AEB2E
		protected override PawnOverlayDrawer OverlayDrawer(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000B0931 File Offset: 0x000AEB31
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && !parms.Portrait && !parms.Cache;
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x000B0954 File Offset: 0x000AEB54
		public override void PostDraw(PawnRenderNode node, PawnDrawParms parms, Mesh mesh, Matrix4x4 matrix)
		{
			Vector3 vector2;
			Vector3 vector = this.OffsetFor(node, parms, out vector2);
			Quaternion quat = this.RotationFor(node, parms);
			if (node.Props.overlayLayer == PawnOverlayDrawer.OverlayLayer.Head)
			{
				vector += parms.pawn.Drawer.renderer.BaseHeadOffsetAt(Rot4.North);
			}
			parms.pawn.Drawer.renderer.StatusOverlays.RenderStatusOverlays(vector, quat, mesh);
		}
	}
}

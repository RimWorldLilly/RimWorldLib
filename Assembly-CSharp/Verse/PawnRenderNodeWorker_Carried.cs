using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D2 RID: 978
	public class PawnRenderNodeWorker_Carried : PawnRenderNodeWorker
	{
		// Token: 0x06001CCD RID: 7373 RVA: 0x000B0A2D File Offset: 0x000AEC2D
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && !parms.Portrait && !parms.pawn.Dead && parms.pawn.Spawned;
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x000B0A64 File Offset: 0x000AEC64
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 result = base.OffsetFor(node, parms, out pivot);
			result.y = base.AltitudeFor(node, parms);
			return result;
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x000B0A8B File Offset: 0x000AEC8B
		public override void AppendDrawRequests(PawnRenderNode node, PawnDrawParms parms, List<PawnGraphicDrawRequest> requests)
		{
			requests.Add(new PawnGraphicDrawRequest(node, null, null));
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x000B0A9C File Offset: 0x000AEC9C
		public override void PostDraw(PawnRenderNode node, PawnDrawParms parms, Mesh mesh, Matrix4x4 matrix)
		{
			Vector3 vector2;
			Vector3 vector = parms.matrix.Position() + this.OffsetFor(node, parms, out vector2);
			Pawn_CarryTracker carryTracker = parms.pawn.carryTracker;
			if (((carryTracker != null) ? carryTracker.CarriedThing : null) != null)
			{
				PawnRenderUtility.DrawCarriedThing(parms.pawn, vector, parms.pawn.carryTracker.CarriedThing);
				return;
			}
			PawnRenderUtility.DrawEquipmentAndApparelExtras(parms.pawn, vector, parms.facing, parms.flags);
		}
	}
}

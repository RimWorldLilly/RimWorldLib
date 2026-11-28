using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003BF RID: 959
	public class PawnRenderNodeWorker_AnimalBody : PawnRenderNodeWorker
	{
		// Token: 0x06001C92 RID: 7314 RVA: 0x000AFF0C File Offset: 0x000AE10C
		protected override GraphicStateDef GetGraphicState(PawnRenderNode node, PawnDrawParms parms)
		{
			if (node.tree.currentAnimation == null && parms.pawn.DrawNonHumanlikeSwimmingGraphic)
			{
				return GraphicStateDefOf.Swimming;
			}
			if (node.tree.currentAnimation == null && parms.pawn.DrawNonHumanlikeStationaryGraphic)
			{
				return GraphicStateDefOf.Stationary;
			}
			return base.GetGraphicState(node, parms);
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x000AFF61 File Offset: 0x000AE161
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			return base.OffsetFor(node, parms, out pivot) + node.PrimaryGraphic.DrawOffset(parms.facing);
		}
	}
}

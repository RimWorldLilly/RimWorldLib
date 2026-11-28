using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C8 RID: 968
	public class PawnRenderNodeWorker_Head : PawnRenderNodeWorker_FlipWhenCrawling
	{
		// Token: 0x06001CAB RID: 7339 RVA: 0x000B04FF File Offset: 0x000AE6FF
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && !parms.flags.FlagSet(PawnRenderFlags.HeadStump);
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x000B051C File Offset: 0x000AE71C
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 result = base.OffsetFor(node, parms, out pivot) + parms.pawn.Drawer.renderer.BaseHeadOffsetAt(parms.facing);
			if (parms.pawn.story.headType.narrow && node.Props.narrowCrownHorizontalOffset != 0f && parms.facing.IsHorizontal)
			{
				if (parms.facing == Rot4.East)
				{
					result.x -= node.Props.narrowCrownHorizontalOffset;
				}
				else if (parms.facing == Rot4.West)
				{
					result.x += node.Props.narrowCrownHorizontalOffset;
				}
				result.z -= node.Props.narrowCrownHorizontalOffset;
			}
			if (!parms.Portrait && parms.swimming)
			{
				result.z -= 0.5f;
			}
			return result;
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x000B0618 File Offset: 0x000AE818
		public override Quaternion RotationFor(PawnRenderNode node, PawnDrawParms parms)
		{
			Quaternion quaternion = base.RotationFor(node, parms);
			if (!parms.Portrait && parms.pawn.Crawling)
			{
				quaternion *= PawnRenderUtility.CrawlingHeadAngle(parms.facing).ToQuat();
				if (parms.flipHead)
				{
					quaternion *= 180f.ToQuat();
				}
			}
			if (parms.pawn.IsShambler && parms.pawn.mutant != null && parms.pawn.mutant.HasTurned && !parms.pawn.Dead)
			{
				Quaternion lhs = quaternion;
				Vector3 up = Vector3.up;
				Hediff_Shambler hediff_Shambler = parms.pawn.mutant.Hediff as Hediff_Shambler;
				quaternion = lhs * Quaternion.Euler(up * ((hediff_Shambler != null) ? hediff_Shambler.headRotation : 0f));
			}
			return quaternion;
		}
	}
}

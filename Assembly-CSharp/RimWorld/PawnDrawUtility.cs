using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001495 RID: 5269
	public static class PawnDrawUtility
	{
		// Token: 0x06008127 RID: 33063 RVA: 0x002E1938 File Offset: 0x002DFB38
		public static bool AnchorUsable(Pawn pawn, BodyTypeDef.WoundAnchor anchor, Rot4 pawnRot)
		{
			return (anchor.rotation == null || anchor.rotation.Value == pawnRot || (anchor.canMirror && anchor.rotation.Value == pawnRot.Opposite)) && (anchor.narrowCrown == null || (pawn.story != null && pawn.story.headType.narrow == anchor.narrowCrown.Value)) && (pawn.health.hediffSet.HasHead || anchor.layer != PawnOverlayDrawer.OverlayLayer.Head);
		}

		// Token: 0x06008128 RID: 33064 RVA: 0x002E19DA File Offset: 0x002DFBDA
		public static IEnumerable<BodyTypeDef.WoundAnchor> FindAnchors(Pawn pawn, BodyPartRecord curPart)
		{
			PawnDrawUtility.<FindAnchors>d__3 <FindAnchors>d__ = new PawnDrawUtility.<FindAnchors>d__3(-2);
			<FindAnchors>d__.<>3__pawn = pawn;
			<FindAnchors>d__.<>3__curPart = curPart;
			return <FindAnchors>d__;
		}

		// Token: 0x06008129 RID: 33065 RVA: 0x002E19F4 File Offset: 0x002DFBF4
		public static void CalcAnchorData(Pawn pawn, BodyTypeDef.WoundAnchor anchor, Rot4 pawnRot, out Vector3 anchorOffset, out float range)
		{
			anchorOffset = anchor.offset;
			if (anchor.rotation == pawnRot.Opposite)
			{
				anchorOffset.x *= -1f;
			}
			if ((anchor.tag == "LeftEye" || anchor.tag == "RightEye") && pawnRot.IsHorizontal && (!ModsConfig.BiotechActive || !pawn.DevelopmentalStage.Juvenile()))
			{
				Vector3? eyeOffsetEastWest = pawn.story.headType.eyeOffsetEastWest;
				if (eyeOffsetEastWest != null)
				{
					if (pawnRot == Rot4.East)
					{
						anchorOffset = eyeOffsetEastWest.Value;
					}
					else
					{
						anchorOffset = eyeOffsetEastWest.Value.ScaledBy(new Vector3(-1f, 1f, 1f));
					}
				}
			}
			range = anchor.range;
		}

		// Token: 0x04005055 RID: 20565
		public const string AnchorTagEyeLeft = "LeftEye";

		// Token: 0x04005056 RID: 20566
		public const string AnchorTagEyeRight = "RightEye";
	}
}

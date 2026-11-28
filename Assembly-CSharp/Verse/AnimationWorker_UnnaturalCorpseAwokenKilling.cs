using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000386 RID: 902
	public class AnimationWorker_UnnaturalCorpseAwokenKilling : AnimationWorker_Keyframes
	{
		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x000A9493 File Offset: 0x000A7693
		private Material LensFlareMat
		{
			get
			{
				if (this.lensFlareMat == null)
				{
					this.lensFlareMat = MaterialPool.MatFrom("Things/Pawn/Revenant/Revenant_LensFlare", ShaderDatabase.MoteGlow);
				}
				return this.lensFlareMat;
			}
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x000A94C0 File Offset: 0x000A76C0
		public override void PostDraw(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms, Matrix4x4 matrix)
		{
			foreach (Vector3 a in ((parms.facing == Rot4.North || parms.facing == Rot4.South) ? AnimationWorker_UnnaturalCorpseAwokenKilling.EyePositionsPxNS : AnimationWorker_UnnaturalCorpseAwokenKilling.EyePositionsPxEW))
			{
				a.y = 1f;
				if (parms.facing == Rot4.North || parms.facing == Rot4.West)
				{
					a.x *= -1f;
				}
				if (parms.facing == Rot4.North)
				{
					a.y = -1f;
				}
				float x = (float)(Find.TickManager.TicksGame - node.tree.animationStartTick) % 30f / 30f;
				this.LensFlareMat.SetColor(ShaderPropertyIDs.Color, new Color(1f, 1f, 1f, AnimationWorker_UnnaturalCorpseAwokenKilling.LensFlareOpacity.Evaluate(x)));
				GenDraw.DrawMeshNowOrLater(MeshPool.GridPlane(Vector2.one * 0.7f), matrix * Matrix4x4.Translate(a / 100f), this.LensFlareMat, parms.DrawNow, null);
			}
			Pawn pawn = node.tree.pawn.mindState.enemyTarget as Pawn;
			if (pawn != null)
			{
				Rot4 rotation = (pawn.GetPosture() == PawnPosture.Standing) ? Rot4.North : pawn.Drawer.renderer.LayingFacing();
				Vector3 b = pawn.Drawer.renderer.BaseHeadOffsetAt(rotation).RotatedBy(pawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None));
				Vector3 loc = (pawn.DrawPos + b).WithYOffset(PawnRenderUtility.AltitudeForLayer(80f));
				Quaternion quat = Quaternion.Euler(Vector3.up * pawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None));
				GenDraw.DrawMeshNowOrLater(MeshPool.GridPlane(Vector2.one * 2f), loc, quat, this.LensFlareMat, parms.DrawNow);
			}
		}

		// Token: 0x04001688 RID: 5768
		private const float LensFlareScale = 0.7f;

		// Token: 0x04001689 RID: 5769
		private const float LensFlareAnimPeriod = 30f;

		// Token: 0x0400168A RID: 5770
		private const float Layer_AboveHead = 80f;

		// Token: 0x0400168B RID: 5771
		private const float PxToUnitScale = 100f;

		// Token: 0x0400168C RID: 5772
		private static readonly List<Vector3> EyePositionsPxNS = new List<Vector3>
		{
			new Vector3(-8f, 0f, -3f),
			new Vector3(12f, 0f, -5f)
		};

		// Token: 0x0400168D RID: 5773
		private static readonly List<Vector3> EyePositionsPxEW = new List<Vector3>
		{
			new Vector3(19f, 0f, 9f),
			new Vector3(18f, 0f, -7f)
		};

		// Token: 0x0400168E RID: 5774
		private static readonly SimpleCurve LensFlareOpacity = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.4f),
				true
			},
			{
				new CurvePoint(0.5f, 0.8f),
				true
			},
			{
				new CurvePoint(1f, 0.4f),
				true
			}
		};

		// Token: 0x0400168F RID: 5775
		private Material lensFlareMat;
	}
}

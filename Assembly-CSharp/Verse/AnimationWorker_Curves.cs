using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000382 RID: 898
	public class AnimationWorker_Curves : BaseAnimationWorker
	{
		// Token: 0x06001B53 RID: 6995 RVA: 0x000A89B1 File Offset: 0x000A6BB1
		public override bool Enabled(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return def.playWhenDowned || !parms.pawn.Downed;
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x000A89CC File Offset: 0x000A6BCC
		public override void PostDraw(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms, Matrix4x4 matrix)
		{
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x000A89D0 File Offset: 0x000A6BD0
		public override Vector3 OffsetAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			CurveKeyAnimationPart curveKeyAnimationPart = (CurveKeyAnimationPart)part;
			Pawn pawn = parms.pawn;
			float time = (float)tick;
			if (curveKeyAnimationPart.normalized)
			{
				time = Mathf.InverseLerp(0f, (float)def.durationTicks, (float)tick);
			}
			Vector3Curve offset = curveKeyAnimationPart.offset;
			Vector3 offset2 = (offset != null) ? offset.Evaluate(time, 0f) : Vector3.zero;
			return AnimationUtility.AdjustOffsetForRotationMode(part.rotationMode, offset2, pawn);
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x000A8A38 File Offset: 0x000A6C38
		public override float AngleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			CurveKeyAnimationPart curveKeyAnimationPart = (CurveKeyAnimationPart)part;
			float time = (float)tick;
			if (curveKeyAnimationPart.normalized)
			{
				time = Mathf.InverseLerp(0f, (float)def.durationTicks, (float)tick);
			}
			ComplexCurve angle = curveKeyAnimationPart.angle;
			if (angle == null)
			{
				return 0f;
			}
			return angle.Evaluate(time);
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x000A8A80 File Offset: 0x000A6C80
		public override Vector3 ScaleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			CurveKeyAnimationPart curveKeyAnimationPart = (CurveKeyAnimationPart)part;
			float time = (float)tick;
			if (curveKeyAnimationPart.normalized)
			{
				time = Mathf.InverseLerp(0f, (float)def.durationTicks, (float)tick);
			}
			Vector3Curve scale = curveKeyAnimationPart.scale;
			if (scale == null)
			{
				return Vector3.one;
			}
			return scale.Evaluate(time, 1f);
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x000A8ACD File Offset: 0x000A6CCD
		public override GraphicStateDef GraphicStateAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return AnimationWorker_Curves.GetKeyframeForTick(tick, part).graphicState;
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x000A8ADC File Offset: 0x000A6CDC
		protected static CurveKey GetKeyframeForTick(int tick, AnimationPart p)
		{
			CurveKeyAnimationPart curveKeyAnimationPart = (CurveKeyAnimationPart)p;
			if (tick <= curveKeyAnimationPart.keyframes[0].tick)
			{
				return curveKeyAnimationPart.keyframes[0];
			}
			List<CurveKey> keyframes = curveKeyAnimationPart.keyframes;
			if (tick >= keyframes[keyframes.Count - 1].tick)
			{
				List<CurveKey> keyframes2 = curveKeyAnimationPart.keyframes;
				return keyframes2[keyframes2.Count - 1];
			}
			for (int i = 0; i < curveKeyAnimationPart.keyframes.Count; i++)
			{
				if (tick <= curveKeyAnimationPart.keyframes[i].tick)
				{
					return curveKeyAnimationPart.keyframes[i];
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Attempted to get a curve keyframe for a tick (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(tick);
			defaultInterpolatedStringHandler.AppendLiteral(") which wasn't handled");
			Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			return null;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000383 RID: 899
	public class AnimationWorker_Keyframes : BaseAnimationWorker
	{
		// Token: 0x06001B5B RID: 7003 RVA: 0x000A8BB6 File Offset: 0x000A6DB6
		public override bool Enabled(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return def.playWhenDowned || !parms.pawn.Downed;
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x000A8BD1 File Offset: 0x000A6DD1
		public override void PostDraw(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms, Matrix4x4 matrix)
		{
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x000A8BD4 File Offset: 0x000A6DD4
		public override Vector3 OffsetAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			Pawn pawn = parms.pawn;
			Vector3 keyframeData = AnimationWorker_Keyframes.GetKeyframeData<Vector3>(tick, part, this.cachedGetKeyframeOffset, AnimationWorker_Keyframes.cachedLerpVector3);
			return AnimationUtility.AdjustOffsetForRotationMode(part.rotationMode, keyframeData, pawn);
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x000A8C0B File Offset: 0x000A6E0B
		public override float AngleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return AnimationWorker_Keyframes.GetKeyframeData<float>(tick, part, this.cachedGetKeyframeAngle, AnimationWorker_Keyframes.cachedLerpMathf);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x000A8C20 File Offset: 0x000A6E20
		public override Vector3 ScaleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return AnimationWorker_Keyframes.GetKeyframeData<Vector3>(tick, part, this.cachedGetKeyframeScale, AnimationWorker_Keyframes.cachedLerpVector3);
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x000A8C35 File Offset: 0x000A6E35
		public override GraphicStateDef GraphicStateAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return AnimationWorker_Keyframes.GetKeyframeForTick(tick, part).graphicState;
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x000A8C44 File Offset: 0x000A6E44
		protected static T GetKeyframeData<T>(int tick, AnimationPart p, Func<Keyframe, T> func, Func<T, T, float, T> lerpFunc) where T : struct
		{
			KeyframeAnimationPart keyframeAnimationPart = (KeyframeAnimationPart)p;
			if (tick <= keyframeAnimationPart.keyframes[0].tick)
			{
				return func(keyframeAnimationPart.keyframes[0]);
			}
			List<Keyframe> keyframes = keyframeAnimationPart.keyframes;
			if (tick >= keyframes[keyframes.Count - 1].tick)
			{
				List<Keyframe> keyframes2 = keyframeAnimationPart.keyframes;
				return func(keyframes2[keyframes2.Count - 1]);
			}
			Keyframe keyframe = keyframeAnimationPart.keyframes[0];
			List<Keyframe> keyframes3 = keyframeAnimationPart.keyframes;
			Keyframe keyframe2 = keyframes3[keyframes3.Count - 1];
			int i = 0;
			while (i < keyframeAnimationPart.keyframes.Count)
			{
				if (tick <= keyframeAnimationPart.keyframes[i].tick)
				{
					keyframe2 = keyframeAnimationPart.keyframes[i];
					if (i > 0)
					{
						keyframe = keyframeAnimationPart.keyframes[i - 1];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			float arg = (float)(tick - keyframe.tick) / (float)(keyframe2.tick - keyframe.tick);
			return lerpFunc(func(keyframe), func(keyframe2), arg);
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x000A8D58 File Offset: 0x000A6F58
		protected static Keyframe GetKeyframeForTick(int tick, AnimationPart p)
		{
			KeyframeAnimationPart keyframeAnimationPart = (KeyframeAnimationPart)p;
			if (tick <= keyframeAnimationPart.keyframes[0].tick)
			{
				return keyframeAnimationPart.keyframes[0];
			}
			List<Keyframe> keyframes = keyframeAnimationPart.keyframes;
			if (tick >= keyframes[keyframes.Count - 1].tick)
			{
				List<Keyframe> keyframes2 = keyframeAnimationPart.keyframes;
				return keyframes2[keyframes2.Count - 1];
			}
			for (int i = 0; i < keyframeAnimationPart.keyframes.Count; i++)
			{
				if (tick <= keyframeAnimationPart.keyframes[i].tick)
				{
					return keyframeAnimationPart.keyframes[i];
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Attempted to get a keyframe for a tick (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(tick);
			defaultInterpolatedStringHandler.AppendLiteral(") which wasn't handled");
			Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			return null;
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x000A8E2A File Offset: 0x000A702A
		private static Vector3 GetKeyframeScale(Keyframe keyframe)
		{
			return keyframe.scale;
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x000A8E32 File Offset: 0x000A7032
		private static Vector3 GetKeyframeOffset(Keyframe keyframe)
		{
			return keyframe.offset;
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x000A8E3A File Offset: 0x000A703A
		private static float GetKeyframeAngle(Keyframe keyframe)
		{
			return keyframe.angle;
		}

		// Token: 0x04001677 RID: 5751
		private static readonly Func<Vector3, Vector3, float, Vector3> cachedLerpVector3 = new Func<Vector3, Vector3, float, Vector3>(Vector3.Lerp);

		// Token: 0x04001678 RID: 5752
		private static readonly Func<float, float, float, float> cachedLerpMathf = new Func<float, float, float, float>(Mathf.Lerp);

		// Token: 0x04001679 RID: 5753
		private readonly Func<Keyframe, Vector3> cachedGetKeyframeOffset = new Func<Keyframe, Vector3>(AnimationWorker_Keyframes.GetKeyframeOffset);

		// Token: 0x0400167A RID: 5754
		private readonly Func<Keyframe, float> cachedGetKeyframeAngle = new Func<Keyframe, float>(AnimationWorker_Keyframes.GetKeyframeAngle);

		// Token: 0x0400167B RID: 5755
		private readonly Func<Keyframe, Vector3> cachedGetKeyframeScale = new Func<Keyframe, Vector3>(AnimationWorker_Keyframes.GetKeyframeScale);
	}
}

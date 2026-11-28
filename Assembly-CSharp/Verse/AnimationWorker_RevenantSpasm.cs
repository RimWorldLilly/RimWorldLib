using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000385 RID: 901
	public class AnimationWorker_RevenantSpasm : BaseAnimationWorker
	{
		// Token: 0x06001B6C RID: 7020 RVA: 0x000A921B File Offset: 0x000A741B
		private static float AnimationProgress(AnimationWorker_RevenantSpasm.RevenantSpasmData data)
		{
			return ((float)Find.TickManager.TicksGame - data.spasmStart) / data.spasmLength;
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x000A9236 File Offset: 0x000A7436
		private static float Rotation(AnimationWorker_RevenantSpasm.RevenantSpasmData data)
		{
			return Mathf.Lerp(data.startHeadRot, data.targetHeadRot, AnimationWorker_RevenantSpasm.AnimationProgress(data));
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x000A9250 File Offset: 0x000A7450
		private static Vector3 Offset(AnimationWorker_RevenantSpasm.RevenantSpasmData data)
		{
			return new Vector3(Mathf.Lerp(data.startHeadOffset.x, data.targetHeadOffset.x, AnimationWorker_RevenantSpasm.AnimationProgress(data)), 0f, Mathf.Lerp(data.startHeadOffset.z, data.targetHeadOffset.z, AnimationWorker_RevenantSpasm.AnimationProgress(data)));
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x000A92A9 File Offset: 0x000A74A9
		public override bool Enabled(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return def.playWhenDowned || !parms.pawn.Downed;
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x000A92C4 File Offset: 0x000A74C4
		public override void PostDraw(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms, Matrix4x4 matrix)
		{
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x000A92C8 File Offset: 0x000A74C8
		public override Vector3 OffsetAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			AnimationWorker_RevenantSpasm.RevenantSpasmData animationWorkerData = node.GetAnimationWorkerData<AnimationWorker_RevenantSpasm.RevenantSpasmData>();
			AnimationWorker_RevenantSpasm.CheckAndSpasm(animationWorkerData);
			if (parms.facing == Rot4.East || parms.facing == Rot4.West)
			{
				return AnimationWorker_RevenantSpasm.Offset(animationWorkerData) / 2f;
			}
			return AnimationWorker_RevenantSpasm.Offset(animationWorkerData);
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x000A9320 File Offset: 0x000A7520
		public override float AngleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			AnimationWorker_RevenantSpasm.RevenantSpasmData animationWorkerData = node.GetAnimationWorkerData<AnimationWorker_RevenantSpasm.RevenantSpasmData>();
			AnimationWorker_RevenantSpasm.CheckAndSpasm(animationWorkerData);
			if (parms.facing == Rot4.East || parms.facing == Rot4.West)
			{
				return AnimationWorker_RevenantSpasm.Rotation(animationWorkerData) / 2f;
			}
			return AnimationWorker_RevenantSpasm.Rotation(animationWorkerData);
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x000A9373 File Offset: 0x000A7573
		public override Vector3 ScaleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return Vector3.one;
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x000A937A File Offset: 0x000A757A
		public override GraphicStateDef GraphicStateAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return null;
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x000A9380 File Offset: 0x000A7580
		private static void CheckAndSpasm(AnimationWorker_RevenantSpasm.RevenantSpasmData data)
		{
			if ((float)Find.TickManager.TicksGame > data.nextSpasm)
			{
				data.startHeadRot = AnimationWorker_RevenantSpasm.Rotation(data);
				data.startHeadOffset = AnimationWorker_RevenantSpasm.Offset(data);
				data.targetHeadRot = (float)Rand.Range(-20, 20);
				data.targetHeadOffset = new Vector3(Rand.Range(-0.1f, 0.1f), 0f, Rand.Range(-0.05f, 0.05f));
				data.spasmStart = (float)Find.TickManager.TicksGame;
				data.spasmLength = (float)(Rand.Bool ? AnimationWorker_RevenantSpasm.SpasmLength.RandomInRange : 6);
				data.nextSpasm = (float)(Find.TickManager.TicksGame + (Rand.Bool ? AnimationWorker_RevenantSpasm.SpasmIntervalShort.RandomInRange : AnimationWorker_RevenantSpasm.SpasmIntervalLong.RandomInRange));
			}
		}

		// Token: 0x04001684 RID: 5764
		private static readonly IntRange SpasmIntervalShort = new IntRange(6, 18);

		// Token: 0x04001685 RID: 5765
		private static readonly IntRange SpasmIntervalLong = new IntRange(120, 180);

		// Token: 0x04001686 RID: 5766
		private static readonly IntRange SpasmLength = new IntRange(30, 60);

		// Token: 0x04001687 RID: 5767
		private const int ShortSpasmLength = 6;

		// Token: 0x0200269F RID: 9887
		private class RevenantSpasmData
		{
			// Token: 0x04009E19 RID: 40473
			public float nextSpasm = -99999f;

			// Token: 0x04009E1A RID: 40474
			public float spasmStart = -99999f;

			// Token: 0x04009E1B RID: 40475
			public float spasmLength;

			// Token: 0x04009E1C RID: 40476
			public float startHeadRot = 90f;

			// Token: 0x04009E1D RID: 40477
			public Vector3 startHeadOffset = Vector3.zero;

			// Token: 0x04009E1E RID: 40478
			public float targetHeadRot = 90f;

			// Token: 0x04009E1F RID: 40479
			public Vector3 targetHeadOffset = Vector3.zero;
		}
	}
}

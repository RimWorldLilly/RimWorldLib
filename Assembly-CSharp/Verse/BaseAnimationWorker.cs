using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000387 RID: 903
	[StaticConstructorOnStartup]
	public abstract class BaseAnimationWorker
	{
		// Token: 0x06001B7C RID: 7036 RVA: 0x000A97F4 File Offset: 0x000A79F4
		static BaseAnimationWorker()
		{
			foreach (Type type in typeof(BaseAnimationWorker).AllSubclassesNonAbstract())
			{
				GenWorker<BaseAnimationWorker>.Get(type);
			}
		}

		// Token: 0x06001B7D RID: 7037
		public abstract bool Enabled(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms);

		// Token: 0x06001B7E RID: 7038
		public abstract void PostDraw(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms, Matrix4x4 matrix);

		// Token: 0x06001B7F RID: 7039
		public abstract Vector3 OffsetAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms);

		// Token: 0x06001B80 RID: 7040
		public abstract float AngleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms);

		// Token: 0x06001B81 RID: 7041
		public abstract Vector3 ScaleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms);

		// Token: 0x06001B82 RID: 7042
		public abstract GraphicStateDef GraphicStateAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms);
	}
}

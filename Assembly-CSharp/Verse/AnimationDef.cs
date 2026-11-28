using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x02000111 RID: 273
	public class AnimationDef : Def
	{
		// Token: 0x0600095E RID: 2398 RVA: 0x000358C8 File Offset: 0x00033AC8
		public bool TryGetPartForNodeTag(PawnRenderNodeTagDef nodeTag, out AnimationPart part)
		{
			KeyframeAnimationPart keyframeAnimationPart;
			if (this.keyframeParts != null && this.keyframeParts.TryGetValue(nodeTag, out keyframeAnimationPart))
			{
				part = keyframeAnimationPart;
				return true;
			}
			CurveKeyAnimationPart curveKeyAnimationPart;
			if (this.curveParts != null && this.curveParts.TryGetValue(nodeTag, out curveKeyAnimationPart))
			{
				part = curveKeyAnimationPart;
				return true;
			}
			part = null;
			return false;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00035913 File Offset: 0x00033B13
		public override IEnumerable<string> ConfigErrors()
		{
			AnimationDef.<ConfigErrors>d__8 <ConfigErrors>d__ = new AnimationDef.<ConfigErrors>d__8(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00035923 File Offset: 0x00033B23
		[return: TupleElementNames(new string[]
		{
			"state",
			"graphic"
		})]
		public virtual IEnumerable<ValueTuple<GraphicStateDef, Graphic>> GetGraphicStates(PawnRenderNode node)
		{
			AnimationDef.<GetGraphicStates>d__9 <GetGraphicStates>d__ = new AnimationDef.<GetGraphicStates>d__9(-2);
			<GetGraphicStates>d__.<>4__this = this;
			<GetGraphicStates>d__.<>3__node = node;
			return <GetGraphicStates>d__;
		}

		// Token: 0x0400046C RID: 1132
		public int durationTicks;

		// Token: 0x0400046D RID: 1133
		public bool startOnRandomTick;

		// Token: 0x0400046E RID: 1134
		public bool playWhenDowned;

		// Token: 0x0400046F RID: 1135
		public float posRotScale = 1f;

		// Token: 0x04000470 RID: 1136
		public LoopMode loopMode;

		// Token: 0x04000471 RID: 1137
		public Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart> keyframeParts;

		// Token: 0x04000472 RID: 1138
		public Dictionary<PawnRenderNodeTagDef, CurveKeyAnimationPart> curveParts;
	}
}

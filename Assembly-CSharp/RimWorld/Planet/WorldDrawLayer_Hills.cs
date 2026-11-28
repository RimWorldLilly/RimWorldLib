using System;
using System.Collections;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200231A RID: 8986
	public class WorldDrawLayer_Hills : WorldDrawLayer
	{
		// Token: 0x0600DDEE RID: 56814 RVA: 0x0052932C File Offset: 0x0052752C
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer_Hills.<Regenerate>d__6 <Regenerate>d__ = new WorldDrawLayer_Hills.<Regenerate>d__6(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x04008E7E RID: 36478
		private static readonly FloatRange BaseSizeRange = new FloatRange(0.9f, 1.1f);

		// Token: 0x04008E7F RID: 36479
		private static readonly IntVec2 TexturesInAtlas = new IntVec2(2, 2);

		// Token: 0x04008E80 RID: 36480
		private static readonly FloatRange BasePosOffsetRange_SmallHills = new FloatRange(0f, 0.37f);

		// Token: 0x04008E81 RID: 36481
		private static readonly FloatRange BasePosOffsetRange_LargeHills = new FloatRange(0f, 0.2f);

		// Token: 0x04008E82 RID: 36482
		private static readonly FloatRange BasePosOffsetRange_Mountains = new FloatRange(0f, 0.08f);

		// Token: 0x04008E83 RID: 36483
		private static readonly FloatRange BasePosOffsetRange_ImpassableMountains = new FloatRange(0f, 0.08f);
	}
}

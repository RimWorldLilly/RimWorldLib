using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000484 RID: 1156
	public class HediffCompProperties_ThoughtSetter : HediffCompProperties
	{
		// Token: 0x06002033 RID: 8243 RVA: 0x000C20D1 File Offset: 0x000C02D1
		public HediffCompProperties_ThoughtSetter()
		{
			this.compClass = typeof(HediffComp_ThoughtSetter);
		}

		// Token: 0x04001906 RID: 6406
		public ThoughtDef thought;

		// Token: 0x04001907 RID: 6407
		public int moodOffset;

		// Token: 0x04001908 RID: 6408
		public FloatRange moodOffsetRange = FloatRange.Zero;
	}
}

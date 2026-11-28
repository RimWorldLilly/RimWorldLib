using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001943 RID: 6467
	public class CompProperties_ProximityLetter : CompProperties
	{
		// Token: 0x0600A245 RID: 41541 RVA: 0x003A0011 File Offset: 0x0039E211
		public CompProperties_ProximityLetter()
		{
			this.compClass = typeof(CompProximityLetter);
		}

		// Token: 0x04005FE1 RID: 24545
		public int radius = 4;

		// Token: 0x04005FE2 RID: 24546
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04005FE3 RID: 24547
		[MustTranslate]
		public string letterText;

		// Token: 0x04005FE4 RID: 24548
		public LetterDef letterDef;
	}
}

using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001947 RID: 6471
	public class CompProperties_Psylinkable : CompProperties
	{
		// Token: 0x0600A257 RID: 41559 RVA: 0x003A0553 File Offset: 0x0039E753
		public CompProperties_Psylinkable()
		{
			this.compClass = typeof(CompPsylinkable);
		}

		// Token: 0x04005FE8 RID: 24552
		public List<int> requiredSubplantCountPerPsylinkLevel;

		// Token: 0x04005FE9 RID: 24553
		public MeditationFocusDef requiredFocus;

		// Token: 0x04005FEA RID: 24554
		public SoundDef linkSound;

		// Token: 0x04005FEB RID: 24555
		[MustTranslate]
		public string enoughPlantsLetterLabel;

		// Token: 0x04005FEC RID: 24556
		[MustTranslate]
		public string enoughPlantsLetterText;
	}
}

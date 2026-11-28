using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF3 RID: 4083
	public class StyleItemSpawningProperties : IExposable
	{
		// Token: 0x06006722 RID: 26402 RVA: 0x00248B0E File Offset: 0x00246D0E
		public void ExposeData()
		{
			Scribe_Values.Look<StyleItemFrequency>(ref this.frequency, "frequency", StyleItemFrequency.Never, false);
			Scribe_Values.Look<StyleGender>(ref this.gender, "gender", StyleGender.Male, false);
		}

		// Token: 0x040040D7 RID: 16599
		public StyleItemFrequency frequency;

		// Token: 0x040040D8 RID: 16600
		public StyleGender gender = StyleGender.Any;
	}
}
